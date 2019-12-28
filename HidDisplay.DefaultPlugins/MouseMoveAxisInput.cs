using System;
using System.Collections.Generic;
using System.Text;
using HidDisplay.PluginDefinition;


namespace HidDisplay.DefaultPlugins
{
    /// <summary>
    /// Mouse movement translator for single axis value.
    /// </summary>
    public sealed class MouseMoveAxisInput : IRangeableInput
    {
        private double _value;

        /// <inheritdoc />
        public int Id { get; set; }

        /// <inheritdoc />
        public string Name { get; set; }

        /// <inheritdoc />
        public bool IsEmpty { get; set; } = false;

        /// <inheritdoc />
        public object Value { get => _value; private set => _value = (double)value; }

        /// <inheritdoc />
        public int ValueInt { get => Convert.ToInt32(_value); set => _value = Convert.ToDouble(value); }

        /// <inheritdoc />
        public decimal ValueDecimal { get => Convert.ToDecimal(_value); set => _value = Convert.ToDouble(value); }

        /// <inheritdoc />
        public float ValueSingle { get => Convert.ToSingle(_value); set => _value = Convert.ToDouble(value); }

        /// <inheritdoc />
        public double ValueDouble { get => _value; private set => _value = value; }

        /// <inheritdoc />
        public IRangeableInputDescription RangeInfo { get { return MouseRangeInfo.Instance; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="MouseMoveAxisInput"/> class.
        /// </summary>
        /// <param name="value">Value.</param>
        public MouseMoveAxisInput(double value)
        {
            _value = value;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return _value.ToString();
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
