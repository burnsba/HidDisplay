using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace WinApi.User32
{
    /// <summary>
    /// Defines the raw input data coming from the specified mouse.
    /// </summary>
    /// <remarks>
    /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-rid_device_info_mouse
    /// https://www.pinvoke.net/default.aspx/user32/GetRawInputDeviceInfo%20.html
    /// </remarks>
    public struct RidDeviceInfoMouse
    {
        /// <summary>
        /// The identifier of the mouse device.
        /// </summary>
        public uint ID;

        /// <summary>
        /// The number of buttons for the mouse.
        /// </summary>
        public uint NumberOfButtons;

        /// <summary>
        /// The number of data points per second. This information may not be applicable for every mouse device.
        /// </summary>
        public uint SampleRate;

        /// <summary>
        /// TRUE if the mouse has a wheel for horizontal scrolling; otherwise, FALSE.
        /// </summary>
        [MarshalAs(UnmanagedType.U1)]
        public bool HasHorizontalWheel;
    }
}
