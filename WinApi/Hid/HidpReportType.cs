using System;
using System.Collections.Generic;
using System.Text;

namespace WinApi.Hid
{
    /// <summary>
    /// The HIDP_REPORT_TYPE enumeration type is used to specify a HID report type.
    /// </summary>
    /// <remarks>
    /// https://docs.microsoft.com/en-us/windows-hardware/drivers/ddi/hidpi/ne-hidpi-_hidp_report_type
    /// </remarks>
    public enum HidpReportType : int
    {
        /// <summary>
        /// Indicates an input report.
        /// </summary>
        HidP_Input,

        /// <summary>
        /// Indicates an output report.
        /// </summary>
        HidP_Output,

        /// <summary>
        /// Indicates a feature report.
        /// </summary>
        HidP_Feature
    }
}
