using System;
using System.Collections.Generic;
using System.Text;

namespace HidDisplay.PluginDefinition
{
    /// <summary>
    /// Button with three states.
    /// </summary>
    public enum Button3State
    {
        /// <summary>
        /// Default/unknown.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Neutral.
        /// </summary>
        StateDefault = 1,

        /// <summary>
        /// Active/scroll up.
        /// </summary>
        State2 = 2,

        /// <summary>
        /// Released/scroll down.
        /// </summary>
        State3 = 3,
    }
}
