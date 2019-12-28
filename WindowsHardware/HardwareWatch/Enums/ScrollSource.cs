using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsHardwareWatch.HardwareWatch.Enums
{
    /// <summary>
    /// Which scroll wheel changed.
    /// </summary>
    public enum ScrollSource
    {
        /// <summary>
        /// Default / unknown.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Default/vertical scroll.
        /// </summary>
        VerticalScrollWheel = 1,

        /// <summary>
        /// Horizontal scroll.
        /// </summary>
        HorizontalScrollWheel = 2,

        /// <summary>
        /// Scroll wheel 3.
        /// </summary>
        Scroll3 = 3,

        /// <summary>
        /// Scroll wheel 4.
        /// </summary>
        Scroll4 = 4,

        /// <summary>
        /// Scroll wheel 5.
        /// </summary>
        Scroll5 = 5,
    }
}
