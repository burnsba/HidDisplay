using System;
using System.Collections.Generic;
using System.Text;
using HidDisplay.PluginDefinition;

namespace HidDisplay.DefaultPlugins
{
    /// <summary>
    /// Mouse movement coordinates.
    /// </summary>
    public sealed class MouseMoveRangeableInput : IRangeableInput2
    {
        private static MouseMoveRangeableInput _empty = 
            new MouseMoveRangeableInput(double.NaN, double.NaN) 
            {
                IsEmpty = true
            };

        /// <summary>
        /// Gets the generic null movement.
        /// </summary>
        public static MouseMoveRangeableInput Empty
        {
            get
            {
                return _empty;
            }
        }

        /// <inheritdoc />
        public int Id { get; set; }

        /// <inheritdoc />
        public string Name { get; set; }
        
        /// <inheritdoc />
        public bool IsEmpty { get; set; } = false;

        /// <summary>
        /// Gets mouse X movement.
        /// </summary>
        public IRangeableInput Value1 { get; private set; }

        /// <summary>
        /// Gets mouse Y movement.
        /// </summary>
        public IRangeableInput Value2 { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MouseMoveRangeableInput"/> class.
        /// </summary>
        /// <param name="x">X value.</param>
        /// <param name="y">Y value.</param>
        public MouseMoveRangeableInput(double x, double y)
        {
            Value1 = new MouseMoveAxisInput(x) { Id = 1, Name = "X" };
            Value2 = new MouseMoveAxisInput(y) { Id = 2, Name = "Y" };
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{Value1}, {Value2}";
        }

        /// <inheritdoc />
        public UInt64 EventSourceId
        {
            get
            {
                return (UInt64)Id;
            }
        }
    }
}
