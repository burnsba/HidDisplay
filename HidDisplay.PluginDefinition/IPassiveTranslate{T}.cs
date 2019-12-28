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
    public interface IPassiveTranslate<in T>
    {
        /// <summary>
        /// Accepts message from main application. Will parse and emit <see cref="GenericInputEventArgs"/>
        /// if required.
        /// </summary>
        /// <param name="sender">Sender of message.</param>
        /// <param name="message">Incoming data.</param>
        void AcceptMessage(object sender, T message);
    }
}
