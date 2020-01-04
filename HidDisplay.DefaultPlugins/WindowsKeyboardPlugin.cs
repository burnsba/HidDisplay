using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using BurnsBac.WinApi.User32;
using BurnsBac.WinApi.Windows;
using BurnsBac.WindowsHardware.HardwareWatch;
using BurnsBac.WindowsHardware.HardwareWatch.Enums;
using HidDisplay.PluginDefinition;

namespace HidDisplay.DefaultPlugins
{
    /// <summary>
    /// Plugin wrapper to listen for keyboard events.
    /// </summary>
    public class WindowsKeyboardPlugin : PluginBase, IPlugin, IActiveMonitorPlugin
    {
        // via settings.json
        private const string ConfigWatchWindowTitleKey = "Keyboard.WatchWindowTitle";
        private const string ConfigWindowTitleMatch = "Keyboard.WindowTitleMatch";

        private bool _isSetup = false;
        private KeyboardWatcher _keyboardWatcher;
        private WindowTitleMatch _titleMatch;
        private string _windowTitle;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowsKeyboardPlugin"/> class.
        /// </summary>
        public WindowsKeyboardPlugin()
        {
        }

        /// <inheritdoc />
        public override void InstanceDispose()
        {
            Stop();
        }

        /// <inheritdoc />
        public override void Setup(Dictionary<string, string> configOptions)
        {
            if (_isSetup)
            {
                return;
            }

            if (!configOptions.ContainsKey(ConfigWatchWindowTitleKey))
            {
                throw new ArgumentException($"Missing required config setting: {ConfigWatchWindowTitleKey}");
            }

            if (!configOptions.ContainsKey(ConfigWindowTitleMatch))
            {
                throw new ArgumentException($"Missing required config setting: {ConfigWindowTitleMatch}");
            }

            _windowTitle = configOptions[ConfigWatchWindowTitleKey];
            _titleMatch = (WindowTitleMatch)Enum.Parse(typeof(WindowTitleMatch), configOptions[ConfigWindowTitleMatch], true);

            if (string.IsNullOrEmpty(_windowTitle))
            {
                throw new ArgumentException("Window title must be set.");
            }

            if (_titleMatch == WindowTitleMatch.Unknown)
            {
                throw new ArgumentException("WindowTitleMatch must be set.");
            }

            _keyboardWatcher = new KeyboardWatcher();
            _keyboardWatcher.KeyboardChangeEvent += InputEventMapper;

            // Calling setup will start monitoring keyboard events, so delaying that until now.
            _keyboardWatcher.Setup(_titleMatch, _windowTitle);

            _isSetup = true;
        }

        /// <inheritdoc />
        public override void Start()
        {
            IsEnabled = true;
        }

        /// <inheritdoc />
        public override void Stop()
        {
            IsEnabled = false;

            _isSetup = false;

            if (!object.ReferenceEquals(null, _keyboardWatcher))
            {
                _keyboardWatcher.Dispose();
                _keyboardWatcher = null;
            }
        }

        /// <summary>
        /// Translate button state from hardware watch to plugin event format.
        /// </summary>
        /// <param name="direction">Hardware button state.</param>
        /// <returns>Plugin button state.</returns>
        private Button2State FromKeys(KeyChangeDirection direction)
        {
            switch (direction)
            {
                case KeyChangeDirection.KeyDown:
                    return Button2State.Active;
                case KeyChangeDirection.KeyUp:
                    return Button2State.Released;
                default:
                    return Button2State.Unknown;
            }
        }

        /// <summary>
        /// Accepts events from the hardware watch and translates them to plugin event format.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="args">Event args.</param>
        private void InputEventMapper(object sender, KeyboardChangeEventArgs args)
        {
            if (!IsEnabled)
            {
                return;
            }

            if (!AnyEventListeners())
            {
                return;
            }

            var genArgs = new GenericInputEventArgs();
            genArgs.Button2s.Add(new Button2()
            {
                Id = (int)args.Key,
                Name = args.Key.ToString(),
                State = FromKeys(args.Direction),
            });

            FireEventHandler(sender, genArgs);
        }
    }
}
