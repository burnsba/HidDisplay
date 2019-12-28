using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace WinApi.User32
{
    /// <summary>
    /// Contains information about a raw input device.
    /// </summary>
    /// <remarks>
    /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-rawinputdevicelist
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct RawInputDeviceList
    {
        /// <summary>
        /// A handle to the device generating the raw input data.
        /// This is different from the handle obtained by
        /// calling <see cref="Kernel32.Api.CreateFile(string, uint, uint, IntPtr, uint, uint, IntPtr)"/>.
        /// </summary>
        public IntPtr hDevice;

        /// <summary>
        /// The type of device.
        /// </summary>
        public RawInputDeviceType Type;
    }
}
