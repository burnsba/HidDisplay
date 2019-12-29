using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace WinApi.Hid
{
    /// <summary>
    /// Union struct for <see cref="HidpButtonCaps"/>.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct HidpButtonCapsRange
    {
        /// <summary>
        /// Indicates the inclusive lower bound of usage range whose inclusive upper
        /// bound is specified by Range.UsageMax.
        /// </summary>
        [FieldOffset(0)]
        public ushort UsageMin;

        /// <summary>
        /// Indicates the inclusive upper bound of a usage range whose inclusive lower
        /// bound is indicated by Range.UsageMin.
        /// </summary>
        [FieldOffset(2)]
        public ushort UsageMax;

        /// <summary>
        /// Indicates the inclusive lower bound of a range of string descriptors (specified by string
        /// minimum and string maximum items) whose inclusive upper bound is indicated by Range.StringMax.
        /// </summary>
        [FieldOffset(4)]
        public ushort StringMin;

        /// <summary>
        /// Indicates the inclusive upper bound of a range of string descriptors (specified by string
        /// minimum and string maximum items) whose inclusive lower bound is indicated by Range.StringMin.
        /// </summary>
        [FieldOffset(6)]
        public ushort StringMax;

        /// <summary>
        /// Indicates the inclusive lower bound of a range of designators (specified by designator
        /// minimum and designator maximum items) whose inclusive lower bound is indicated by Range.DesignatorMax.
        /// </summary>
        [FieldOffset(8)]
        public ushort DesignatorMin;

        /// <summary>
        /// Indicates the inclusive upper bound of a range of designators (specified by designator
        /// minimum and designator maximum items) whose inclusive lower bound is indicated by Range.DesignatorMin.
        /// </summary>
        [FieldOffset(10)]
        public ushort DesignatorMax;

        /// <summary>
        /// Indicates the inclusive lower bound of a sequential range of data indices that correspond,
        /// one-to-one and in the same order, to the usages specified by the usage range Range.UsageMin to Range.UsageMax.
        /// </summary>
        [FieldOffset(12)]
        public ushort DataIndexMin;

        /// <summary>
        /// Indicates the inclusive upper bound of a sequential range of data indices that correspond,
        /// one-to-one and in the same order, to the usages specified by the usage range
        /// Range.UsageMin to Range.UsageMax.
        /// </summary>
        [FieldOffset(14)]
        public ushort DataIndexMax;

        public static HidpButtonCapsRange FromBytes(byte[] bytes, int offset, out int nextByteOffset)
        {
            var hbc = new HidpButtonCapsRange()
            {
                UsageMin = (ushort)(((ushort)bytes[offset + 1] << 8) | (ushort)(bytes[offset + 0])),
                UsageMax = (ushort)(((ushort)bytes[offset + 3] << 8) | (ushort)(bytes[offset + 2])),
                StringMin = (ushort)(((ushort)bytes[offset + 5] << 8) | (ushort)(bytes[offset + 4])),
                StringMax = (ushort)(((ushort)bytes[offset + 7] << 8) | (ushort)(bytes[offset + 6])),
                DesignatorMin = (ushort)(((ushort)bytes[offset + 9] << 8) | (ushort)(bytes[offset + 8])),
                DesignatorMax = (ushort)(((ushort)bytes[offset + 11] << 8) | (ushort)(bytes[offset + 10])),
                DataIndexMin = (ushort)(((ushort)bytes[offset + 13] << 8) | (ushort)(bytes[offset + 12])),
                DataIndexMax = (ushort)(((ushort)bytes[offset + 15] << 8) | (ushort)(bytes[offset + 14])),
            };

            nextByteOffset = offset + 15 + 1;

            return hbc;
        }
    }
}
