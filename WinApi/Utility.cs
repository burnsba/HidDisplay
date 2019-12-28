using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using WinApi.Hid;
using WinApi.Hid.Usage;

namespace WinApi
{
    /// <summary>
    /// Helper methods.
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Converts consecutive bytes to a pointer. Bytes are read in big endianess. Pointer length
        /// is host system pointer size.
        /// </summary>
        /// <param name="bytes">Byte array to read.</param>
        /// <param name="offset">Starting offset containing pointer.</param>
        /// <returns>Pointer.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr MakePointer(byte[] bytes, int offset)
        {
            return MakePointerBigEndian(bytes, offset);
        }

        /// <summary>
        /// Converts consecutive bytes to a pointer. Bytes are read in big endianess. Pointer length
        /// is host system pointer size.
        /// </summary>
        /// <param name="bytes">Byte array to read.</param>
        /// <param name="offset">Starting offset containing pointer.</param>
        /// <returns>Pointer.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr MakePointerBigEndian(byte[] bytes, int offset)
        {
            if (IntPtr.Size == 4)
            {
                return MakePointerBigEndian32(bytes, offset);
            }
            else if (IntPtr.Size == 8)
            {
                return MakePointerBigEndian64(bytes, offset);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Converts consecutive bytes to a pointer. Bytes are read in big endianess. Pointer length
        /// is 32 bits.
        /// </summary>
        /// <param name="bytes">Byte array to read.</param>
        /// <param name="offset">Starting offset containing pointer.</param>
        /// <returns>Pointer.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr MakePointerBigEndian32(byte[] bytes, int offset)
        {
            return new IntPtr((int)(((int)bytes[offset + 3] << 24) | ((int)bytes[offset + 2] << 16) | ((int)bytes[offset + 1] << 8) | (int)(bytes[offset + 0])));
        }

        /// <summary>
        /// Converts consecutive bytes to a pointer. Bytes are read in big endianess. Pointer length
        /// is 64 bits.
        /// </summary>
        /// <param name="bytes">Byte array to read.</param>
        /// <param name="offset">Starting offset containing pointer.</param>
        /// <returns>Pointer.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr MakePointerBigEndian64(byte[] bytes, int offset)
        {
            return new IntPtr((long)((
                (long)bytes[offset + 7] << 56)
                | ((long)bytes[offset + 6] << 48)
                | ((long)bytes[offset + 5] << 40)
                | ((long)bytes[offset + 4] << 32)
                | ((long)bytes[offset + 3] << 24)
                | ((long)bytes[offset + 2] << 16)
                | ((long)bytes[offset + 1] << 8)
                | (long)(bytes[offset + 0])));
        }

        /// <summary>
        /// Converts consecutive bytes to a pointer. Bytes are read in litle endianess. Pointer length
        /// is host system pointer size.
        /// </summary>
        /// <param name="bytes">Byte array to read.</param>
        /// <param name="offset">Starting offset containing pointer.</param>
        /// <returns>Pointer.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static  IntPtr MakePointerLittleEndian(byte[] bytes, int offset)
        {
            if (IntPtr.Size == 4)
            {
                return MakePointerLittleEndian32(bytes, offset);
            }
            else if (IntPtr.Size == 8)
            {
                return MakePointerLittleEndian64(bytes, offset);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Converts consecutive bytes to a pointer. Bytes are read in litle endianess. Pointer length
        /// is 32 bits.
        /// </summary>
        /// <param name="bytes">Byte array to read.</param>
        /// <param name="offset">Starting offset containing pointer.</param>
        /// <returns>Pointer.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr MakePointerLittleEndian32(byte[] bytes, int offset)
        {
            return new IntPtr((int)(((int)bytes[offset + 0] << 24) | ((int)bytes[offset + 1] << 16) | ((int)bytes[offset + 2] << 8) | (int)(bytes[offset + 3])));
        }

        /// <summary>
        /// Converts consecutive bytes to a pointer. Bytes are read in litle endianess. Pointer length
        /// is 64 bits.
        /// </summary>
        /// <param name="bytes">Byte array to read.</param>
        /// <param name="offset">Starting offset containing pointer.</param>
        /// <returns>Pointer.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr MakePointerLittleEndian64(byte[] bytes, int offset)
        {
            return new IntPtr((long)((
                (long)bytes[offset + 0] << 56)
                | ((long)bytes[offset + 1] << 48)
                | ((long)bytes[offset + 2] << 40)
                | ((long)bytes[offset + 3] << 32)
                | ((long)bytes[offset + 4] << 24)
                | ((long)bytes[offset + 5] << 16)
                | ((long)bytes[offset + 6] << 8)
                | (long)(bytes[offset + 7])));
        }

        /// <summary>
        /// Converts usage page and usage into a friendly name.
        /// </summary>
        /// <param name="usagePageRaw">Usage page.</param>
        /// <param name="usageRaw">Usage from usage page.</param>
        /// <returns>Converted value, or originial int if not found.</returns>
        /// <remarks>
        /// https://www.freebsddiary.org/APC/usb_hid_usages.php
        /// </remarks>
        public static string UsagePageAndUsageToString(int usagePageRaw, int usageRaw)
        {
            if (!Enum.IsDefined(typeof(HidUsagePages), usagePageRaw))
                return $"{usagePageRaw}:{usageRaw}";

            var up = (HidUsagePages)usagePageRaw;

            switch (up)
            {
                case (HidUsagePages.GenericDesktop):
                    if (Enum.IsDefined(typeof(GenericDesktop), usageRaw))
                        return $"{up}:{((GenericDesktop)usageRaw).ToString()}";
                    break;

                case (HidUsagePages.SimulationControl):
                    if (Enum.IsDefined(typeof(SimulationControl), usageRaw))
                        return $"{up}:{((SimulationControl)usageRaw).ToString()}";
                    break;

                case (HidUsagePages.VRControl):
                    if (Enum.IsDefined(typeof(VRControl), usageRaw))
                        return $"{up}:{((VRControl)usageRaw).ToString()}";
                    break;

                case (HidUsagePages.SportsControl):
                    if (Enum.IsDefined(typeof(SportsControl), usageRaw))
                        return $"{up}:{((SportsControl)usageRaw).ToString()}";
                    break;

                case (HidUsagePages.GameControl):
                    if (Enum.IsDefined(typeof(GameControl), usageRaw))
                        return $"{up}:{((GameControl)usageRaw).ToString()}";
                    break;

                case (HidUsagePages.Keyboard):
                    if (Enum.IsDefined(typeof(Keyboard), usageRaw))
                        return $"{up}:{((Keyboard)usageRaw).ToString()}";
                    break;

                case (HidUsagePages.LED):
                    if (Enum.IsDefined(typeof(LED), usageRaw))
                        return $"{up}:{((LED)usageRaw).ToString()}";
                    break;

                case (HidUsagePages.Telephony):
                    if (Enum.IsDefined(typeof(Telephony), usageRaw))
                        return $"{up}:{((Telephony)usageRaw).ToString()}";
                    break;

                case (HidUsagePages.Consumer):
                    if (Enum.IsDefined(typeof(Consumer), usageRaw))
                        return $"{up}:{((Consumer)usageRaw).ToString()}";
                    break;

                case (HidUsagePages.Digitizer):
                    if (Enum.IsDefined(typeof(Digitizer), usageRaw))
                        return $"{up}:{((Digitizer)usageRaw).ToString()}";
                    break;

                case (HidUsagePages.AlphnumericDisplay):
                    if (Enum.IsDefined(typeof(AlphnumericDisplay), usageRaw))
                        return $"{up}:{((AlphnumericDisplay)usageRaw).ToString()}";
                    break;

                case (HidUsagePages.Monitor):
                    if (Enum.IsDefined(typeof(Monitor), usageRaw))
                        return $"{up}:{((Monitor)usageRaw).ToString()}";
                    break;

                case (HidUsagePages.VESAVirtualControls):
                    if (Enum.IsDefined(typeof(VESAVirtualControls), usageRaw))
                        return $"{up}:{((VESAVirtualControls)usageRaw).ToString()}";
                    break;

                case (HidUsagePages.VESACommand):
                    if (Enum.IsDefined(typeof(VESACommand), usageRaw))
                        return $"{up}:{((VESACommand)usageRaw).ToString()}";
                    break;

                case (HidUsagePages.BatterySystem):
                    if (Enum.IsDefined(typeof(BatterySystem), usageRaw))
                        return $"{up}:{((BatterySystem)usageRaw).ToString()}";
                    break;

                // special cases

                case (HidUsagePages.Button):
                    if (usageRaw == 0)
                        return $"{up}:No Button Pressed";
                    return $"{up}:Button {usageRaw}";

                case (HidUsagePages.Ordinal):
                    if (usageRaw == 0)
                        return $"{up}:Unused";
                    return $"{up}:Instance {usageRaw}";

                case (HidUsagePages.Unicode):
                    throw new NotImplementedException("value=Unicode Char u%04x");

                case (HidUsagePages.MonitorEnumeratedValues):
                    if (usageRaw == 0)
                        return $"{up}:unassigned";
                    return $"{up}:ENUM {usageRaw}";

                default:
                    break;
            }

            return $"{up}:{usageRaw}";
        }

        /// <summary>
        /// Converts usage page into a friendly name.
        /// </summary>
        /// <param name="usagePageRaw">Usage page.</param>
        /// <returns>Converted value, or originial int if not found.</returns>
        /// <remarks>
        /// https://www.freebsddiary.org/APC/usb_hid_usages.php
        /// </remarks>
        public static string UsagePageToString(int usagePageRaw)
        {
            if (!Enum.IsDefined(typeof(HidUsagePages), usagePageRaw))
                return $"{usagePageRaw}";

            var up = (HidUsagePages)usagePageRaw;

            return up.ToString();
        }
    }
}
