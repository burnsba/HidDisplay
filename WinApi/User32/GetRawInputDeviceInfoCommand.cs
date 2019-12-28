using System;
using System.Collections.Generic;
using System.Text;

namespace WinApi.User32
{
    /// <summary>
    /// Command flag for <see cref="Api.GetRawInputDeviceInfo(IntPtr, GetRawInputDeviceInfoCommand, IntPtr, ref uint)"/>.
    /// </summary>
    /// <remarks>
    /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getrawinputdeviceinfoa
    /// </remarks>
    public enum GetRawInputDeviceInfoCommand
    {
        /// <summary>
        /// pData points to a string that contains the device name.
        /// 
        /// For this uiCommand only, the value in pcbSize is the character count (not the byte count).
        /// </summary>
        RIDI_DEVICENAME = 0x20000007,

        /// <summary>
        /// pData points to an <see cref="User32.RidDeviceInfo"/> structure. 
        /// </summary>
        RIDI_DEVICEINFO = 0x2000000b,

        /// <summary>
        /// pData points to the previously parsed data. 
        /// </summary>
        RIDI_PREPARSEDDATA = 0x20000005,
    }
}
