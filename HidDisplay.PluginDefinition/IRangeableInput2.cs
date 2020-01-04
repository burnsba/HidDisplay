using System;
using System.Collections.Generic;
using System.Text;

namespace HidDisplay.PluginDefinition
{
    /// <summary>
    /// Defines two dimensional ranging input.
    /// </summary>
    public interface IRangeableInput2 : IInputSource
    {
        /// <summary>
        /// Gets a value indicating whether the input is "empty".
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
    }
}
