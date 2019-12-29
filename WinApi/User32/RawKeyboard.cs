using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WinApi.User32
{
    /// <summary>
    /// Contains information about the state of the keyboard.
    /// One possible union for property <see cref="RawInput.Data"/>.
    /// </summary>
    /// <remarks>
    /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-rawkeyboard
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct RawKeyboard
    {
        /// <summary>
        /// The scan code from the key depression.
        /// The scan code for keyboard overrun is KEYBOARD_OVERRUN_MAKE_CODE.
        /// </summary>
        public ushort Makecode;

        /// <summary>
        /// One or more of RI_KEY_MAKE, RI_KEY_BREAK, RI_KEY_E0, RI_KEY_E1
        /// </summary>
        public ushort Flags;

        /// <summary>
        /// Reserved; must be zero.
        /// </summary>
        internal ushort Reserved;

        /// <summary>
        /// Windows message compatible virtual-key code. For more information, see Virtual Key Codes.
        /// </summary>
        public ushort VKey;

        /// <summary>
        /// The corresponding window message, for example WM_KEYDOWN, WM_SYSKEYDOWN, and so forth.
        /// </summary>
        public uint Message;

        /// <summary>
        /// The device-specific additional information for the event.
        /// </summary>
        public uint ExtraInformation;

        public override string ToString()
        {
            return string.Format("Rawkeyboard\n Makecode: {0}\n Makecode(hex) : {0:X}\n Flags: {1}\n Reserved: {2}\n VKeyName: {3}\n Message: {4}\n ExtraInformation {5}\n",
                                                Makecode, Flags, Reserved, VKey, Message, ExtraInformation);
        }

        public static RawKeyboard FromBytes(byte[] bytes, int offset, out int nextByteOffset)
        {
            var rk = new RawKeyboard()
            {
                Makecode = (ushort)(((ushort)bytes[offset + 1] << 8) | (ushort)(bytes[offset])),
                Flags = (ushort)(((ushort)bytes[offset + 3] << 8) | (ushort)(bytes[offset + 2])),
                Reserved = (ushort)(((ushort)bytes[offset + 5] << 8) | (ushort)(bytes[offset + 4])),
                VKey = (ushort)(((ushort)bytes[offset + 7] << 8) | (ushort)(bytes[offset + 6])),
                Message = (uint)(((int)bytes[offset + 11] << 24) | ((int)bytes[offset + 10] << 16) | ((int)bytes[offset + 9] << 8) | (int)(bytes[offset + 8])),
                ExtraInformation = (uint)(((uint)bytes[offset + 15] << 24) | ((uint)bytes[offset + 14] << 16) | ((uint)bytes[offset + 13] << 8) | (uint)(bytes[offset + 12])),
            };

            nextByteOffset = offset + 15 + 1;

            return rk;
        }
    }
}
