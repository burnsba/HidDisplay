using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace WinApi.Hid
{
    /// <summary>
    /// Function definitions for hid.dll.
    /// </summary>
    public static class Api
    {
        /// <summary>
        /// HidP_GetButtonCaps returns all the buttons (binary values) that are a part
        /// of the given report type for the Hid device represented by the given
        /// preparsed data.
        /// </summary>
        /// <param name="reportType">One of HidP_Input, HidP_Output, or HidP_Feature.</param>
        /// <param name="buttonCaps">
        /// A _HIDP_BUTTON_CAPS array containing information about all the 
        /// binary values in the given report.This buffer is provided by
        /// the caller.
        /// </param>
        /// <param name="buttonCapsLength">
        /// As input, this parameter specifies the length of the
        /// ButtonCaps parameter(array) in number of array elements.
        /// As output, this value is set to indicate how many of those
        /// array elements were filled in by the function.The maximum number of
        /// button caps that can be returned is found in the HIDP_CAPS
        /// structure.If HIDP_STATUS_BUFFER_TOO_SMALL is returned,
        /// this value contains the number of array elements needed to
        /// successfully complete the request.
        /// </param>
        /// <param name="preparsedData">The preparsed data returned from HIDCLASS.</param>
        /// <returns>HIDP_STATUS_SUCCESS, HIDP_STATUS_INVALID_PREPARSED_DATA.</returns>
        /// <remarks>
        /// https://docs.microsoft.com/en-us/windows-hardware/drivers/ddi/hidpi/nf-hidpi-hidp_getbuttoncaps
        /// </remarks>
        [DllImport("hid.dll", SetLastError = true)]
        public static extern long HidP_GetButtonCaps(
            [In] HidpReportType reportType,
            [Out] IntPtr buttonCaps, // CLR crashes without exception (when auto-marshalling struct with union?)
            [In, Out] ref uint buttonCapsLength,
            [In] IntPtr preparsedData
        );

        /// <summary>
        /// The HidP_GetCaps routine returns a top-level collection's HIDP_CAPS structure.
        /// </summary>
        /// <param name="PreparsedData">
        /// Pointer to a top-level collection's preparsed data.
        /// </param>
        /// <param name="Capabilities">
        /// Pointer to a caller-allocated buffer that the routine uses to return a collection's HIDP_CAPS structure.
        /// </param>
        /// <returns>
        /// HIDP_STATUS_SUCCESS, HIDP_STATUS_INVALID_PREPARSED_DATA.
        /// </returns>
        /// <remarks>
        /// https://docs.microsoft.com/en-us/windows-hardware/drivers/ddi/hidpi/nf-hidpi-hidp_getcaps
        /// </remarks>
        [DllImport("hid.dll", SetLastError = true)]
        public static extern long HidP_GetCaps(
            IntPtr PreparsedData,
            out HidpCaps Capabilities);

        /// <summary>
        /// The HidD_GetHidGuid routine returns the device interfaceGUID for HIDClass devices.
        /// </summary>
        /// <param name="hidGuid">Pointer to a caller-allocated GUID buffer that the routine
        /// uses to return the device interface GUID for HIDClass devices.</param>
        /// <remarks>
        /// https://docs.microsoft.com/en-us/windows-hardware/drivers/ddi/hidsdi/nf-hidsdi-hidd_gethidguid
        /// </remarks>
        [DllImport("hid.dll", SetLastError = true)]
        public static extern void HidD_GetHidGuid(
            [Out] out Guid hidGuid
            );

        /// <summary>
        /// The HidD_GetInputReport routine returns an input reports from a top-level collection.
        /// </summary>
        /// <param name="hidDeviceObject">Specifies an open handle to a top-level collection.</param>
        /// <param name="report">
        /// Pointer to a caller-allocated input report buffer that the caller uses to specify a
        /// HID report ID and HidD_GetInputReport uses to return the specified input report.
        /// </param>
        /// <param name="reportBufferLength">
        /// Specifies the size, in bytes, of the report buffer. The report buffer must be large enough
        /// to hold the input report -- excluding its report ID, if report IDs are used -- plus
        /// one additional byte that specifies a nonzero report ID or zero.
        /// </param>
        /// <returns>
        /// HidD_GetInputReport returns TRUE if it succeeds; otherwise, it returns FALSE.
        /// Use GetLastError to get extended error information.
        /// </returns>
        /// <remarks>
        /// https://docs.microsoft.com/en-us/windows-hardware/drivers/ddi/hidsdi/nf-hidsdi-hidd_getinputreport
        /// </remarks>
        [DllImport("hid.dll", SetLastError = true)]
        public static extern bool HidD_GetInputReport(
            [In] SafeHandle hidDeviceObject,
            [In] byte[] report,
            [In] uint reportBufferLength
        );

        /// <summary>
        /// This function returns the maximum number of usages that a call to
        /// HidP_GetUsages or HidP_GetUsagesEx could return for a given HID report.
        /// If calling for number of usages returned by HidP_GetUsagesEx, use 0 as
        /// the UsagePage value.
        /// </summary>
        /// <param name="reportType">One of HidP_Input, HidP_Output or HidP_Feature.</param>
        /// <param name="usagePage">
        /// Specifies the optional UsagePage to query for.  If 0, will
        /// return all the maximum number of usage values that could be
        /// returned for a given ReportType. If non-zero, will return
        /// the maximum number of usages that would be returned for the
        /// ReportType with the given UsagePage.</param>
        /// <param name="preparsedData">Preparsed data returned from HIDCLASS</param>
        /// <returns>
        /// The length of the usage list array required for the HidP_GetUsages or
        /// HidP_GetUsagesEx function call.If an error occurs(such as
        /// HIDP_STATUS_INVALID_REPORT_TYPE or HIDP_INVALID_PREPARSED_DATA, this
        /// returns 0.
        /// </returns>
        /// <remarks>
        /// https://docs.microsoft.com/en-us/windows-hardware/drivers/ddi/hidpi/nf-hidpi-hidp_maxusagelistlength
        /// </remarks>
        [DllImport("hid.dll", SetLastError = true)]
        public static extern ulong HidP_MaxUsageListLength(
            [In] HidpReportType reportType,
            [In] ushort usagePage,
            [In] IntPtr preparsedData
        );

        /// <summary>
        /// The HidD_GetManufacturerString routine returns a top-level collection's embedded
        /// string that identifies the manufacturer.
        /// </summary>
        /// <param name="hidDeviceObject">
        /// Specifies an open handle to a top-level collection.
        /// </param>
        /// <param name="buffer">
        /// Pointer to a caller-allocated buffer that the routine uses to return the
        /// collection's manufacturer string. The routine returns a NULL-terminated wide
        /// character string in a human-readable format.
        /// </param>
        /// <param name="bufferLength">
        /// Specifies the length, in bytes, of a caller-allocated buffer provided at Buffer. If
        /// the buffer is not large enough to return the entire NULL-terminated embedded string,
        /// the routine returns nothing in the buffer.
        /// </param>
        /// <returns>
        /// HidD_HidD_GetManufacturerString returns TRUE if it returns the entire NULL-terminated
        /// embedded string. Otherwise, the routine returns FALSE. Use GetLastError to get extended
        /// error information.
        /// </returns>
        /// <remarks>
        /// https://docs.microsoft.com/en-us/windows-hardware/drivers/ddi/hidsdi/nf-hidsdi-hidd_getmanufacturerstring
        /// </remarks>
        [DllImport("hid.dll", SetLastError = true)]
        public static extern bool HidD_GetManufacturerString(
            [In] SafeHandle hidDeviceObject,
            IntPtr buffer,
            uint bufferLength
            );

        /// <summary>
        /// The HidD_GetPhysicalDescriptor routine returns the embedded string of a top-level
        /// collection that identifies the collection's physical device.
        /// </summary>
        /// <param name="hidDeviceObject">
        /// Specifies an open handle to a top-level collection.
        /// </param>
        /// <param name="buffer">
        /// Pointer to a caller-allocated buffer that the routine uses to return the requested
        /// physical descriptor.
        /// </param>
        /// <param name="bufferLength">
        /// Specifies the length, in bytes, of the buffer at Buffer.
        /// </param>
        /// <returns>
        /// HidD_GetPhysicalDescriptor returns TRUE if it succeeds; otherwise, it returns FALSE.
        /// Use GetLastError to get extended error information.
        /// </returns>
        /// <remarks>
        /// https://docs.microsoft.com/en-us/windows-hardware/drivers/ddi/hidsdi/nf-hidsdi-hidd_getphysicaldescriptor
        /// </remarks>
        [DllImport("hid.dll", SetLastError = true)]
        public static extern bool HidD_GetPhysicalDescriptor(
            [In] SafeHandle hidDeviceObject,
            IntPtr buffer,
            uint bufferLength
            );

        /// <summary>
        /// The HidD_GetProductString routine returns the embedded string of a top-level
        /// collection that identifies the manufacturer's product.
        /// </summary>
        /// <param name="hidDeviceObject">
        /// Specifies an open handle to a top-level collection.
        /// </param>
        /// <param name="buffer">
        /// Pointer to a caller-allocated buffer that the routine uses to return the
        /// requested product string. The routine returns a NULL-terminated wide character string.
        /// </param>
        /// <param name="bufferLength">
        /// Specifies the length, in bytes, of a caller-allocated buffer provided at Buffer.
        /// If the buffer is not large enough to return the entire NULL-terminated
        /// embedded string, the routine returns nothing in the buffer.</param>
        /// <returns>
        /// HidD_GetProductString returns TRUE if it successfully returns the entire NULL-terminated
        /// embedded string. Otherwise, the routine returns FALSE. Use GetLastError to get
        /// extended error information.
        /// </returns>
        [DllImport("hid.dll", SetLastError = true)]
        public static extern bool HidD_GetProductString(
            [In] SafeHandle hidDeviceObject,
            IntPtr buffer,
            uint bufferLength
            );

        /// <summary>
        /// The HidD_GetSerialNumberString routine returns the embedded string of a top-level
        /// collection that identifies the serial number of the collection's physical device.
        /// </summary>
        /// <param name="hidDeviceObject">
        /// Specifies an open handle to a top-level collection.
        /// </param>
        /// <param name="buffer">
        /// Pointer to a caller-allocated buffer that the routine uses to return the
        /// requested serial number string. The routine returns a NULL-terminated wide character string.
        /// </param>
        /// <param name="bufferLength">
        /// Specifies the length, in bytes, of a caller-allocated buffer provided at Buffer.
        /// If the buffer is not large enough to return the entire NULL-terminated embedded
        /// string, the routine returns nothing in the buffer.
        /// </param>
        /// <returns>
        /// HidD_GetSerialNumberString returns TRUE if it successfully returns the entire
        /// NULL-terminated embedded string. Otherwise, the routine returns FALSE. Use
        /// GetLastError to get extended error information.
        /// </returns>
        [DllImport("hid.dll", SetLastError = true)]
        public static extern bool HidD_GetSerialNumberString(
            [In] SafeHandle hidDeviceObject,
            IntPtr buffer,
            uint bufferLength
            );

        /// <summary>
        /// This function returns the binary values (buttons) that are set in a HID
        /// report.Given a report packet of correct length, it searches the report
        /// packet for each usage for the given usage page and returns them in the
        /// usage list.
        /// </summary>
        /// <param name="reportType">One of HidP_Input, HidP_Output or HidP_Feature.</param>
        /// <param name="usagePage">
        /// All of the usages in the usage list, which HidP_GetUsages will
        /// retrieve in the report, refer to this same usage page.
        /// If the client wishes to get usages in a packet for multiple
        /// usage pages then that client needs to make multiple calls
        /// to HidP_GetUsages.</param>
        /// <param name="linkCollection">
        /// An optional value which can limit which usages are returned
        /// in the UsageList to those usages that exist in a specific
        /// LinkCollection.A non-zero value indicates the index into
        /// the HIDP_LINK_COLLECITON_NODE list returned by
        /// HidP_GetLinkCollectionNodes of the link collection the
        /// usage should belong to.A value of 0 indicates this
        /// should value be ignored.
        /// </param>
        /// <param name="usageList">
        /// The usage array that will contain all the usages found in
        /// the report packet.
        /// </param>
        /// <param name="usageLength">
        /// The length of the given usage array in array elements.
        /// On input, this value describes the length of the usage list.
        /// On output, HidP_GetUsages sets this value to the number of
        /// usages that was found. Use HidP_MaxUsageListLength to
        /// determine the maximum length needed to return all the usages
        /// that a given report packet may contain.
        /// </param>
        /// <param name="preparsedData">Preparsed data structure returned by HIDCLASS</param>
        /// <param name="report">The report packet.</param>
        /// <param name="reportLength">Length (in bytes) of the given report packet</param>
        /// <returns>
        /// HIDP_STATUS_SUCCESS,
        /// HIDP_INVALID_REPORT_LENGTH,
        /// HIDP_INVALID_REPORT_TYPE,
        /// HIDP_STATUS_BUFFER_TOO_SMALL,
        /// HIDP_STATUS_INCOMPATIBLE_REPORT_ID,
        /// HIDP_STATUS_INVALID_PREPARSED_DATA,
        /// HIDP_STATUS_USAGE_NOT_FOUND.
        /// </returns>
        /// <remarks>
        /// https://docs.microsoft.com/en-us/windows-hardware/drivers/ddi/hidpi/nf-hidpi-hidp_getusages
        /// </remarks>
        [DllImport("hid.dll", SetLastError = true)]
        public static extern long HidP_GetUsages(
            [In] HidpReportType reportType,
            [In] ushort usagePage,
            [In] ushort linkCollection,
            [In] ushort[] usageList,
            [In, Out] ref uint usageLength,
            [In] IntPtr preparsedData,
            [In] byte[] report,
            [In] uint reportLength
        );

        /// <summary>
        /// HidP_GetUsageValue retrieves the value from the HID Report for the usage
        /// specified by the combination of usage page, usage and link collection.
        /// If a report packet contains two different fields with the same
        /// Usage and UsagePage, they can be distinguished with the optional
        /// LinkCollection field value.
        /// </summary>
        /// <param name="reportType">One of HidP_Input or HidP_Feature.</param>
        /// <param name="usagePage">The usage page to which the given usage refers.</param>
        /// <param name="linkCollection">(Optional)  This value can be used to differentiate
        /// between two fields that may have the same
        /// UsagePage and Usage but exist in different
        /// collections.If the link collection value
        /// is zero, this function will set the first field
        /// it finds that matches the usage page and
        /// usage.</param>
        /// <param name="usage">The usage whose value HidP_GetUsageValue will retrieve</param>
        /// <param name="usageValue">The raw value that is set for the specified field in the report
        /// buffer.This value will either fall within the logical range
        /// or if NULL values are allowed, a number outside the range to
        /// indicate a NULL</param>
        /// <param name="preparsedData">The preparsed data returned for HIDCLASS</param>
        /// <param name="report">The report packet.</param>
        /// <param name="reportLength">Length (in bytes) of the given report packet.</param>
        /// <returns>
        /// HIDP_STATUS_SUCCESS, HIDP_INVALID_REPORT_LENGTH,
        /// HIDP_INVALID_REPORT_TYPE, HIDP_STATUS_INCOMPATIBLE_REPORT_ID, 
        /// HIDP_STATUS_INVALID_PREPARSED_DATA, HIDP_STATUS_USAGE_NOT_FOUND.
        /// </returns>
        /// <remarks>
        /// https://docs.microsoft.com/en-us/windows-hardware/drivers/ddi/hidpi/nf-hidpi-hidp_getusagevalue
        /// </remarks>
        [DllImport("hid.dll", SetLastError = true)]
        public static extern ulong HidP_GetUsageValue(
            [In] HidpReportType reportType,
            [In] ushort usagePage,
            [In] ushort linkCollection,
            [In] ushort usage,
            [Out] out uint usageValue,
            [In] IntPtr preparsedData,
            [In] byte[] report,
            [In] uint reportLength
        );

        /// <summary>
        /// HidP_GetValueCaps returns all the values (non-binary) that are a part
        /// of the given report type for the Hid device represented by the given
        /// preparsed data.
        /// </summary>
        /// <param name="reportType">
        /// Specifies a HIDP_REPORT_TYPE enumerator value that identifies the report type.
        /// </param>
        /// <param name="valueCaps">
        /// Pointer to a caller-allocated buffer in which the routine returns a value
        /// capability array for the specified report type.</param>
        /// <param name="valueCapsLength">
        /// Specifies the length, on input, in array elements, of the ValueCaps buffer. On output,
        /// the routine sets ValueCapsLength to the number of elements that the it actually returns.
        /// </param>
        /// <param name="preparsedData">Pointer to a top-level collection's preparsed data.</param>
        /// <returns>HIDP_STATUS_SUCCESS, HIDP_STATUS_INVALID_PREPARSED_DATA</returns>
        /// <remarks>
        /// https://docs.microsoft.com/en-us/windows-hardware/drivers/ddi/hidpi/nf-hidpi-hidp_getvaluecaps
        /// </remarks>
        [DllImport("hid.dll", SetLastError = true)]
        public static extern long HidP_GetValueCaps(
            [In] HidpReportType reportType,
            [Out] IntPtr valueCaps,
            [In, Out] ref uint valueCapsLength,
            [In] IntPtr preparsedData
        );
    }
}
