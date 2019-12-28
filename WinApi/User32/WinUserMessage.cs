using System;
using System.Collections.Generic;
using System.Text;

namespace WinApi.User32
{
    /// <summary>
    /// Contains message information from a thread's message queue.
    /// </summary>
    /// <remarks>
    /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-msg
    /// </remarks>
    [Serializable]
    public struct WinUserMessage
    {
        /// <summary>
        /// A handle to the window whose window procedure receives the message. This member is NULL when the message is a thread message.
        /// </summary>
        public IntPtr hwnd;

        /// <summary>
        /// The message identifier. Applications can only use the low word; the high word is reserved by the system.
        /// </summary>
        public int message;

        /// <summary>
        /// Additional information about the message. The exact meaning depends on the value of the message member.
        /// </summary>
        public IntPtr wParam;

        /// <summary>
        /// Additional information about the message. The exact meaning depends on the value of the message member.
        /// </summary>
        public IntPtr lParam;

        /// <summary>
        /// The time at which the message was posted.
        /// </summary>
        public int time;

        /// <summary>
        /// The cursor x position, in screen coordinates, when the message was posted.
        /// </summary>
        public int pt_x;

        /// <summary>
        /// The cursor y position, in screen coordinates, when the message was posted.
        /// </summary>
        public int pt_y;

        /// <summary>
        /// undocumented.
        /// </summary>
        internal uint lprivate;
    }
}
