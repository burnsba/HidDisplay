using System;
using System.Collections.Generic;
using System.Text;
using HidDisplay.PluginDefinition;

namespace HidDisplay.Controller.ControllerState.Nintendo64
{
    /// <summary>
    /// Mouse movement translator for single axis value.
    /// </summary>
    public sealed class Nintendo64AxisInput : IRangeableInput
    {
        private short _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Nintendo64AxisInput"/> class.
        /// </summary>
        /// <param name="value">Value.</param>
        public Nintendo64AxisInput(short value)
        {
            _value = value;
        }

        /// <inheritdoc />
        public UInt64 EventSourceId
        {
            get
            {
                return (UInt64)Id;
            }
        }

        /// <inheritdoc />
        public int Id { get; set; }

        /// <inheritdoc />
        public bool IsEmpty { get; set; } = false;

        /// <inheritdoc />
        public string Name { get; set; }

        /// <inheritdoc />
        public IRangeableInputDescription RangeInfo { get { return Nintendo64RangeInfo.Instance; } }

        /// <inheritdoc />
        public object Value { get => _value; private set => _value = (short)value; }

        /// <inheritdoc />
        public decimal ValueDecimal { get => Convert.ToDecimal(_value); set => _value = Convert.ToInt16(value); }

        /// <inheritdoc />
        public double ValueDouble { get => Convert.ToDouble(_value); private set => _value = Convert.ToInt16(value); }

        /// <inheritdoc />
        public int ValueInt { get => _value; set => _value = (short)value; }

        /// <inheritdoc />
        public float ValueSingle { get => Convert.ToSingle(_value); set => _value = Convert.ToInt16(value); }

        /// <inheritdoc />
        public override string ToString()
        {
            return _value.ToString();
        }
    }
}
