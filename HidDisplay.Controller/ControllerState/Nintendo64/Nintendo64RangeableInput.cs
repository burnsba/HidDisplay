using HidDisplay.PluginDefinition;
using System;
using System.Collections.Generic;
using System.Text;

namespace HidDisplay.Controller.ControllerState.Nintendo64
{
    /// <summary>
    /// Nintendo64 analog stick movement.
    /// </summary>
    public sealed class Nintendo64RangeableInput : IRangeableInput2
    {
        private static Nintendo64RangeableInput _empty =
            new Nintendo64RangeableInput(0, 0)
            {
                IsEmpty = true
            };

        /// <summary>
        /// Gets the generic null movement.
        /// </summary>
        public static Nintendo64RangeableInput Empty
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
        /// Gets mouse X value.
        /// </summary>
        public IRangeableInput Value1 { get; private set; }

        /// <summary>
        /// Gets mouse Y value.
        /// </summary>
        public IRangeableInput Value2 { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Nintendo64RangeableInput"/> class.
        /// </summary>
        /// <param name="x">X value.</param>
        /// <param name="y">Y value.</param>
        public Nintendo64RangeableInput(short x, short y)
        {
            Value1 = new Nintendo64AxisInput(x) { Id = 1, Name = "X" };
            Value2 = new Nintendo64AxisInput(y) { Id = 2, Name = "Y" };
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
