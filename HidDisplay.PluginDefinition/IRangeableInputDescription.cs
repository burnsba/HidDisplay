using System;
using System.Collections.Generic;
using System.Text;

namespace HidDisplay.PluginDefinition
{
    /// <summary>
    /// Describes input ranges.
    /// </summary>
    public interface IRangeableInputDescription
    {
        /// <summary>
        /// Gets name of range description.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets min value as integer for range.
        /// </summary>
        int MinValueInt { get; }

        /// <summary>
        /// Gets max value as integer for range.
        /// </summary>
        int MaxValueInt { get; }

        /// <summary>
        /// Gets min value as decimal for range.
        /// </summary>
        decimal MinValueDecimal { get; }

        /// <summary>
        /// Gets max value as decimal for range.
        /// </summary>
        decimal MaxValueDecimal { get; }

        /// <summary>
        /// Gets min value as double for range.
        /// </summary>
        double MinValueDouble { get; }

        /// <summary>
        /// Gets max value as double for range.
        /// </summary>
        double MaxValueDouble { get; }

        /// <summary>
        /// Gets min value as single for range.
        /// </summary>
        Single MinValueSingle { get; }

        /// <summary>
        /// Gets max value as single for range.
        /// </summary>
        Single MaxValueSingle { get; }

        /// <summary>
        /// Gets the base data type: int, single, double, decimal.
        /// </summary>
        Type BaseType { get; }
    }
}
