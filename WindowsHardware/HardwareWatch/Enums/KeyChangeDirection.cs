using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsHardwareWatch.HardwareWatch.Enums
{
    /// <summary>
    /// How key changed.
    /// </summary>
    public enum KeyChangeDirection
    {
        /// <summary>
        /// Default / unknown.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Key up event.
        /// </summary>
        KeyUp = 1,

        /// <summary>
        /// Key down event.
        /// </summary>
        KeyDown = 2,
    }
}
