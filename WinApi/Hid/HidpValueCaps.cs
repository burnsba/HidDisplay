using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace WinApi.Hid
{
    /// <summary>
    /// The HIDP_VALUE_CAPS structure contains information that describes the capability of a set of HID control values (either a single usage or a usage range).
    /// </summary>
    /// <remarks>
    /// https://docs.microsoft.com/en-us/windows-hardware/drivers/ddi/hidpi/ns-hidpi-_hidp_value_caps
    /// https://github.com/tpn/winsdk-10/blob/master/Include/10.0.14393.0/shared/hidpi.h
    /// </remarks>
    [StructLayout(LayoutKind.Explicit)]
    public struct HidpValueCaps
    {
        /// <summary>
        /// Specifies the usage page of the usage or usage range.
        /// </summary>
        [FieldOffset(0)]
        [MarshalAs(UnmanagedType.U2)]
        public ushort UsagePage;

        /// <summary>
        /// Specifies the report ID of the HID report that contains the usage or usage range.
        /// </summary>
        [FieldOffset(2)]
        [MarshalAs(UnmanagedType.U1)]
        public byte ReportID;

        /// <summary>
        /// Indicates, if TRUE, that the usage is member of a set of aliased usages.
        /// Otherwise, if IsAlias is FALSE, the value has only one usage.
        /// </summary>
        [FieldOffset(3)]
        [MarshalAs(UnmanagedType.U1)]
        public bool IsAlias;

        /// <summary>
        /// Contains the data fields (one or two bytes) associated with an input,
        /// output, or feature main item.
        /// </summary>
        [FieldOffset(4)]
        [MarshalAs(UnmanagedType.U2)]
        public ushort BitField;

        /// <summary>
        /// Specifies the index of the link collection in a top-level collection's link
        /// collection array that contains the usage or usage range. If
        /// LinkCollection is zero, the usage or usage range is contained in the top-level collection.
        /// </summary>
        [FieldOffset(6)]
        [MarshalAs(UnmanagedType.U2)]
        public ushort LinkCollection;

        /// <summary>
        /// Specifies the usage of the link collection that contains the usage
        /// or usage range. If LinkCollection is zero, LinkUsage specifies the
        /// usage of the top-level collection.
        /// </summary>
        [FieldOffset(8)]
        [MarshalAs(UnmanagedType.U2)]
        public ushort LinkUsage;

        /// <summary>
        /// Specifies the usage page of the link collection that contains the
        /// usage or usage range. If LinkCollection is zero, LinkUsagePage
        /// specifies the usage page of the top-level collection.
        /// </summary>
        [FieldOffset(10)]
        [MarshalAs(UnmanagedType.U2)]
        public ushort LinkUsagePage;

        /// <summary>
        /// Specifies, if TRUE, that the structure describes a usage range.
        /// Otherwise, if IsRange is FALSE, the structure describes a single usage.
        /// </summary>
        [FieldOffset(12)]
        [MarshalAs(UnmanagedType.U1)]
        public bool IsRange;

        /// <summary>
        /// Specifies, if TRUE, that the usage or usage range has a set of
        /// string descriptors. Otherwise, if IsStringRange is FALSE, the usage
        /// or usage range has zero or one string descriptor.
        /// </summary>
        [FieldOffset(13)]
        [MarshalAs(UnmanagedType.U1)]
        public bool IsStringRange;

        /// <summary>
        /// Specifies, if TRUE, that the usage or usage range has a set of
        /// designators. Otherwise, if IsDesignatorRange is FALSE, the usage
        /// or usage range has zero or one designator.
        /// </summary>
        [FieldOffset(14)]
        [MarshalAs(UnmanagedType.U1)]
        public bool IsDesignatorRange;

        /// <summary>
        /// Specifies, if TRUE, that the usage or usage range provides
        /// absolute data. Otherwise, if IsAbsolute is FALSE, the value
        /// is the change in state from the previous value.
        /// </summary>
        [FieldOffset(15)]
        [MarshalAs(UnmanagedType.U1)]
        public bool IsAbsolute;

        /// <summary>
        /// Specifies, if TRUE, that the usage supports a NULL value, which
        /// indicates that the data is not valid and should be ignored.
        /// Otherwise, if HasNull is FALSE, the usage does not have a NULL value.
        /// </summary>
        [FieldOffset(16)]
        [MarshalAs(UnmanagedType.U1)]
        public bool HasNull;

        /// <summary>
        /// Reserved for internal system use.
        /// </summary>
        [FieldOffset(17)]
        [MarshalAs(UnmanagedType.U1)]
        internal byte Reserved;

        /// <summary>
        /// Specifies the size, in bits, of a usage's data field in a report. If
        /// ReportCount is greater than one, each usage has a separate data field of this size.
        /// </summary>
        [FieldOffset(18)]
        [MarshalAs(UnmanagedType.U2)]
        public ushort BitSize;

        /// <summary>
        /// Specifies the number of usages that this structure describes.
        /// </summary>
        [FieldOffset(20)]
        [MarshalAs(UnmanagedType.U2)]
        public ushort ReportCount;

        /// <summary>
        /// Reserved for internal system use.
        /// </summary>
        [FieldOffset(22)]
        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        internal ushort Reserved2a;
        [FieldOffset(24)]
        internal ushort Reserved2b;
        [FieldOffset(26)]
        internal ushort Reserved2c;
        [FieldOffset(28)]
        internal ushort Reserved2d;
        [FieldOffset(30)]
        internal ushort Reserved2e;

        /// <summary>
        /// Specifies the usage's exponent, as described by the USB HID standard.
        /// </summary>
        [FieldOffset(32)]
        [MarshalAs(UnmanagedType.U4)]
        public uint UnitsExp;

        /// <summary>
        /// Specifies the usage's units, as described by the USB HID Standard.
        /// </summary>
        [FieldOffset(36)]
        [MarshalAs(UnmanagedType.U4)]
        public uint Units;

        /// <summary>
        /// Specifies a usage's signed lower bound.
        /// </summary>
        [FieldOffset(40)]
        [MarshalAs(UnmanagedType.U4)]
        public int LogicalMin;

        /// <summary>
        /// Specifies a usage's signed upper bound.
        /// </summary>
        [FieldOffset(44)]
        [MarshalAs(UnmanagedType.U4)]
        public int LogicalMax;

        /// <summary>
        /// Specifies a usage's signed lower bound after scaling is applied to the logical minimum value.
        /// </summary>
        [FieldOffset(48)]
        [MarshalAs(UnmanagedType.U4)]
        public int PhysicalMin;

        /// <summary>
        /// Specifies a usage's signed upper bound after scaling is applied to the logical maximum value.
        /// </summary>
        [FieldOffset(52)]
        [MarshalAs(UnmanagedType.U4)]
        public int PhysicalMax;

        [FieldOffset(56)]
        public HidpValueValueCapsRange Range;

        [FieldOffset(56)]
        public HidpValueValueCapsNotRange NotRange;

        public override string ToString()
        {
            if (IsRange)
                return Utility.UsagePageToString(UsagePage);

            return Utility.UsagePageAndUsageToString(UsagePage, NotRange.Usage);
        }

        public static HidpValueCaps FromBytes(byte[] bytes, int offset, out int nextByteOffset)
        {
            int nextOffset;

            var hbc = new HidpValueCaps()
            {
                UsagePage = (ushort)(((ushort)bytes[offset + 1] << 8) | (ushort)(bytes[offset + 0])),
                ReportID = bytes[offset + 2],
                IsAlias = bytes[offset + 3] > 0 ? true : false,
                BitField = (ushort)(((ushort)bytes[offset + 5] << 8) | (ushort)(bytes[offset + 4])),
                LinkCollection = (ushort)(((ushort)bytes[offset + 7] << 8) | (ushort)(bytes[offset + 6])),
                LinkUsage = (ushort)(((ushort)bytes[offset + 9] << 8) | (ushort)(bytes[offset + 8])),
                LinkUsagePage = (ushort)(((ushort)bytes[offset + 11] << 8) | (ushort)(bytes[offset + 10])),
                IsRange = bytes[offset + 12] > 0 ? true : false,
                IsStringRange = bytes[offset + 13] > 0 ? true : false,
                IsDesignatorRange = bytes[offset + 14] > 0 ? true : false,
                IsAbsolute = bytes[offset + 15] > 0 ? true : false,
                HasNull = bytes[offset + 16] > 0 ? true : false,
                BitSize = (ushort)(((ushort)bytes[offset + 19] << 8) | (ushort)(bytes[offset + 18])),
                ReportCount = (ushort)(((ushort)bytes[offset + 21] << 8) | (ushort)(bytes[offset + 20])),
                // skip reserved
                UnitsExp = (uint)(((uint)bytes[offset + 35] << 24) | ((uint)bytes[offset + 34] << 16) | ((uint)bytes[offset + 33] << 8) | (uint)(bytes[offset + 32])),
                Units = (uint)(((uint)bytes[offset + 39] << 24) | ((uint)bytes[offset + 38] << 16) | ((uint)bytes[offset + 37] << 8) | (uint)(bytes[offset + 36])),
                LogicalMin = (int)(((int)bytes[offset + 43] << 24) | ((int)bytes[offset + 42] << 16) | ((int)bytes[offset + 41] << 8) | (int)(bytes[offset + 40])),
                LogicalMax = (int)(((int)bytes[offset + 47] << 24) | ((int)bytes[offset + 46] << 16) | ((int)bytes[offset + 45] << 8) | (int)(bytes[offset + 44])),
                PhysicalMin = (int)(((int)bytes[offset + 51] << 24) | ((int)bytes[offset + 50] << 16) | ((int)bytes[offset + 49] << 8) | (int)(bytes[offset + 48])),
                PhysicalMax = (int)(((int)bytes[offset + 55] << 24) | ((int)bytes[offset + 54] << 16) | ((int)bytes[offset + 53] << 8) | (int)(bytes[offset + 52])),

                Range = HidpValueValueCapsRange.FromBytes(bytes, offset + 56, out nextOffset)
            };

            nextByteOffset = nextOffset;

            return hbc;
        }
    }
}
