using System;
using System.Collections.Generic;
using System.Text;

namespace HidDisplay.SkinModel.InputSourceDescription
{
    /// <summary>
    /// Interface for how to match/process incoming hardware events from <see cref="HidDisplay.PluginDefinition.GenericInputEventArgs" />.
    /// </summary>
    public interface IInputSourceDescription
    {
        /// <summary>
        /// Gets or sets id of event originator.
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// Gets or sets name of event to match/process.
        /// </summary>
        string Name { get; set; }
    }
}
