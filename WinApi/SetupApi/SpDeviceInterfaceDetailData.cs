using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace WinApi.SetupApi
{
    /// <summary>
    /// An SP_DEVICE_INTERFACE_DETAIL_DATA structure contains the path for a device interface.
    /// </summary>
    /// <remarks>
    /// https://docs.microsoft.com/en-us/windows/win32/api/setupapi/ns-setupapi-sp_device_interface_detail_data_a
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct SpDeviceInterfaceDetailData
    {
        /// <summary>
        /// The size, in bytes, of the SP_DEVICE_INTERFACE_DETAIL_DATA structure. For more information, see the following Remarks section.
        /// </summary>
        public uint cbSize;

        /// <summary>
        /// A NULL-terminated string that contains the device interface path. This path can be passed to Win32 functions such as CreateFile.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2048)]
        public string DevicePath;
    }
}
