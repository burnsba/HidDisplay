using System;
using System.Collections.Generic;
using System.Text;

namespace HidDisplay.PluginDefinition
{
    /// <summary>
    /// Input source interface.
    /// </summary>
    public interface IInputSource
    {
        /// <summary>
        /// Gets id of input source.
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Gets name of input source.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets unique sourceid. This should resolve based on item performing the action, such as 
        /// left mouse button click event, or mouse wheel scroll up event. This is used to
        /// make reset timers, which is why just identifying "mouse wheel vertical scroll"
        /// is not enough (need direction as well).
        /// </summary>
        UInt64 EventSourceId { get; }
    }
}
