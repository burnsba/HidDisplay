using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WinApi.User32
{
    /// <summary>
    /// Describes the format of the raw input from a Human Interface Device (HID).
    /// One possible union for property <see cref="RawInput.Data"/>.
    /// </summary>
    /// <remarks>
    /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-rawhid
    /// </remarks>
    [StructLayout(LayoutKind.Explicit)]
    public struct RawHid
    {
        /// <summary>
        /// The size, in bytes, of each HID input in bRawData.
        /// </summary>
        [FieldOffset(0)]
        [MarshalAs(UnmanagedType.U4)]
        public int dwSizeHid;

        /// <summary>
        /// The number of HID inputs in bRawData.
        /// </summary>
        [FieldOffset(4)]
        [MarshalAs(UnmanagedType.U4)]
        public int dwCount;

        /// <summary>
        /// The raw input data, as an array of bytes.
        /// </summary>
        [FieldOffset(8)]
        public byte[] bRawData;

        public static RawHid FromBytes(byte[] bytes, int offset, out int nextByteOffset)
        {
            int dwSizeHid = (int)(((int)bytes[offset + 3] << 24) | ((int)bytes[offset + 2] << 16) | ((int)bytes[offset + 1] << 8) | (int)(bytes[offset]));
            int dwCount = (int)(((int)bytes[offset + 7] << 24) | ((int)bytes[offset + 6] << 16) | ((int)bytes[offset + 5] << 8) | (int)(bytes[offset + 4]));
            int len = dwSizeHid * dwCount;
            byte[] arrdata = new byte[len];
            Array.Copy(bytes, offset+8, arrdata, 0, len);

            var hid = new RawHid()
            {
                dwSizeHid = dwSizeHid,
                dwCount = dwCount,
                bRawData = arrdata
            };

            nextByteOffset = offset + 7 + len + 1;

            return hid;
        }
    }
}
