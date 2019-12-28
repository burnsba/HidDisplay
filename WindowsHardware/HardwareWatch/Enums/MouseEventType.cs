using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsHardwareWatch.HardwareWatch.Enums
{
    /// <summary>
    /// What triggered the mouse event.
    /// </summary>
    public enum MouseEventType
    {
        /// <summary>
        /// Default / unknown.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Move event.
        /// </summary>
        Move = 1,

        /// <summary>
        /// Scroll event.
        /// </summary>
        Scroll = 2,

        /// <summary>
        /// Button press or release.
        /// </summary>
        Button = 3,
    }
}
