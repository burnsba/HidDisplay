using System;
using System.Collections.Generic;
using System.Text;

namespace HidDisplay.PluginDefinition
{
    /// <summary>
    /// One dimensional rangeable input.
    /// </summary>
    public interface IRangeableInput : IInputSource
    {
        /// <summary>
        /// Gets object value.
        /// </summary>
        object Value { get; }

        /// <summary>
        /// Gets object value as integer.
        /// </summary>
        int ValueInt { get; }

        /// <summary>
        /// Gets or sets "empty" flag.
        /// </summary>
        /// <remarks>
        /// There should be a way to distinguish between an empty reading, and
        /// a neutral/zero reading.
        /// </remarks>
        bool IsEmpty { get; }

        /// <summary>
        /// Gets object value as decimal.
        /// </summary>
        decimal ValueDecimal { get; }

        /// <summary>
        /// Gets object value as single.
        /// </summary>
        Single ValueSingle { get; }

        /// <summary>
        /// Gets object value as double.
        /// </summary>
        double ValueDouble { get; }

        /// <summary>
        /// Gets range description.
        /// </summary>
        IRangeableInputDescription RangeInfo { get; }
    }
}
