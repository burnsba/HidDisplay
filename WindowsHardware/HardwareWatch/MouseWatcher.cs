using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using WinApi.User32;
using WinApi.Windows;
using WindowsHardwareWatch.HardwareWatch.Enums;
using WindowsHardwareWatch.Windows;

namespace WindowsHardwareWatch.HardwareWatch
{
    /// <summary>
    /// Hook mouse events and notify changes.
    /// </summary>
    public class MouseWatcher : LowLevelWatcher, IDisposable
    {
        private WindowsPoint _previousePosition = new WindowsPoint(0,0);

        private delegate IntPtr LowLevelMouseProc(int nCode, WindowsMessages wParam, IntPtr lParam);

        // this needs to be an instance member, otherwise it gets garbage collected away.
        private LowLevelMouseProc _llmp = null;

        public delegate void MouseChangeEventHandler(object sender, MouseChangeEventArgs args);
        public event MouseChangeEventHandler MouseChangeEvent;

        public DateTime LastMessageReceived { get; private set; } = DateTime.MinValue;

        /// <inheritdoc />
        protected override IntPtr SetWindowsHook()
        {
            _llmp = new LowLevelMouseProc(LowLevelMouseHook);

            var handle = IntPtr.Zero;

            using (var currentProcess = Process.GetCurrentProcess())
            using (var currentModule = currentProcess.MainModule)
            {
                var module = WinApi.Kernel32.Api.GetModuleHandle(currentModule.ModuleName);
                handle = WinApi.User32.Api.SetWindowsHookEx(SetWindowsHookExType.WH_MOUSE_LL, _llmp, module, 0);
            }

            return handle;
        }

        /// <summary>
        /// Handler to run on mouse change.
        /// </summary>
        /// <param name="nCode">Event code.</param>
        /// <param name="wParam">Type of windows message.</param>
        /// <param name="lParam">Keyboard status struct.</param>
        /// <returns>Pointer to next callback.</returns>
        private IntPtr LowLevelMouseHook(int nCode, WindowsMessages wParam, IntPtr lParam)
        {
            MouseLowLevelHookStruct mouse = (MouseLowLevelHookStruct)Marshal.PtrToStructure(lParam, typeof(MouseLowLevelHookStruct));

            if (nCode >= 0 && FromAllowedWindow())
            {
                LastMessageReceived = DateTime.Now;

                RaiseEvent(BuildEventArgs(wParam, mouse));
            }

            return WinApi.User32.Api.CallNextHookEx(_hook, nCode, (IntPtr)wParam, lParam);
        }

        /// <summary>
        /// Fires the event to any listeners.
        /// </summary>
        /// <param name="args">Arguments to send.</param>
        private void RaiseEvent(MouseChangeEventArgs args)
        {
            if (!object.ReferenceEquals(null, MouseChangeEvent))
            {
                MouseChangeEvent.Invoke(this, args);
            }
        }

        /// <summary>
        /// Parses event and builds event args.
        /// </summary>
        /// <param name="wParam">Type of event.</param>
        /// <param name="mouse">Mouse event properties.</param>
        /// <returns>Strongly typed event info.</returns>
        private MouseChangeEventArgs BuildEventArgs(WindowsMessages wParam, MouseLowLevelHookStruct mouse)
        {
            var args = new MouseChangeEventArgs()
            {
                Button = ButtonSource.Unknown,
                ButtonDirection = ButtonChangeDirection.Unknown,
                DeltaPosition = WindowsPoint.Invalid,
                EventType = MouseEventType.Unknown,
                NewPosition = WindowsPoint.Invalid,
                Scroll = ScrollSource.Unknown,
                ScrollDirection = ScrollChangeDirection.Unknown,
            };

            if (wParam == WindowsMessages.MOUSEWHEEL)
            {
                args.EventType = MouseEventType.Scroll;
                args.Scroll = ScrollSource.VerticalScrollWheel;
                if (mouse.mouseData > 0)
                {
                    args.ScrollDirection = ScrollChangeDirection.Up;
                }
                else
                {
                    args.ScrollDirection = ScrollChangeDirection.Down;
                }
            }
            else if (wParam == WindowsMessages.MOUSEHWHEEL)
            {
                args.EventType = MouseEventType.Scroll;
                args.Scroll = ScrollSource.HorizontalScrollWheel;
                if (mouse.mouseData > 0)
                {
                    args.ScrollDirection = ScrollChangeDirection.Up;
                }
                else
                {
                    args.ScrollDirection = ScrollChangeDirection.Down;
                }
            }
            else if (wParam == WindowsMessages.MOUSEMOVE)
            {
                args.EventType = MouseEventType.Move;
                args.DeltaPosition = _previousePosition.Delta(mouse.pt);
                _previousePosition = mouse.pt;
                args.NewPosition = new WindowsPoint(mouse.pt.X, mouse.pt.Y);
            }
            else if (wParam == WindowsMessages.LBUTTONDOWN)
            {
                args.EventType = MouseEventType.Button;
                args.Button = ButtonSource.LeftButton;
                args.ButtonDirection = ButtonChangeDirection.Down;
            }
            else if (wParam == WindowsMessages.LBUTTONUP)
            {
                args.EventType = MouseEventType.Button;
                args.Button = ButtonSource.LeftButton;
                args.ButtonDirection = ButtonChangeDirection.Up;
            }
            else if (wParam == WindowsMessages.RBUTTONDOWN)
            {
                args.EventType = MouseEventType.Button;
                args.Button = ButtonSource.RightButton;
                args.ButtonDirection = ButtonChangeDirection.Down;
            }
            else if (wParam == WindowsMessages.RBUTTONUP)
            {
                args.EventType = MouseEventType.Button;
                args.Button = ButtonSource.RightButton;
                args.ButtonDirection = ButtonChangeDirection.Up;
            }
            else if (wParam == WindowsMessages.MBUTTONDOWN)
            {
                args.EventType = MouseEventType.Button;
                args.Button = ButtonSource.MiddleButton;
                args.ButtonDirection = ButtonChangeDirection.Down;
            }
            else if (wParam == WindowsMessages.MBUTTONUP)
            {
                args.EventType = MouseEventType.Button;
                args.Button = ButtonSource.MiddleButton;
                args.ButtonDirection = ButtonChangeDirection.Up;
            }
            else if (wParam == WindowsMessages.XBUTTONDOWN)
            {
                args.EventType = MouseEventType.Button;
                args.ButtonDirection = ButtonChangeDirection.Down;

                if ((mouse.flags & 0x01) > 0 && (mouse.mouseData & 0x10000) > 0)
                {
                    args.Button = ButtonSource.Mouse4;
                }
                else if ((mouse.flags & 0x01) > 0 && (mouse.mouseData & 0x20000) > 0)
                {
                    args.Button = ButtonSource.Mouse5;
                }
            }
            else if (wParam == WindowsMessages.XBUTTONUP)
            {
                args.EventType = MouseEventType.Button;
                args.ButtonDirection = ButtonChangeDirection.Up;

                if ((mouse.flags & 0x01) > 0 && (mouse.mouseData & 0x10000) > 0)
                {
                    args.Button = ButtonSource.Mouse4;
                }
                else if ((mouse.flags & 0x01) > 0 && (mouse.mouseData & 0x20000) > 0)
                {
                    args.Button = ButtonSource.Mouse5;
                }
            }

            return args;
        }
    }
}
