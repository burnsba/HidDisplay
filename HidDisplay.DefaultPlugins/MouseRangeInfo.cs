using System;
using System.Collections.Generic;
using System.Text;
using HidDisplay.PluginDefinition;

namespace HidDisplay.DefaultPlugins
{
    /// <summary>
    /// Minimal implementation to describe movement value.
    /// </summary>
    public sealed class MouseRangeInfo : IRangeableInputDescription
    {
        private static MouseRangeInfo _instance = new MouseRangeInfo();

        /// <summary>
        /// Gets the static instance.
        /// </summary>
        public static MouseRangeInfo Instance { get { return _instance; } }

        /// <inheritdoc />
        public string Name { get { return "MouseMoveDescription"; } }

        /// <inheritdoc />
        public int MinValueInt { get { throw new NotImplementedException(); } }

        /// <inheritdoc />
        public int MaxValueInt { get { throw new NotImplementedException(); } }

        /// <inheritdoc />
        public decimal MinValueDecimal { get { throw new NotImplementedException(); } }

        /// <inheritdoc />
        public decimal MaxValueDecimal { get { throw new NotImplementedException(); } }

        /// <inheritdoc />
        public double MinValueDouble { get { throw new NotImplementedException(); } }

        /// <inheritdoc />
        public double MaxValueDouble { get { throw new NotImplementedException(); } }

        /// <inheritdoc />
        public Single MinValueSingle { get { throw new NotImplementedException(); } }

        /// <inheritdoc />
        public Single MaxValueSingle { get { throw new NotImplementedException(); } }

        /// <inheritdoc />
        public Type BaseType { get { return typeof(double); } }

        /// <inheritdoc />
        public override string ToString()
        {
            return Name.ToString();
        }
    }
}
