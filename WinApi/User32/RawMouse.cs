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
    [StructLayout(LayoutKind.Sequential)]
    public struct RawMouse
    {
        /// <summary>
        /// The mouse state.
        /// </summary>
        public RawMouseFlags Flags;

        [StructLayout(LayoutKind.Explicit)]
        public struct Data
        {
            [FieldOffset(0)]
            public uint Buttons;

            /// <summary>
            /// If the mouse wheel is moved, this will contain the delta amount.
            /// </summary>
            [FieldOffset(2)]
            public ushort ButtonData;

            /// <summary>
            /// Flags for the event.
            /// </summary>
            [FieldOffset(0)]
            public RawMouseButtons ButtonFlags;
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

        public static RawMouse FromByteArray(byte[] bytes, int offset)
        {
            var rm = new RawMouse()
            {
                Flags = (RawMouseFlags)(((ushort)bytes[offset + 1] << 8) | (ushort)(bytes[offset])),
                RawMouseData = new Data() {
                    Buttons = (uint)(((uint)bytes[offset + 5] << 24) | ((uint)bytes[offset + 4] << 16) | ((uint)bytes[offset + 3] << 8) | (uint)(bytes[offset + 2]))
                },
                RawButtons = (uint)(((uint)bytes[offset + 9] << 24) | ((uint)bytes[offset + 8] << 16) | ((uint)bytes[offset + 7] << 8) | (uint)(bytes[offset + 6])),
                LastX = (int)(((int)bytes[offset + 13] << 24) | ((int)bytes[offset + 12] << 16) | ((int)bytes[offset + 11] << 8) | (int)(bytes[offset + 10])),
                LastY = (int)(((int)bytes[offset + 17] << 24) | ((int)bytes[offset + 16] << 16) | ((int)bytes[offset + 15] << 8) | (int)(bytes[offset + 14])),
                ExtraInformation = (uint)(((uint)bytes[offset + 21] << 24) | ((uint)bytes[offset + 20] << 16) | ((uint)bytes[offset + 19] << 8) | (uint)(bytes[offset + 18])),
            };

            return rm;
        }
    }
}
