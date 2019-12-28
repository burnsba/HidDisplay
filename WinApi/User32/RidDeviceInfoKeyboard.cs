using System;
using System.Collections.Generic;
using System.Text;

namespace WinApi.User32
{
    /// <summary>
    /// Defines the raw input data coming from the specified keyboard.
    /// </summary>
    /// <remarks>
    /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-rid_device_info_keyboard
    /// https://www.pinvoke.net/default.aspx/user32/GetRawInputDeviceInfo%20.html
    /// </remarks>
    public struct RidDeviceInfoKeyboard
    {
        /// <summary>
        /// The type of the keyboard.
        /// </summary>
        public uint Type;

        /// <summary>
        /// The subtype of the keyboard.
        /// </summary>
        public uint SubType;

        /// <summary>
        /// The scan code mode.
        /// </summary>
        public uint KeyboardMode;

        /// <summary>
        /// The number of function keys on the keyboard.
        /// </summary>
        public uint NumberOfFunctionKeys;

        /// <summary>
        /// The number of LED indicators on the keyboard.
        /// </summary>
        public uint NumberOfIndicators;

        /// <summary>
        /// The total number of keys on the keyboard.
        /// </summary>
        public uint NumberOfKeysTotal;
    }
}
