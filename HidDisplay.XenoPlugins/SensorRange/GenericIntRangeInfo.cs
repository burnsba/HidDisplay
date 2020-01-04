using System;
using System.Collections.Generic;
using System.Text;
using HidDisplay.PluginDefinition;

namespace HidDisplay.XenoPlugins.SensorRange
{
    /// <summary>
    /// Generic int range info.
    /// </summary>
    public class GenericIntRangeInfo : IRangeableInputDescription
    {
        private static GenericIntRangeInfo _instance = new GenericIntRangeInfo();

        /// <summary>
        /// Gets the static instance.
        /// </summary>
        public static GenericIntRangeInfo Instance { get { return _instance; } }

        /// <inheritdoc />
        public string Name => "Generic integer range";

        /// <inheritdoc />
        public int MinValueInt => 0;

        /// <inheritdoc />
        public int MaxValueInt => int.MaxValue;

        /// <inheritdoc />
        public decimal MinValueDecimal => 0.0m;

        /// <inheritdoc />
        public decimal MaxValueDecimal => int.MaxValue;

        /// <inheritdoc />
        public double MinValueDouble => 0.0;

        /// <inheritdoc />
        public double MaxValueDouble => int.MaxValue;

        /// <inheritdoc />
        public float MinValueSingle => 0.0f;

        /// <inheritdoc />
        public float MaxValueSingle => int.MaxValue;

        /// <inheritdoc />
        public Type BaseType => typeof(int);
    }
}
