using System;
using System.Collections.Generic;
using System.Text;

namespace HidDisplay.PluginDefinition
{
    /// <summary>
    /// Button with two states.
    /// </summary>
    public enum Button2State
    {
        /// <summary>
        /// Default/unknown.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Active, pressed, down.
        /// </summary>
        Active = 1,

        /// <summary>
        /// Inactive, released, up.
        /// </summary>
        Released = 2,
    }
}
