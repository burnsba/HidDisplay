using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WinApi.User32
{
    /// <summary>
    /// Contains the header information that is part of the raw input data.
    /// </summary>
    /// <remarks>
    /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-rawinputheader
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct RawInputHeader
    {
        /// <summary>
        /// The type of raw input.
        /// </summary>
        [MarshalAs(UnmanagedType.U4)]
        public RawInputDeviceType dwType;

        /// <summary>
        /// The size, in bytes, of the entire input packet of data. This includes
        /// <see cref="RawInput"/> plus possible extra input reports in
        /// the <see cref="RawHid"/> variable length array.
        /// </summary>
        [MarshalAs(UnmanagedType.U4)]
        public uint dwSize;

        /// <summary>
        /// A handle to the device generating the raw input data.
        /// This is different from the handle obtained by calling <see cref="Kernel32.Api.CreateFile(string, uint, uint, IntPtr, uint, uint, IntPtr)"/>.
        /// </summary>
        public IntPtr hDevice;

        /// <summary>
        /// The value passed in the wParam parameter of the WM_INPUT message.
        /// RIM_INPUT 0 if input occurred while application was in the foreground else RIM_INPUTSINK 1 if it was not.
        /// </summary>
        public IntPtr wParam;

        public override string ToString()
        {
            return string.Format("RawInputHeader\n dwType : {0}\n dwSize : {1}\n hDevice : {2}\n wParam : {3}", dwType, dwSize, hDevice, wParam);
        }

        public static RawInputHeader FromBytes(byte[] bytes, int offset, out int nextByteOffset)
        {
            var ptrsize = IntPtr.Size;

            var rih = new RawInputHeader()
            {
                dwType = (RawInputDeviceType)(((uint)bytes[offset + 3] << 24) | ((uint)bytes[offset + 2] << 16) | ((uint)bytes[offset + 1] << 8) | (uint)(bytes[offset + 0])),
                dwSize = (uint)(((uint)bytes[offset + 7] << 24) | ((uint)bytes[offset + 6] << 16) | ((uint)bytes[offset + 5] << 8) | (uint)(bytes[offset + 4])),
                hDevice = Utility.MakePointer(bytes, 8),
                wParam = Utility.MakePointer(bytes, 8 + IntPtr.Size),
            };

            nextByteOffset = offset + 7 + IntPtr.Size + IntPtr.Size + 1;

            return rih;
        }
    }
}
