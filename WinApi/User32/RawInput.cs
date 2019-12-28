using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WinApi.User32
{
    /// <summary>
    /// Contains the raw input from a device.
    /// </summary>
    /// <remarks>
    /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-rawinput
    /// </remarks>
    [StructLayout(LayoutKind.Explicit)]
    public struct RawInput
    {
        /// <summary>
        /// Header info.
        /// </summary>
        [FieldOffset(0)]
        public RawInputHeader Header;

        /// <summary>
        /// Input data.
        /// </summary>
        [FieldOffset(16)]
        public RawInputData Data;

        public static RawInput FromBytes(byte[] bytes, int offset)
        {
            RawInput ri;

            if ((int)bytes[offset] == (int)RawInputDeviceType.Mouse)
            {
                ri = new RawInput()
                {
                    Header = RawInputHeader.FromBytes(bytes, offset + 0),
                    Data = new RawInputData()
                    {
                        Mouse = RawMouse.FromByteArray(bytes, offset + Marshal.SizeOf(typeof(RawInputHeader)))
                    }
                };
            }
            else if ((int)bytes[offset] == (int)RawInputDeviceType.Keyboard)
            {
                ri = new RawInput()
                {
                    Header = RawInputHeader.FromBytes(bytes, offset + 0),
                    Data = new RawInputData()
                    {
                        Keyboard = RawKeyboard.FromByteArray(bytes, offset + Marshal.SizeOf(typeof(RawInputHeader)))
                    }
                };
            }
            else if ((int)bytes[offset] == (int)RawInputDeviceType.Hid)
            {
                ri = new RawInput()
                {
                    Header = RawInputHeader.FromBytes(bytes, offset + 0),
                    Data = new RawInputData()
                    {
                        Hid = RawHid.FromByteArray(bytes, offset + Marshal.SizeOf(typeof(RawInputHeader)))
                    }
                };
            }
            else
            {
                throw new NotSupportedException();
            }

            return ri;
        }
    }
}
