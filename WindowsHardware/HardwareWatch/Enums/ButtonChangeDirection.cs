using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsHardwareWatch.HardwareWatch.Enums
{
    /// <summary>
    /// How mouse button changed.
    /// </summary>
    public enum ButtonChangeDirection
    {
        /// <summary>
        /// Default / unknown.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Up / release event.
        /// </summary>
        Up = 1,

        /// <summary>
        /// Down / press.
        /// </summary>
        Down = 2,
    }
}
