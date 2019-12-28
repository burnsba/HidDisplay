using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace WinApi.Windows
{
    /// <summary>
    /// The POINT structure defines the x- and y-coordinates of a point.
    /// </summary>
    /// <remarks>
    /// http://pinvoke.net/default.aspx/Structures/POINT.html .
    /// https://docs.microsoft.com/en-us/windows/win32/api/windef/ns-windef-point
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct WindowsPoint
    {
        private static WindowsPoint _invalid = new WindowsPoint(int.MinValue, int.MinValue);

        /// <summary>
        /// The x-coordinate of the point.
        /// </summary>
        public int X;

        /// <summary>
        /// The y-coordinate of the point.
        /// </summary>
        public int Y;

        public WindowsPoint(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public static WindowsPoint Invalid
        {
            get
            {
                return _invalid;
            }
        }

        public static implicit operator System.Drawing.Point(WindowsPoint p)
        {
            return new System.Drawing.Point(p.X, p.Y);
        }

        public static implicit operator WindowsPoint(System.Drawing.Point p)
        {
            return new WindowsPoint(p.X, p.Y);
        }

        public WindowsPoint Delta(WindowsPoint p)
        {
            return new WindowsPoint(p.X - X, p.Y - Y);
        }
    }
}
