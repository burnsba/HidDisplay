using System;
using System.Collections.Generic;
using System.Text;

namespace WinApi.User32
{
    /// <summary>
    /// Defines the raw input data coming from the specified Human Interface Device (HID).
    /// </summary>
    /// <remarks>
    /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-rid_device_info_hid
    /// https://www.pinvoke.net/default.aspx/user32/GetRawInputDeviceInfo%20.html
    /// </remarks>
    public struct RidDeviceInfoHid
    {
        /// <summary>
        /// The vendor identifier for the HID.
        /// </summary>
        public uint VendorID;

        /// <summary>
        /// The product identifier for the HID.
        /// </summary>
        public uint ProductID;

        /// <summary>
        /// The version number for the HID.
        /// </summary>
        public uint VersionNumber;

        /// <summary>
        /// The top-level collection Usage Page for the device.
        /// </summary>
        public ushort UsagePage;

        /// <summary>
        /// The top-level collection Usage for the device.
        /// </summary>
        public ushort Usage;
    }
}
