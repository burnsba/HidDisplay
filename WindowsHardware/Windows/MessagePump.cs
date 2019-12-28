using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using WinApi.User32;
using WinApi.Windows;

namespace WindowsHardwareWatch.Windows
{
    /// <summary>
    /// Helper wrapper to capture windows events in this application.
    /// </summary>
    /// <remarks>
    /// Yes! of course there is a message pump, windows receives and sends messages
    /// all the time, everything within windows is based on messages but when you
    /// create a console application and you want to listen to the messages you need
    /// to get an access to the message loop.
    /// .
    /// https://social.msdn.microsoft.com/Forums/vstudio/en-US/ed5be22c-cef8-4615-a625-d05caf113afc/console-keyboard-hook-not-getting-called?forum=csharpgeneral
    /// </remarks>
    public class MessagePump : IEnumerable<bool>, IEnumerable
    {
        private static MessagePump _instance;
        private static WinUserMessage _message;
        private static bool _shutdown = false;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static MessagePump Instance
        {
            get
            {
                if (object.ReferenceEquals(null, _instance))
                {
                    _instance = new MessagePump();
                }

                return _instance;
            }
        }

        /// <summary>
        /// Instantiates a new instance of <see cref="MessagePump" />.
        /// </summary>
        private MessagePump()
        { }

        /// <summary>
        /// Stops iterator.
        /// </summary>
        public void Shutdown()
        {
            _shutdown = true;
        }

        /// <summary>
        /// Allows iterator to continue.
        /// </summary>
        public void Restart()
        {
            _shutdown = false;
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<bool>)Instance).GetEnumerator();
        }

        /// <inheritdoc />
        public IEnumerator<bool> GetEnumerator()
        {
            var hwnd = Process.GetCurrentProcess().MainWindowHandle;
            int messageResult = 1;

            uint timeout = 10;

            bool waitTimeout = false;

            while (!_shutdown && messageResult > 0)
            {
                waitTimeout = false;

                // Create a timer in case this gets stuck waiting for a message that never arrives.
                // https://stackoverflow.com/a/10866328/1462295
                IntPtr timerId = WinApi.User32.Api.SetTimer(IntPtr.Zero, IntPtr.Zero, timeout, IntPtr.Zero);
                messageResult = WinApi.User32.Api.GetMessage(out _message, hwnd, 0, 0);
                WinApi.User32.Api.KillTimer(IntPtr.Zero, timerId);

                switch ((WindowsMessages)_message.message)
                {
                    // Only process the mouse and keyboard events.
                    case WindowsMessages.MOUSEWHEEL: // fallthrough
                    case WindowsMessages.MOUSEHWHEEL: // fallthrough
                    case WindowsMessages.MOUSEMOVE: // fallthrough
                    case WindowsMessages.LBUTTONDOWN: // fallthrough
                    case WindowsMessages.LBUTTONUP: // fallthrough
                    case WindowsMessages.RBUTTONDOWN: // fallthrough
                    case WindowsMessages.RBUTTONUP: // fallthrough
                    case WindowsMessages.MBUTTONDOWN: // fallthrough
                    case WindowsMessages.MBUTTONUP: // fallthrough
                    case WindowsMessages.XBUTTONDOWN: // fallthrough
                    case WindowsMessages.XBUTTONUP: // fallthrough
                    case WindowsMessages.KEYDOWN: // fallthrough
                    case WindowsMessages.KEYUP: // fallthrough
                    case WindowsMessages.SYSKEYDOWN: // fallthrough
                    case WindowsMessages.SYSKEYUP: // fallthrough
                        WinApi.User32.Api.TranslateMessage(ref _message);
                        WinApi.User32.Api.DispatchMessage(ref _message);

                        break;

                    case WindowsMessages.TIMER:
                        waitTimeout = true;
                        break;

                    default:
                        // do nothing
                        break;
                }

                if (waitTimeout)
                {
                    yield return false;
                }
                else
                {
                    yield return true;
                }
            }

            yield return false;
        }

        //[Serializable]
        //private struct WindowsMessage
        //{
        //    public IntPtr hwnd;
        //    public IntPtr lParam;
        //    public int message;
        //    public int pt_x;
        //    public int pt_y;
        //    public int time;
        //    public IntPtr wParam;
        //}
    }
}
