using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsHardwareWatch.HardwareWatch.Enums
{
    /// <summary>
    /// How to match window title.
    /// </summary>
    /// <remarks>
    /// Used by <see cref="LowLevelWatcher"/>.
    /// </remarks>
    public enum WindowTitleMatch
    {
        /// <summary>
        /// Default / unknown.
        /// </summary>
        Unknown,

        /// <summary>
        /// Exact title match (case is ignored).
        /// </summary>
        Exact,

        /// <summary>
        /// Title contains this text (case is ignored).
        /// </summary>
        Contains,

        /// <summary>
        /// Title begins with this text (case is ignored).
        /// </summary>
        BeginsWith,

        /// <summary>
        /// Title ends with this text (case is ignored).
        /// </summary>
        EndsWith,

        /// <summary>
        /// Title matches regex.
        /// </summary>
        Regex,
    }
}
