using System;
using System.Collections.Generic;
using System.Text;

namespace HidDisplay.PluginDefinition
{
    /// <summary>
    /// Base class for plugins.
    /// </summary>
    public abstract class PluginBase : IDisposable, IPlugin
    {
        /// <inheritdoc />
        public event EventHandler<GenericInputEventArgs> UpdateEvent;

        /// <inheritdoc />
        public bool IsEnabled { get; set; } = false;

        /// <inheritdoc />
        public void Dispose()
        {
            UpdateEvent = null;

            InstanceDispose();
        }

        /// <inheritdoc />
        public abstract void Start();

        /// <inheritdoc />
        public abstract void Stop();

        /// <inheritdoc />
        public abstract void Setup(Dictionary<string, string> configOptions);

        /// <summary>
        /// Dispose for concrete implementation.
        /// </summary>
        /// <remarks>
        /// Want to force dispose being called on this abstract class, but also
        /// any concrete implementations.
        /// </remarks>
        public abstract void InstanceDispose();

        /// <summary>
        /// Gets a value indicating whether or not there are any attached event listeners.
        /// </summary>
        /// <returns>True/false.</returns>
        protected bool AnyEventListeners()
        {
            return !(UpdateEvent == null);
        }

        /// <summary>
        /// Forwards arguments to attached event listeners. Ignores <see cref="PluginBase.IsEnabled"/>.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="args">Args.</param>
        protected void FireEventHandler(object sender, GenericInputEventArgs args)
        {
            if (AnyEventListeners())
            {
                UpdateEvent(sender, args);
            }
        }
    }
}
