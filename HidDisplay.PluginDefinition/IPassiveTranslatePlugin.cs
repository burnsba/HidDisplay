using System;
using System.Collections.Generic;
using System.Text;

namespace HidDisplay.PluginDefinition
{
    /// <summary>
    /// A plugin that manages it's own resources, but does not initiate update events on
    /// it's own. The main application will have to pass some kind of data to the plugin,
    /// which will then parse and evaluate.
    /// </summary>
    public interface IPassiveTranslatePlugin
    {
    }
}
