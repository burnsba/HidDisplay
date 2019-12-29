using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace WinApi.Hid
{
    /// <summary>
    /// The <see cref="HidpButtonCaps"/> structure contains information about the capability of a
    /// HID control button usage (or a set of buttons associated with a usage range).
    /// </summary>
    /// <remarks>
    /// https://docs.microsoft.com/en-us/windows-hardware/drivers/ddi/hidpi/ns-hidpi-_hidp_button_caps
    /// https://github.com/tpn/winsdk-10/blob/master/Include/10.0.14393.0/shared/hidpi.h
    /// </remarks>
    [StructLayout(LayoutKind.Explicit)]
    public struct HidpButtonCaps
    {
        /// <summary>
        /// Specifies the usage page for a usage or usage range.
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
        /// Indicates, if TRUE, that a button has a set of aliased usages. Otherwise,
        /// if IsAlias is FALSE, the button has only one usage.
        /// </summary>
        [FieldOffset(3)]
        [MarshalAs(UnmanagedType.U1)]
        public bool IsAlias;

        /// <summary>
        /// Contains the data fields (one or two bytes) associated with an input, output, or feature main item.
        /// </summary>
        [FieldOffset(4)]
        [MarshalAs(UnmanagedType.U2)]
        public ushort BitField;

        /// <summary>
        /// Specifies the index of the link collection in a top-level collection's link
        /// collection array that contains the usage or usage range.
        /// If LinkCollection is zero, the usage or usage range is contained in
        /// the top-level collection.
        /// </summary>
        [FieldOffset(6)]
        [MarshalAs(UnmanagedType.U2)]
        public ushort LinkCollection;

        /// <summary>
        /// Specifies the usage of the link collection that contains the usage or
        /// usage range. If LinkCollection is zero, LinkUsage specifies the usage of
        /// the top-level collection.
        /// </summary>
        [FieldOffset(8)]
        [MarshalAs(UnmanagedType.U2)]
        public ushort LinkUsage;

        /// <summary>
        /// Specifies the usage page of the link collection that contains the usage or usage
        /// range. If LinkCollection is zero, LinkUsagePage specifies the usage page of the
        /// top-level collection.
        /// </summary>
        [FieldOffset(10)]
        [MarshalAs(UnmanagedType.U2)]
        public ushort LinkUsagePage;

        /// <summary>
        /// Specifies, if TRUE, that the structure describes a usage range. Otherwise, if
        /// IsRange is FALSE, the structure describes a single usage.
        /// </summary>
        [FieldOffset(12)]
        [MarshalAs(UnmanagedType.U1)]
        public bool IsRange;

        /// <summary>
        /// Specifies, if TRUE, that the usage or usage range has a set of string
        /// descriptors. Otherwise, if IsStringRange is FALSE, the usage or usage
        /// range has zero or one string descriptor.
        /// </summary>
        [FieldOffset(13)]
        [MarshalAs(UnmanagedType.U1)]
        public bool IsStringRange;

        /// <summary>
        /// Specifies, if TRUE, that the usage or usage range has a set of designators.
        /// Otherwise, if IsDesignatorRange is FALSE, the usage or usage range has
        /// zero or one designator.
        /// </summary>
        [FieldOffset(14)]
        [MarshalAs(UnmanagedType.U1)]
        public bool IsDesignatorRange;

        /// <summary>
        /// Specifies, if TRUE, that the button usage or usage range provides absolute
        /// data. Otherwise, if IsAbsolute is FALSE, the button data is the change in
        /// state from the previous value.
        /// </summary>
        [FieldOffset(15)]
        [MarshalAs(UnmanagedType.U1)]
        public bool IsAbsolute;

        /// <summary>
        /// Reserved for internal system use.
        /// </summary>
        [FieldOffset(16)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        internal uint[] Reserved;

        [FieldOffset(56)]
        public HidpButtonCapsRange Range;

        [FieldOffset(56)]
        public HidpButtonCapsNotRange NotRange;

        public override string ToString()
        {
            if (IsRange)
                return Utility.UsagePageToString(UsagePage);

            return Utility.UsagePageAndUsageToString(UsagePage, NotRange.Usage);
        }

        public static HidpButtonCaps FromBytes(byte[] bytes, int offset, out int nextByteOffset)
        {
            int nextOffset;

            var hbc = new HidpButtonCaps()
            {
                UsagePage = (ushort)(((ushort)bytes[offset+1] << 8) | (ushort)(bytes[offset])),
                ReportID = bytes[offset + 2],
                IsAlias = bytes[offset + 3] > 0 ? true : false,
                BitField = (ushort)(((ushort)bytes[offset + 5] << 8) | (ushort)(bytes[offset + 4])),
                LinkCollection = (ushort)(((ushort)bytes[offset + 7] << 8) | (ushort)(bytes[offset + 6])),
                LinkUsage = (ushort)(((ushort)bytes[offset + 9] << 8) | (ushort)(bytes[offset + 8])),
                IsRange = bytes[offset + 10] > 0 ? true : false,
                IsStringRange = bytes[offset + 11] > 0 ? true : false,
                IsDesignatorRange = bytes[offset + 12] > 0 ? true : false,
                IsAbsolute = bytes[offset + 13] > 0 ? true : false,
                // skip reserved
                Range = HidpButtonCapsRange.FromBytes(bytes, offset + 56, out nextOffset)
            };

            nextByteOffset = nextOffset;

            return hbc;
        }
    }
}