using System;
using System.Collections.Generic;
using System.Text;

namespace WinApi.User32
{
    /// <summary>
    /// Flags used by <see cref="KeyboardLowLevelHookStruct"/>.
    /// </summary>
    /// <remarks>
    /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-kbdllhookstruct
    /// </remarks>
    [Flags]
    public enum KeyboardLowLevelHookStructFlags : uint
    {
        /// <summary>
        /// Test the extended-key flag. 
        /// </summary>
        LLKHF_EXTENDED = 0x01,

        /// <summary>
        /// Test the event-injected (from a process running at lower integrity level) flag. 
        /// </summary>
        LLKHF_LOWER_IL_INJECTED = 0x02,

        /// <summary>
        /// Test the event-injected (from any process) flag. 
        /// </summary>
        LLKHF_INJECTED = 0x10,

        /// <summary>
        /// Test the context code. 
        /// </summary>
        LLKHF_ALTDOWN = 0x20,

        /// <summary>
        /// Test the transition-state flag. 
        /// </summary>
        LLKHF_UP = 0x80,
    }
}
