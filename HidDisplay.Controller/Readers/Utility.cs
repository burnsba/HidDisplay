using System;
using System.Collections.Generic;
using System.Text;

namespace HidDisplay.Controller.Readers
{
    /// <summary>
    /// Helper functions.
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Reverses the bits in a byte.
        /// </summary>
        /// <param name="b">Byte to reverse.</param>
        /// <returns>Reversed byte.</returns>
        public static int Reverse(Byte b)
        {
            int i = (int)b;

            i = (i & 0xF0) >> 4 | (i & 0x0F) << 4;
            i = (i & 0xCC) >> 2 | (i & 0x33) << 2;
            i = (i & 0xAA) >> 1 | (i & 0x55) << 1;

            return i;
        }

        /// <summary>
        /// Reverses the bits in a byte. If the highest bit is set, 255 is subtracted.
        /// </summary>
        /// <param name="b">Byte to reverse and convert to signed value.</param>
        /// <returns>Reversed signed byte.</returns>
        public static int MakeSignedReverseByte(byte b)
        {
            int i = (int)b;

            i = (i & 0xF0) >> 4 | (i & 0x0F) << 4;
            i = (i & 0xCC) >> 2 | (i & 0x33) << 2;
            i = (i & 0xAA) >> 1 | (i & 0x55) << 1;

            if ((i & 0x80) > 0)
            {
                return i - 255;
            }

            return i;
        }

        /// <summary>
        /// Treats an array of bytes as 8 bits and converts to a signed int. A non-zero
        /// value of the byte is treated as a 1 bit.
        /// </summary>
        /// <param name="b">Byte array to convert.</param>
        /// <param name="startIndex">Starting index of byte array.</param>
        /// <returns>Signed int.</returns>
        public static int MakeSignedReverseByte(byte[] b, int startIndex)
        {
            int i = 0;

            i = ((b[startIndex] > 1) ? 1 : 0) << 7;
            i |= ((b[startIndex + 1] > 1) ? 1 : 0) << 6;
            i |= ((b[startIndex + 2] > 1) ? 1 : 0) << 5;
            i |= ((b[startIndex + 3] > 1) ? 1 : 0) << 4;
            i |= ((b[startIndex + 4] > 1) ? 1 : 0) << 3;
            i |= ((b[startIndex + 5] > 1) ? 1 : 0) << 2;
            i |= ((b[startIndex + 6] > 1) ? 1 : 0) << 1;
            i |= ((b[startIndex + 7] > 1) ? 1 : 0) << 0;

            if ((i & 0x80) > 0)
            {
                return i - 255;
            }

            return i;
        }
    }
}
