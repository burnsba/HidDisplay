using System;
using System.Collections.Generic;
using System.Text;

namespace HidDisplay.PluginDefinition
{
    public interface IRangeableInput3 : IInputSource
    {
        /// <summary>
        /// Gets or sets "empty" flag.
        /// </summary>
        /// <remarks>
        /// There should be a way to distinguish between an empty reading, and
        /// a neutral/zero reading.
        /// </remarks>
        bool IsEmpty { get; }

        /// <summary>
        /// Gets the first value.
        /// </summary>
        IRangeableInput Value1 { get; }

        /// <summary>
        /// Gets the second value.
        /// </summary>
        IRangeableInput Value2 { get; }

        /// <summary>
        /// Gets the third value.
        /// </summary>
        IRangeableInput Value3 { get; }
    }
}
