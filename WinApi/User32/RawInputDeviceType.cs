using System;
using System.Collections.Generic;
using System.Text;

namespace WinApi.User32
{
    /// <summary>
    /// Type flag for <see cref="RidDeviceInfo"/>.
    /// </summary>
    /// <remarks>
    /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-rid_device_info
    /// </remarks>
    public enum RawInputDeviceType : uint
    {
        /// <summary>
        /// Data comes from a mouse. 
        /// </summary>
        Mouse = 0,

        /// <summary>
        /// Data comes from a keyboard. 
        /// </summary>
        Keyboard = 1,

        /// <summary>
        /// Data comes from an HID that is not a keyboard or a mouse. 
        /// </summary>
        Hid = 2
    }
}
