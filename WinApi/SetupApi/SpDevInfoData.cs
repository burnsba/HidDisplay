using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace WinApi.SetupApi
{
    /// <summary>
    /// An SP_DEVINFO_DATA structure defines a device instance that is a member of a device information set.
    /// </summary>
    /// <remarks>
    /// https://docs.microsoft.com/en-us/windows/win32/api/setupapi/ns-setupapi-sp_devinfo_data
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct SpDevInfoData
    {
        /// <summary>
        /// The size, in bytes, of the SP_DEVINFO_DATA structure.
        /// </summary>
        public uint cbSize;

        /// <summary>
        /// The GUID of the device's setup class.
        /// </summary>
        public Guid ClassGuid;

        /// <summary>
        /// An opaque handle to the device instance (also known as a handle to the devnode).
        /// Some functions, such as SetupDiXxx functions, take the whole SP_DEVINFO_DATA 
        /// structure as input to identify a device in a device information set. Other 
        /// functions, such as CM_Xxx functions like CM_Get_DevNode_Status, take this
        /// DevInst handle as input.
        /// </summary>
        public uint DevInst;

        /// <summary>
        /// Reserved. For internal use only.
        /// </summary>
        public UIntPtr Reserved;
    }
}
