using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace WinApi.SetupApi
{
    /// <summary>
    /// An <see cref="SpDeviceInterfaceData"/> structure defines a device interface in a device information set.
    /// </summary>
    /// <remarks>
    /// https://docs.microsoft.com/en-us/windows/win32/api/setupapi/ns-setupapi-sp_device_interface_data
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct SpDeviceInterfaceData
    {
        /// <summary>
        /// The size, in bytes, of the <see cref="SpDeviceInterfaceData"/> structure. For more information, see the Remarks section.
        /// </summary>
        public uint cbSize;

        /// <summary>
        /// The GUID for the class to which the device interface belongs.
        /// </summary>
        public Guid InterfaceClassGuid;

        /// <summary>
        /// SPINT_ACTIVE, SPINT_DEFAULT, SPINT_REMOVED
        /// </summary>
        public uint Flags;

        /// <summary>
        /// Reserved. Do not use.
        /// </summary>
        public UIntPtr Reserved;
    }
}
