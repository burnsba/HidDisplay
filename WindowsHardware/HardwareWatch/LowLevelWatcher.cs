using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using WindowsHardwareWatch.HardwareWatch.Enums;
using WindowsHardwareWatch.Windows;

namespace WindowsHardwareWatch.HardwareWatch
{
    /// <summary>
    /// Common base class for hooking windows hardware events.
    /// </summary>
    public abstract class LowLevelWatcher : IDisposable
    {
        protected const int WindowMaxTitleLength = 256;

        protected string _windowTitle;
        protected Regex _windowTitleRegex = null;
        protected WindowTitleMatch _titleMatch;

        protected IntPtr _hook = IntPtr.Zero;

        /// <summary>
        /// Pass through / pre-setup to hook windows event.
        /// </summary>
        /// <param name="titleMatch">How to match window title.</param>
        /// <param name="windowTitle">Text (or regex) to match window title.</param>
        public void Setup(WindowTitleMatch titleMatch, string windowTitle)
        {
            if (string.IsNullOrEmpty(windowTitle))
            {
                throw new ArgumentException($"{nameof(windowTitle)} must be set");
            }

            if (titleMatch == WindowTitleMatch.Unknown)
            {
                throw new ArgumentException($"{nameof(titleMatch)} must be set");
            }

            if (titleMatch == WindowTitleMatch.Regex)
            {
                _windowTitleRegex = new Regex(windowTitle);
            }

            _titleMatch = titleMatch;
            _windowTitle = windowTitle;

            // Now hook event.
            _hook = SetWindowsHook();
        }

        /// <summary>
        /// Helper function. Returns title of currently active window.
        /// </summary>
        /// <returns>String or null.</returns>
        /// <remarks>
        /// https://stackoverflow.com/a/115905/1462295
        /// .
        /// </remarks>
        protected string GetActiveWindowTitle()
        {
            var buffer = new StringBuilder(WindowMaxTitleLength);
            var handle = WinApi.User32.Api.GetForegroundWindow();

            if (WinApi.User32.Api.GetWindowText(handle, buffer, WindowMaxTitleLength) > 0)
            {
                return buffer.ToString();
            }

            return null;
        }

        /// <summary>
        /// Determines whether the current active window is the one to listen to.
        /// </summary>
        /// <returns>True if so, false otherwise.</returns>
        protected bool FromAllowedWindow()
        {
            var currentWindowTitle = GetActiveWindowTitle() ?? string.Empty;

            switch (_titleMatch)
            {
                case WindowTitleMatch.BeginsWith:
                    return currentWindowTitle.IndexOf(_windowTitle, StringComparison.CurrentCultureIgnoreCase) == 0;

                case WindowTitleMatch.Contains:
                    return currentWindowTitle.IndexOf(_windowTitle, StringComparison.CurrentCultureIgnoreCase) >= 0;

                case WindowTitleMatch.EndsWith:
                    return currentWindowTitle.EndsWith(_windowTitle, StringComparison.CurrentCultureIgnoreCase);

                case WindowTitleMatch.Exact:
                    return string.Compare(_windowTitle, currentWindowTitle, true) == 0;

                case WindowTitleMatch.Regex:
                    return _windowTitleRegex.IsMatch(currentWindowTitle);

                default:
                    return false;
            }
        }

        /// <summary>
        /// Hooks windows event for the specific handler.
        /// </summary>
        /// <returns>Pointer to hook handler.</returns>
        protected abstract IntPtr SetWindowsHook();

        /// <summary>
        /// Unhooks keyboard listener.
        /// </summary>
        public void Dispose()
        {
            try
            {
                if (!object.ReferenceEquals(null, _hook) && !IntPtr.Zero.Equals(_hook))
                {
                    if (WinApi.User32.Api.UnhookWindowsHookEx(_hook))
                    {
                        _hook = IntPtr.Zero;
                    }
                }
            }
            catch
            { }
        }
    }
}
