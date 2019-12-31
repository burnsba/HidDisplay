using System;
using System.Collections.Generic;
using System.Text;
using HidDisplay.PluginDefinition;


namespace HidDisplay.XenoPlugins.SensorRange
{
    /// <summary>
    /// Generic rangeable input.
    /// </summary>
    /// <remarks>
    public class GenericIntRangeableInput : IRangeableInput
    {
        private int _value;

        /// <inheritdoc />
        public int Id { get; set; }

        /// <inheritdoc />
        public string Name { get; set; }

        /// <inheritdoc />
        public bool IsEmpty { get; set; } = false;

        /// <inheritdoc />
        public object Value { get => _value; private set => _value = (int)value; }

        /// <inheritdoc />
        public int ValueInt { get => _value; set => _value = value; }

        /// <inheritdoc />
        public decimal ValueDecimal { get => Convert.ToDecimal(_value); set => _value = Convert.ToInt32(value); }

        /// <inheritdoc />
        public float ValueSingle { get => Convert.ToSingle(_value); set => _value = Convert.ToInt32(value); }

        /// <inheritdoc />
        public double ValueDouble { get => Convert.ToDouble(_value); private set => _value = Convert.ToInt32(value); }

        /// <inheritdoc />
        public IRangeableInputDescription RangeInfo { get { return GenericIntRangeInfo.Instance; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericIntRangeableInput"/> class.
        /// </summary>
        /// <param name="value">Value.</param>
        public GenericIntRangeableInput(int value)
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
