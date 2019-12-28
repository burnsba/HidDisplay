using System;
using System.Collections.Generic;
using System.Text;

namespace WinApi.Hid
{
    /*
        https://github.com/tpn/winsdk-10/blob/master/Include/10.0.14393.0/shared/hidpi.h

        // FACILITY_HID_ERROR_CODE defined in ntstatus.h
        #ifndef FACILITY_HID_ERROR_CODE
        #define FACILITY_HID_ERROR_CODE 0x11
        #endif

        #define HIDP_ERROR_CODES(SEV, CODE) \
                ((NTSTATUS) (((SEV) << 28) | (FACILITY_HID_ERROR_CODE << 16) | (CODE)))

        #define HIDP_STATUS_SUCCESS                  (HIDP_ERROR_CODES(0x0,0))
        #define HIDP_STATUS_NULL                     (HIDP_ERROR_CODES(0x8,1))
        #define HIDP_STATUS_INVALID_PREPARSED_DATA   (HIDP_ERROR_CODES(0xC,1))
        #define HIDP_STATUS_INVALID_REPORT_TYPE      (HIDP_ERROR_CODES(0xC,2))
        #define HIDP_STATUS_INVALID_REPORT_LENGTH    (HIDP_ERROR_CODES(0xC,3))
        #define HIDP_STATUS_USAGE_NOT_FOUND          (HIDP_ERROR_CODES(0xC,4))
        #define HIDP_STATUS_VALUE_OUT_OF_RANGE       (HIDP_ERROR_CODES(0xC,5))
        #define HIDP_STATUS_BAD_LOG_PHY_VALUES       (HIDP_ERROR_CODES(0xC,6))
        #define HIDP_STATUS_BUFFER_TOO_SMALL         (HIDP_ERROR_CODES(0xC,7))
        #define HIDP_STATUS_INTERNAL_ERROR           (HIDP_ERROR_CODES(0xC,8))
        #define HIDP_STATUS_I8042_TRANS_UNKNOWN      (HIDP_ERROR_CODES(0xC,9))
        #define HIDP_STATUS_INCOMPATIBLE_REPORT_ID   (HIDP_ERROR_CODES(0xC,0xA))
        #define HIDP_STATUS_NOT_VALUE_ARRAY          (HIDP_ERROR_CODES(0xC,0xB))
        #define HIDP_STATUS_IS_VALUE_ARRAY           (HIDP_ERROR_CODES(0xC,0xC))
        #define HIDP_STATUS_DATA_INDEX_NOT_FOUND     (HIDP_ERROR_CODES(0xC,0xD))
        #define HIDP_STATUS_DATA_INDEX_OUT_OF_RANGE  (HIDP_ERROR_CODES(0xC,0xE))
        #define HIDP_STATUS_BUTTON_NOT_PRESSED       (HIDP_ERROR_CODES(0xC,0xF))
        #define HIDP_STATUS_REPORT_DOES_NOT_EXIST    (HIDP_ERROR_CODES(0xC,0x10))
        #define HIDP_STATUS_NOT_IMPLEMENTED          (HIDP_ERROR_CODES(0xC,0x20))

        // Helper to get enum value:
        public static string HIDP_ERROR_CODES(UInt64 SEV, UInt64 CODE) =
        {
            return "0x" + ((UInt64)(((SEV) << 28) | ((UInt64)0x11 << 16) | (CODE))).ToString("X"); }
        }
     * */
    public enum HidpStatus : long
    {
        /// <summary>
        /// upon successfully retrieving all the usages from the report packet
        /// </summary>
        HIDP_STATUS_SUCCESS = 0x110000,
        HIDP_STATUS_NULL = 0x80110001,

        /// <summary>
        /// if PreparsedData is not valid
        /// </summary>
        HIDP_STATUS_INVALID_PREPARSED_DATA = 0xC0110001,

        /// <summary>
        /// if ReportType is not valid.
        /// </summary>
        HIDP_STATUS_INVALID_REPORT_TYPE = 0xC0110002,

        /// <summary>
        /// the length of the report packet is not equal to the length specified in
        /// the HIDP_CAPS structure for the given
        /// ReportType
        /// </summary>
        HIDP_STATUS_INVALID_REPORT_LENGTH = 0xC0110003,

        /// <summary>
        /// if there are no usages in a reports for
        /// the device and ReportType that match the
        /// UsagePage and LinkCollection that were
        /// specified
        /// </summary>
        HIDP_STATUS_USAGE_NOT_FOUND = 0xC0110004,
        HIDP_STATUS_VALUE_OUT_OF_RANGE = 0xC0110005,
        HIDP_STATUS_BAD_LOG_PHY_VALUES = 0xC0110006,

        /// <summary>
        /// if the UsageList is not big enough to
        /// hold all the usages found in the report
        /// packet.  If this is returned, the buffer
        /// will contain UsageLength number of
        /// usages.  Use HidP_MaxUsageListLength to
        /// find the maximum length needed
        /// </summary>
        HIDP_STATUS_BUFFER_TOO_SMALL = 0xC0110007,
        HIDP_STATUS_INTERNAL_ERROR = 0xC0110008,
        HIDP_STATUS_I8042_TRANS_UNKNOWN = 0xC0110009,

        /// <summary>
        /// if no usages were found but usages
        /// that match the UsagePage and
        /// LinkCollection specified could be found
        /// in a report with a different report ID
        /// </summary>
        HIDP_STATUS_INCOMPATIBLE_REPORT_ID = 0xC011000A,
        HIDP_STATUS_NOT_VALUE_ARRAY = 0xC011000B,
        HIDP_STATUS_IS_VALUE_ARRAY = 0xC011000C,
        HIDP_STATUS_DATA_INDEX_NOT_FOUND = 0xC011000D,
        HIDP_STATUS_DATA_INDEX_OUT_OF_RANGE = 0xC011000E,
        HIDP_STATUS_BUTTON_NOT_PRESSED = 0xC011000F,

        /// <summary>
        /// if there are no reports on this device
        /// for the given ReportType
        /// </summary>
        HIDP_STATUS_REPORT_DOES_NOT_EXIST = 0xC0110010,
        HIDP_STATUS_NOT_IMPLEMENTED = 0xC0110020,
    }
}
