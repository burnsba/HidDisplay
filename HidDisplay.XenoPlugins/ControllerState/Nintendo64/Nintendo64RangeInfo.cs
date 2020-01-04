using System;
using HidDisplay.PluginDefinition;

namespace HidDisplay.Controller.ControllerState.Nintendo64
{
    /// <summary>
    /// Minimal implementation to describe movement value.
    /// </summary>
    public sealed class Nintendo64RangeInfo : IRangeableInputDescription
    {
        private static Nintendo64RangeInfo _instance = new Nintendo64RangeInfo();

        /// <summary>
        /// Gets the static instance.
        /// </summary>
        public static Nintendo64RangeInfo Instance { get { return _instance; } }

        /// <inheritdoc />
        public string Name { get { return "Nintendo64AnalogStickDescription"; } }

        /// <inheritdoc />
        public int MinValueInt { get { return -127; } }

        /// <inheritdoc />
        public int MaxValueInt { get { return 128; } }

        /// <inheritdoc />
        public decimal MinValueDecimal { get { return -127; } }

        /// <inheritdoc />
        public decimal MaxValueDecimal { get { return 128; } }

        /// <inheritdoc />
        public double MinValueDouble { get { return -127; } }

        /// <inheritdoc />
        public double MaxValueDouble { get { return 128; } }

        /// <inheritdoc />
        public Single MinValueSingle { get { return -127; } }

        /// <inheritdoc />
        public Single MaxValueSingle { get { return 128; } }

        /// <inheritdoc />
        public Type BaseType { get { return typeof(short); } }

        /// <inheritdoc />
        public override string ToString()
        {
            return Name.ToString();
        }
    }
}
