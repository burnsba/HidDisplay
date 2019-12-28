using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace WinApi.SetupApi
{
    /// <summary>
    /// Function definitions for setupapi.dll.
    /// </summary>
    public static class Api
    {
        /// <summary>
        /// The CM_Get_Device_ID function retrieves the device instance ID for a specified device instance on the local machine.
        /// </summary>
        /// <param name="dnDevInst">
        /// Caller-supplied device instance handle that is bound to the local machine.
        /// </param>
        /// <param name="Buffer">
        /// Address of a buffer to receive a device instance ID string. The required buffer size can be obtained by
        /// calling CM_Get_Device_ID_Size, then incrementing the received value to allow room for the string's
        /// terminating NULL.</param>
        /// <param name="BufferLen">
        /// Caller-supplied length, in characters, of the buffer specified by Buffer.
        /// </param>
        /// <param name="ulFlags">
        /// Not used, must be zero.
        /// </param>
        /// <returns>
        /// If the operation succeeds, the function returns CR_SUCCESS. Otherwise, it returns one
        /// of the CR_-prefixed error codes defined in Cfgmgr32.h.</returns>
        /// <remarks>
        /// https://docs.microsoft.com/en-us/windows/win32/api/cfgmgr32/nf-cfgmgr32-cm_get_device_idw
        /// </remarks>
        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern int CM_Get_Device_IDW(
            IntPtr dnDevInst,
            IntPtr Buffer,
            uint BufferLen,
            uint ulFlags
        );


        /// <summary>
        /// The CM_Get_Parent function obtains a device instance handle to the parent node of
        /// a specified device node (devnode) in the local machine's device tree.
        /// </summary>
        /// <param name="pdnDevInst">
        /// Caller-supplied pointer to the device instance handle to the parent node
        /// that this function retrieves. The retrieved handle is bound to the local machine.
        /// </param>
        /// <param name="dnDevInst">
        /// Caller-supplied device instance handle that is bound to the local machine.
        /// </param>
        /// <param name="ulFlags">
        /// Not used, must be zero.
        /// </param>
        /// <returns>
        /// If the operation succeeds, the function returns CR_SUCCESS. Otherwise, it returns
        /// one of the CR_-prefixed error codes defined in Cfgmgr32.h.
        /// </returns>
        /// <remarks>
        /// https://docs.microsoft.com/en-us/windows/win32/api/cfgmgr32/nf-cfgmgr32-cm_get_parent
        /// </remarks>
        [DllImport("setupapi.dll")]
        public static extern int CM_Get_Parent(
            out IntPtr pdnDevInst,
            UInt32 dnDevInst,
            int ulFlags
        );

        /// <summary>
        /// The SetupDiDestroyDeviceInfoList function deletes a device information set and frees all associated memory.
        /// </summary>
        /// <param name="deviceInfoSet">A handle to the device information set to delete.</param>
        /// <returns>The function returns TRUE if it is successful. Otherwise, it returns FALSE and the
        /// logged error can be retrieved with a call to GetLastError.</returns>
        /// <remarks>
        /// https://docs.microsoft.com/en-us/windows/win32/api/setupapi/nf-setupapi-setupdidestroydeviceinfolist
        /// </remarks>
        [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
        public static extern bool SetupDiDestroyDeviceInfoList(
            IntPtr deviceInfoSet
        );

        /// <summary>
        /// The SetupDiEnumDeviceInterfaces function enumerates the device interfaces that
        /// are contained in a device information set.
        /// </summary>
        /// <param name="hDevInfo">
        /// A pointer to a device information set that contains the device interfaces for
        /// which to return information. This handle is typically returned by SetupDiGetClassDevs.
        /// </param>
        /// <param name="devInfo">
        /// A pointer to an <see cref="SpDevInfoData"/> structure that specifies a device information element
        /// in DeviceInfoSet. This parameter is optional and can be NULL. If this parameter is
        /// specified, SetupDiEnumDeviceInterfaces constrains the enumeration to the interfaces
        /// that are supported by the specified device. If this parameter is NULL, repeated calls
        /// to SetupDiEnumDeviceInterfaces return information about the interfaces that are
        /// associated with all the device information elements in DeviceInfoSet. This pointer
        /// is typically returned by SetupDiEnumDeviceInfo.
        /// </param>
        /// <param name="interfaceClassGuid">
        /// A pointer to a GUID that specifies the device interface class for the requested interface.
        /// </param>
        /// <param name="memberIndex">
        /// A zero-based index into the list of interfaces in the device information set. The caller
        /// should call this function first with MemberIndex set to zero to obtain the first
        /// interface. Then, repeatedly increment MemberIndex and retrieve an interface until
        /// this function fails and GetLastError returns ERROR_NO_MORE_ITEMS.
        /// If DeviceInfoData specifies a particular device, the MemberIndex is relative to only
        /// the interfaces exposed by that device.
        /// </param>
        /// <param name="deviceInterfaceData">
        /// A pointer to a caller-allocated buffer that contains, on successful return, a completed
        /// <see cref="SpDeviceInterfaceData" /> structure that identifies an interface that meets the search
        /// parameters. The caller must set DeviceInterfaceData.cbSize to sizeof(<see cref="SpDeviceInterfaceData" />)
        /// before calling this function.
        /// </param>
        /// <returns>
        /// SetupDiEnumDeviceInterfaces returns TRUE if the function completed without error. If
        /// the function completed with an error, FALSE is returned and the error code for the
        /// failure can be retrieved by calling GetLastError.
        /// </returns>
        /// <remarks>
        /// https://docs.microsoft.com/en-us/windows/win32/api/setupapi/nf-setupapi-setupdienumdeviceinterfaces
        /// </remarks>
        [DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Boolean SetupDiEnumDeviceInterfaces(
            IntPtr hDevInfo,
            IntPtr devInfo,
            ref Guid interfaceClassGuid,
            UInt32 memberIndex,
            ref SpDeviceInterfaceData deviceInterfaceData
        );

        /// <summary>
        /// The SetupDiEnumDeviceInterfaces function enumerates the device interfaces that
        /// are contained in a device information set.
        /// </summary>
        /// <param name="hDevInfo">
        /// A pointer to a device information set that contains the device interfaces for
        /// which to return information. This handle is typically returned by SetupDiGetClassDevs.
        /// </param>
        /// <param name="devInfo">
        /// A pointer to an <see cref="SpDevInfoData"/> structure that specifies a device information element
        /// in DeviceInfoSet. This parameter is optional and can be NULL. If this parameter is
        /// specified, SetupDiEnumDeviceInterfaces constrains the enumeration to the interfaces
        /// that are supported by the specified device. If this parameter is NULL, repeated calls
        /// to SetupDiEnumDeviceInterfaces return information about the interfaces that are
        /// associated with all the device information elements in DeviceInfoSet. This pointer
        /// is typically returned by SetupDiEnumDeviceInfo.
        /// </param>
        /// <param name="interfaceClassGuid">
        /// A pointer to a GUID that specifies the device interface class for the requested interface.
        /// </param>
        /// <param name="memberIndex">
        /// A zero-based index into the list of interfaces in the device information set. The caller
        /// should call this function first with MemberIndex set to zero to obtain the first
        /// interface. Then, repeatedly increment MemberIndex and retrieve an interface until
        /// this function fails and GetLastError returns ERROR_NO_MORE_ITEMS.
        /// If DeviceInfoData specifies a particular device, the MemberIndex is relative to only
        /// the interfaces exposed by that device.
        /// </param>
        /// <param name="deviceInterfaceData">
        /// A pointer to a caller-allocated buffer that contains, on successful return, a completed
        /// <see cref="SpDeviceInterfaceData" /> structure that identifies an interface that meets the search
        /// parameters. The caller must set DeviceInterfaceData.cbSize to sizeof(<see cref="SpDeviceInterfaceData" />)
        /// before calling this function.
        /// </param>
        /// <returns>
        /// SetupDiEnumDeviceInterfaces returns TRUE if the function completed without error. If
        /// the function completed with an error, FALSE is returned and the error code for the
        /// failure can be retrieved by calling GetLastError.
        /// </returns>
        /// <remarks>
        /// https://docs.microsoft.com/en-us/windows/win32/api/setupapi/nf-setupapi-setupdienumdeviceinterfaces
        /// </remarks>
        [DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Boolean SetupDiEnumDeviceInterfaces(
            IntPtr hDevInfo,
            ref SpDevInfoData devInfo,
            ref Guid interfaceClassGuid,
            UInt32 memberIndex,
            ref SpDeviceInterfaceData deviceInterfaceData
        );

        /// <summary>
        /// The SetupDiGetClassDevs function returns a handle to a device information set that contains
        /// requested device information elements for a local computer.
        /// </summary>
        /// <param name="classGuid">
        /// A pointer to the GUID for a device setup class or a device interface class. This pointer is
        /// optional and can be NULL. For more information about how to set ClassGuid, see the following
        /// Remarks section.
        /// </param>
        /// <param name="enumerator">
        /// A pointer to a NULL-terminated string that specifies:
        /// <list type="bullet">
        /// <term>
        /// An identifier(ID) of a Plug and Play(PnP) enumerator.This ID can either be the value's
        /// globally unique identifier (GUID) or symbolic name. For example, "PCI" can be used to specify
        /// the PCI PnP value. Other examples of symbolic names for PnP values include "USB," "PCMCIA,"
        /// and "SCSI".
        /// </term>
        /// <term>
        /// A PnP device instance ID. When specifying a PnP device instance ID, DIGCF_DEVICEINTERFACE
        /// must be set in the Flags parameter.
        /// </term>
        /// </list>
        /// This pointer is optional and can be NULL.If an enumeration value is not used to select
        /// devices, set Enumerator to NULL</param>
        /// <param name="hwndParent">
        /// A handle to the top-level window to be used for a user interface that is associated with
        /// installing a device instance in the device information set. This handle is optional and
        /// can be NULL.
        /// </param>
        /// <param name="flags">
        /// A variable of type DWORD that specifies control options that filter the device information
        /// elements that are added to the device information set. This parameter can be a bitwise OR
        /// of zero or more of the following flags.
        /// </param>
        /// <returns>
        /// If the operation succeeds, SetupDiGetClassDevs returns a handle to a device information set
        /// that contains all installed devices that matched the supplied parameters. If the operation
        /// fails, the function returns INVALID_HANDLE_VALUE. To get extended error information, call
        /// GetLastError.
        /// </returns>
        /// <remarks>
        /// https://docs.microsoft.com/en-us/windows/win32/api/setupapi/nf-setupapi-setupdigetclassdevsw
        /// </remarks>
        [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SetupDiGetClassDevsW(
            ref Guid classGuid,
            [MarshalAs(UnmanagedType.LPTStr)] string enumerator,
            IntPtr hwndParent,
            uint flags
        );

        /// <summary>
        /// The SetupDiGetDeviceInterfaceDetail function returns details about a device interface.
        /// </summary>
        /// <param name="hDevInfo">
        /// A pointer to the device information set that contains the interface for which to
        /// retrieve details. This handle is typically returned by SetupDiGetClassDevs.
        /// </param>
        /// <param name="deviceInterfaceData">
        /// A pointer to an <see cref="SpDeviceInterfaceData" /> structure that specifies the interface in
        /// DeviceInfoSet for which to retrieve details. A pointer of this type is typically
        /// returned by SetupDiEnumDeviceInterfaces.
        /// </param>
        /// <param name="deviceInterfaceDetailData">
        /// A pointer to an <see cref="SpDeviceInterfaceDetailData" /> structure to receive information about
        /// the specified interface. This parameter is optional and can be NULL. This parameter must
        /// be NULL if DeviceInterfaceDetailSize is zero. If this parameter is specified, the caller
        /// must set DeviceInterfaceDetailData.cbSize to sizeof(<see cref="SpDeviceInterfaceDetailData" />) before
        /// calling this function. The cbSize member always contains the size of the fixed part of the
        /// data structure, not a size reflecting the variable-length string at the end.
        /// </param>
        /// <param name="deviceInterfaceDetailDataSize">
        /// The size of the DeviceInterfaceDetailData buffer. The buffer must be at least 
        /// (offsetof(<see cref="SpDeviceInterfaceDetailData" />, DevicePath) + sizeof(TCHAR)) bytes, to contain
        /// the fixed part of the structure and a single NULL to terminate an empty MULTI_SZ string.
        /// This parameter must be zero if <paramref name="deviceInterfaceDetailData"/> is NULL.
        /// </param>
        /// <param name="requiredSize">
        /// A pointer to a variable of type DWORD that receives the required size of the
        /// <paramref name="deviceInterfaceDetailData"/> buffer. This size includes the size of the fixed part of the structure
        /// plus the number of bytes required for the variable-length device path string. This parameter is
        /// optional and can be NULL.
        /// </param>
        /// <param name="deviceInfoData">
        /// A pointer to a buffer that receives information about the device that supports the requested
        /// interface. The caller must set <see cref="SpDevInfoData"/>.cbSize to sizeof(<see cref="SpDevInfoData"/>). This
        /// parameter is optional and can be NULL.
        /// </param>
        /// <returns>
        /// SetupDiGetDeviceInterfaceDetail returns TRUE if the function completed without error. If the
        /// function completed with an error, FALSE is returned and the error code for the failure
        /// can be retrieved by calling GetLastError.
        /// </returns>
        /// <remarks>
        /// https://docs.microsoft.com/en-us/windows/win32/api/setupapi/nf-setupapi-setupdigetdeviceinterfacedetaila
        /// </remarks>
        [DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Boolean SetupDiGetDeviceInterfaceDetailA(
            IntPtr hDevInfo,
            ref SpDeviceInterfaceData deviceInterfaceData,
            ref SpDeviceInterfaceDetailData deviceInterfaceDetailData,
            UInt32 deviceInterfaceDetailDataSize,
            ref UInt32 requiredSize,
            ref SpDevInfoData deviceInfoData
        );
    }
}
