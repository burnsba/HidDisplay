using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace WinApi.User32
{
    /// <summary>
    /// Defines information for the raw input devices.
    /// </summary>
    /// <remarks>
    /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-rawinputdevice
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct RawInputDevice
    {
        /// <summary>
        /// Top level collection Usage page for the raw input device.
        /// </summary>
        public Hid.HidUsagePages UsagePage;

        /// <summary>
        /// Top level collection Usage for the raw input device.
        /// </summary>
        public ushort Usage;

        /// <summary>
        /// Mode flag that specifies how to interpret the information provided by UsagePage and Usage.
        /// </summary>
        public RawInputDeviceFlags Flags;

        /// <summary>
        /// Handle to the target device. If NULL, it follows the keyboard focus.
        /// This is different from the handle obtained by calling
        /// <see cref="Kernel32.Api.CreateFile(string, uint, uint, IntPtr, uint, uint, IntPtr)"/>.
        /// </summary>
        public IntPtr WindowHandle;

        public override string ToString()
        {
            return $"{Utility.UsagePageAndUsageToString((int)UsagePage, Usage)}, flags: {Flags}, target: {WindowHandle}";
        }
    }
}
