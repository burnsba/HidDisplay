using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinApi.User32
{
    /// <summary>
    /// Enumeration containing the button data for raw mouse input.
    /// </summary>
    /// <remarks>
    /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-rawmouse
    /// </remarks>
    [Flags()]
    public enum RawMouseButtons
        : ushort
    {
        /// <summary>
        /// No button.
        /// </summary>
        None = 0,

        /// <summary>
        /// Left (button 1) down.
        /// </summary>
        LeftDown = 0x0001,

        /// <summary>
        /// Left (button 1) up.
        /// </summary>
        LeftUp = 0x0002,

        /// <summary>
        /// Right (button 2) down.
        /// </summary>
        RightDown = 0x0004,

        /// <summary>
        /// Right (button 2) up.
        /// </summary>
        RightUp = 0x0008,

        /// <summary>
        /// Middle (button 3) down.
        /// </summary>
        MiddleDown = 0x0010,

        /// <summary>
        /// Middle (button 3) up.
        /// </summary>
        MiddleUp = 0x0020,

        /// <summary>
        /// Button 4 down.
        /// </summary>
        Button4Down = 0x0040,

        /// <summary>
        /// Button 4 up.
        /// </summary>
        Button4Up = 0x0080,

        /// <summary>
        /// Button 5 down.
        /// </summary>
        Button5Down = 0x0100,

        /// <summary>
        /// Button 5 up.
        /// </summary>
        Button5Up = 0x0200,

        /// <summary>
        /// Mouse wheel moved.
        /// </summary>
        MouseWheel = 0x0400
    }
}
