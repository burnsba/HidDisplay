using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace WinApi.User32
{
    /// <summary>
    /// The KBDLLHOOKSTRUCT structure contains information about a low-level keyboard input event.
    /// </summary>
    /// <remarks>
    /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-kbdllhookstruct
    /// http://www.pinvoke.net/default.aspx/Structures/KBDLLHOOKSTRUCT.html
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    public class KeyboardLowLevelHookStruct
    {
        /// <summary>
        /// A virtual-key code. The code must be a value in the range 1 to 254.
        /// </summary>
        public uint vkCode;

        /// <summary>
        /// A hardware scan code for the key.
        /// </summary>
        public uint scanCode;

        /// <summary>
        /// The extended-key flag, event-injected flags, context code, and transition-state
        /// flag. This member is specified as follows. An application can use the following
        /// values to test the keystroke flags. Testing <see cref="KeyboardLowLevelHookStructFlags.LLKHF_INJECTED"/> (bit 4) will tell you
        /// whether the event was injected. If it was, then testing <see cref="KeyboardLowLevelHookStructFlags.LLKHF_LOWER_IL_INJECTED"/>
        /// (bit 1) will tell you whether or not the event was injected from a process
        /// running at lower integrity level.
        /// </summary>
        public KeyboardLowLevelHookStructFlags flags;

        /// <summary>
        /// The time stamp for this message, equivalent to what GetMessageTime would return for this message.
        /// </summary>
        public uint time;

        /// <summary>
        /// Additional information associated with the message.
        /// </summary>
        public UIntPtr dwExtraInfo;
    }
}
