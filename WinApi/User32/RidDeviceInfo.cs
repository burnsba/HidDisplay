using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace WinApi.User32
{
    /// <summary>
    /// Defines the raw input data coming from any device.
    /// </summary>
    /// <remarks>
    /// https://www.pinvoke.net/default.aspx/user32/GetRawInputDeviceInfo%20.html
    /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-rid_device_info
    /// </remarks>
    [StructLayout(LayoutKind.Explicit)]
    public struct RidDeviceInfo
    {
        /// <summary>
        /// The size, in bytes, of the <see cref="RidDeviceInfo"/> structure.
        /// </summary>
        [FieldOffset(0)]
        public int Size;

        /// <summary>
        /// The type of raw input data.
        /// </summary>
        [FieldOffset(4)]
        [MarshalAs(UnmanagedType.U4)]
        public RawInputDeviceType Type;

        /// <summary>
        /// If <see cref="RidDeviceInfo.Type"/> is <see cref="RawInputDeviceType.Mouse"/>, 
        /// this is the <see cref="RidDeviceInfoMouse"/> structure that defines the mouse.
        /// </summary>
        [FieldOffset(8)]
        public RidDeviceInfoMouse MouseInfo;

        /// <summary>
        /// If <see cref="RidDeviceInfo.Type"/> is <see cref="RawInputDeviceType.Keyboard"/>, 
        /// this is the <see cref="RidDeviceInfoKeyboard"/> structure that defines the keyboard.
        /// </summary>
        [FieldOffset(8)]
        public RidDeviceInfoKeyboard KeyboardInfo;

        /// <summary>
        /// If <see cref="RidDeviceInfo.Type"/> is <see cref="RawInputDeviceType.Hid"/>, 
        /// this is the <see cref="RidDeviceInfoHid"/> structure that defines the HID device.
        /// </summary>
        [FieldOffset(8)]
        public RidDeviceInfoHid HidInfo;
    }
}
