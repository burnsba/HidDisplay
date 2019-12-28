using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinApi.User32
{
    /// <summary>
    /// Enumeration contanining the command types to issue.
    /// Flag used by <see cref="Api.GetRawInputData(IntPtr, RawInputCommand, IntPtr, ref uint, int)"/>.
    /// </summary>
    /// <remarks>
    /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getrawinputdata
    /// http://www.pinvoke.net/default.aspx/Enums/RawInputCommand.html
    /// </remarks>
    public enum RawInputCommand
    {
        /// <summary>
        /// Get input data.
        /// </summary>
        Input = 0x10000003,

        /// <summary>
        /// Get header data.
        /// </summary>
        Header = 0x10000005
    }
}
