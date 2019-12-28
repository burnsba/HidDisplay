using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace WinApi.Hid
{
    /// <summary>
    /// The HIDP_CAPS structure contains information about a top-level collection's capability.
    /// </summary>
    /// <remarks>
    /// https://www.pinvoke.net/default.aspx/hid/HIDP_CAPS.html
    /// https://docs.microsoft.com/en-us/windows-hardware/drivers/ddi/hidpi/ns-hidpi-_hidp_caps
    /// </remarks>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct HidpCaps
    {
        /// <summary>
        /// Specifies a top-level collection's usage ID.
        /// </summary>
        [MarshalAs(UnmanagedType.U2)]
        public UInt16 Usage;

        /// <summary>
        /// Specifies the top-level collection's usage page.
        /// </summary>
        [MarshalAs(UnmanagedType.U2)]
        public UInt16 UsagePage;

        /// <summary>
        /// Specifies the maximum size, in bytes, of all the input reports (including the report ID, if report IDs are used, which is prepended to the report data).
        /// </summary>
        [MarshalAs(UnmanagedType.U2)]
        public UInt16 InputReportByteLength;

        /// <summary>
        /// Specifies the maximum size, in bytes, of all the output reports (including the report ID, if report IDs are used, which is prepended to the report data).
        /// </summary>
        [MarshalAs(UnmanagedType.U2)]
        public UInt16 OutputReportByteLength;

        /// <summary>
        /// Specifies the maximum length, in bytes, of all the feature reports (including the report ID, if report IDs are used, which is prepended to the report data).
        /// </summary>
        [MarshalAs(UnmanagedType.U2)]
        public UInt16 FeatureReportByteLength;

        /// <summary>
        /// Reserved for internal system use.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
        internal UInt16[] Reserved;

        /// <summary>
        /// Specifies the number of HIDP_LINK_COLLECTION_NODE structures that are returned for this top-level collection by HidP_GetLinkCollectionNodes.
        /// </summary>
        [MarshalAs(UnmanagedType.U2)]
        public UInt16 NumberLinkCollectionNodes;

        /// <summary>
        /// Specifies the number of input HIDP_BUTTON_CAPS structures that HidP_GetButtonCaps returns.
        /// </summary>
        [MarshalAs(UnmanagedType.U2)]
        public UInt16 NumberInputButtonCaps;

        /// <summary>
        /// Specifies the number of input HIDP_VALUE_CAPS structures that HidP_GetValueCaps returns.
        /// </summary>
        [MarshalAs(UnmanagedType.U2)]
        public UInt16 NumberInputValueCaps;

        /// <summary>
        /// Specifies the number of data indices assigned to buttons and values in all input reports.
        /// </summary>
        [MarshalAs(UnmanagedType.U2)]
        public UInt16 NumberInputDataIndices;

        /// <summary>
        /// Specifies the number of output HIDP_BUTTON_CAPS structures that HidP_GetButtonCaps returns.
        /// </summary>
        [MarshalAs(UnmanagedType.U2)]
        public UInt16 NumberOutputButtonCaps;

        /// <summary>
        /// Specifies the number of output HIDP_VALUE_CAPS structures that HidP_GetValueCaps returns.
        /// </summary>
        [MarshalAs(UnmanagedType.U2)]
        public UInt16 NumberOutputValueCaps;

        /// <summary>
        /// Specifies the number of data indices assigned to buttons and values in all output reports.
        /// </summary>
        [MarshalAs(UnmanagedType.U2)]
        public UInt16 NumberOutputDataIndices;

        /// <summary>
        /// Specifies the total number of feature HIDP_BUTTONS_CAPS structures that HidP_GetButtonCaps returns.
        /// </summary>
        [MarshalAs(UnmanagedType.U2)]
        public UInt16 NumberFeatureButtonCaps;

        /// <summary>
        /// Specifies the total number of feature HIDP_VALUE_CAPS structures that HidP_GetValueCaps returns.
        /// </summary>
        [MarshalAs(UnmanagedType.U2)]
        public UInt16 NumberFeatureValueCaps;

        /// <summary>
        /// Specifies the number of data indices assigned to buttons and values in all feature reports.
        /// </summary>
        [MarshalAs(UnmanagedType.U2)]
        public UInt16 NumberFeatureDataIndices;

        public override string ToString()
        {
            return Utility.UsagePageAndUsageToString(UsagePage, Usage);
        }
    };
}
