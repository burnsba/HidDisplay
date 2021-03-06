﻿using System;
using System.Collections.Generic;

namespace HidDisplay.PluginDefinition
{
    /// <summary>
    /// Interface to desribe a plugin.
    /// </summary>
    public interface IPlugin : IDisposable
    {
        /// <summary>
        /// Fired when input is received.
        /// </summary>
        event EventHandler<GenericInputEventArgs> UpdateEvent;

        /// <summary>
        /// Gets or sets a value indicating whether the current state of the plugin.
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// Initialization to begin accepting or receiving events.
        /// </summary>
        /// <param name="configOptions">Configuration options.</param>
        void Setup(Dictionary<string, string> configOptions);

        /// <summary>
        /// Starts accepting or receiving events from input source. Events will then be translated.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops accepting or receiving events. Events will stop being translated.
        /// </summary>
        void Stop();
    }
}
