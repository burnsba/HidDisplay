using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace HidDisplay.PluginDefinition
{
    /// <summary>
    /// Button with three states.
    /// </summary>
    public class Button3 : IInputSource
    {
        /// <inheritdoc />
        public UInt64 EventSourceId
        {
            get
            {
                return (UInt64)((Id * 10) + (int)State);
            }
        }

        /// <inheritdoc />
        public int Id { get; set; }

        /// <inheritdoc />
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets current state of the button.
        /// </summary>
        public Button3State State { get; set; }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{Name}: {State}";
        }
    }
}
