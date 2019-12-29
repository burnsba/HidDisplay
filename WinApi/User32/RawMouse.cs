using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WinApi.User32
{
    /// <summary>
    /// Contains information about the state of the mouse.
    /// One possible union for property <see cref="RawInput.Data"/>.
    /// </summary>
    /// <remarks>
    /// http://www.pinvoke.net/default.aspx/Structures/RAWINPUTMOUSE.html
    /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-rawmouse
    /// </remarks>
    public struct RawMouse
    {
        /// <summary>
        /// The mouse state.
        /// </summary>
        public RawMouseFlags Flags;

        public struct Data
        {
            public uint Buttons;

            /// <summary>
            /// If the mouse wheel is moved, this will contain the delta amount.
            /// </summary>
            public ushort ButtonData;

            /// <summary>
            /// Flags for the event.
            /// </summary>
            public RawMouseButtons ButtonFlags;

            public static Data FromBytes(byte[] bytes, int offset, out int nextByteOffset)
            {
                var d = new Data()
                {
                    Buttons = (uint)(((uint)bytes[offset + 3] << 24) | ((uint)bytes[offset + 2] << 16) | ((uint)bytes[offset + 1] << 8) | (uint)(bytes[offset + 0])),

                    ButtonFlags = (RawMouseButtons)(((ushort)bytes[offset + 1] << 8) | (ushort)(bytes[offset + 0])),
                    ButtonData = (ushort)(((ushort)bytes[offset + 3] << 8) | (ushort)(bytes[offset + 2])),
                };

                nextByteOffset = offset + 3 + 1;

                return d;
            }
        }

        /// <summary>
        /// Mouse data.
        /// </summary>
        public Data RawMouseData;

        /// <summary>
        /// Raw button data.
        /// </summary>
        public uint RawButtons;

        /// <summary>
        /// The motion in the X direction. This is signed relative motion or
        /// absolute motion, depending on the value of usFlags.
        /// </summary>
        public int LastX;

        /// <summary>
        /// The motion in the Y direction. This is signed relative motion or absolute motion,
        /// depending on the value of usFlags.
        /// </summary>
        public int LastY;

        /// <summary>
        /// The device-specific additional information for the event.
        /// </summary>
        public uint ExtraInformation;

        public static RawMouse FromBytes(byte[] bytes, int offset, out int nextByteOffset)
        {
            // Something is wrong, the online documentation says the first field is two bytes.
            // But (when testing real data) everything after the first field is shifted by two bytes, which makes
            // it seem like the first field is actually four bytes.
            //
            // https://docs.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-rawmouse
            // https://github.com/tpn/winsdk-10/blob/master/Include/10.0.10240.0/um/WinUser.h

            int dataOffset;

            // should be "offset + 2", but that doesn't work?
            var data = Data.FromBytes(bytes, offset + 4, out dataOffset); 

            var rm = new RawMouse()
            {
                Flags = (RawMouseFlags)(((ushort)bytes[offset + 1] << 8) | (ushort)(bytes[offset])),
                RawMouseData = data,

                RawButtons = (uint)(((uint)bytes[dataOffset + 3] << 24) | ((uint)bytes[dataOffset + 2] << 16) | ((uint)bytes[dataOffset + 1] << 8) | (uint)(bytes[dataOffset + 0])),
                LastX = (int)(((int)bytes[dataOffset + 7] << 24) | ((int)bytes[dataOffset + 6] << 16) | ((int)bytes[dataOffset + 5] << 8) | (int)(bytes[dataOffset + 4])),
                LastY = (int)(((int)bytes[dataOffset + 11] << 24) | ((int)bytes[dataOffset + 10] << 16) | ((int)bytes[dataOffset + 9] << 8) | (int)(bytes[dataOffset + 8])),
                ExtraInformation = (uint)(((uint)bytes[dataOffset + 15] << 24) | ((uint)bytes[dataOffset + 14] << 16) | ((uint)bytes[dataOffset + 13] << 8) | (uint)(bytes[dataOffset + 12])),
            };

            nextByteOffset = dataOffset + 15 + 1;

            return rm;
        }
    }
}
