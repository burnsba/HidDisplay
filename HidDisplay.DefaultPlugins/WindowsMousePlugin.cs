using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Timers;
using BurnsBac.WinApi.User32;
using BurnsBac.WinApi.Windows;
using BurnsBac.WindowsHardware.HardwareWatch;
using BurnsBac.WindowsHardware.HardwareWatch.Enums;
using HidDisplay.PluginDefinition;

namespace HidDisplay.DefaultPlugins
{
    /// <summary>
    /// Plugin wrapper to listen for mouse events.
    /// </summary>
    /// <remarks>
    /// Instead of immediately forwarding every mousemovement, this accumulates events
    /// for a set period of time the forwards the sum of the movements.
    /// </remarks>
    public class WindowsMousePlugin : PluginBase, IPlugin, IActiveMonitorPlugin
    {
        private const string ConfigMoveUpdateIntervalMs = "Mouse.MoveUpdateIntervalMs";

        // via settings.json
        private const string ConfigWatchWindowTitleKey = "Mouse.WatchWindowTitle";
        private const string ConfigWindowTitleMatch = "Mouse.WindowTitleMatch";
        private bool _isSetup = false;
        private ConcurrentQueue<WindowsPoint> _mouseMoveAccumulator = new ConcurrentQueue<WindowsPoint>();
        private Timer _mouseMoveSmoothTimer;
        private MouseWatcher _mouseWatcher;
        private WindowTitleMatch _titleMatch;
        private string _windowTitle;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowsMousePlugin"/> class.
        /// </summary>
        public WindowsMousePlugin()
        {
        }

        /// <summary>
        /// Gets or sets how long to accumulate move events before updating the ui.
        /// </summary>
        public int MouseMoveUpdateIntervalMs { get; set; } = -1;

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

            if (!configOptions.ContainsKey(ConfigMoveUpdateIntervalMs))
            {
                throw new ArgumentException($"Missing required config setting: {ConfigMoveUpdateIntervalMs}");
            }

            _windowTitle = configOptions[ConfigWatchWindowTitleKey];
            _titleMatch = (WindowTitleMatch)Enum.Parse(typeof(WindowTitleMatch), configOptions[ConfigWindowTitleMatch], true);
            MouseMoveUpdateIntervalMs = int.Parse(configOptions[ConfigMoveUpdateIntervalMs]);

            if (string.IsNullOrEmpty(_windowTitle))
            {
                throw new ArgumentException("Window title must be set.");
            }

            if (_titleMatch == WindowTitleMatch.Unknown)
            {
                throw new ArgumentException("WindowTitleMatch must be set.");
            }

            if (MouseMoveUpdateIntervalMs <= 0)
            {
                throw new ArgumentException($"{MouseMoveUpdateIntervalMs} must be positive integer.");
            }

            _mouseWatcher = new MouseWatcher();
            _mouseWatcher.MouseChangeEvent += InputEventMapper;

            // Calling setup will start monitoring keyboard events, so delaying that until now.
            _mouseWatcher.Setup(_titleMatch, _windowTitle);

            _mouseMoveSmoothTimer = new Timer();
            _mouseMoveSmoothTimer.AutoReset = true;
            _mouseMoveSmoothTimer.Interval = MouseMoveUpdateIntervalMs;
            _mouseMoveSmoothTimer.Elapsed += MouseMoveSmootherElapsed;
            _mouseMoveSmoothTimer.Start();

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

            if (!object.ReferenceEquals(null, _mouseWatcher))
            {
                _mouseWatcher.Dispose();
                _mouseWatcher = null;
            }

            if (!object.ReferenceEquals(null, _mouseMoveSmoothTimer))
            {
                _mouseMoveSmoothTimer.Stop();
                _mouseMoveSmoothTimer = null;
            }

            WindowsPoint p;
            while (!_mouseMoveAccumulator.IsEmpty)
            {
                _mouseMoveAccumulator.TryDequeue(out p);
            }
        }

        /// <summary>
        /// Translate button state from hardware watch to plugin event format.
        /// </summary>
        /// <param name="direction">Hardware button state.</param>
        /// <returns>Plugin button state.</returns>
        private Button2State FromMouseButton(ButtonChangeDirection direction)
        {
            switch (direction)
            {
                case ButtonChangeDirection.Down:
                    return Button2State.Active;
                case ButtonChangeDirection.Up:
                    return Button2State.Released;
                default:
                    return Button2State.Unknown;
            }
        }

        /// <summary>
        /// Translate scroll state from hardware watch to plugin event format.
        /// </summary>
        /// <param name="direction">Hardware scroll state.</param>
        /// <returns>Plugin button state.</returns>
        private Button3State FromMouseScroll(ScrollChangeDirection direction)
        {
            switch (direction)
            {
                case ScrollChangeDirection.Up:
                    return Button3State.State2;
                case ScrollChangeDirection.Down:
                    return Button3State.State3;
                default:
                    return Button3State.StateDefault;
            }
        }

        /// <summary>
        /// Accepts hardware mouse events. Button and scroll events are forwarded
        /// immediately. Movement events are added to the accumulator to be
        /// forwarded later.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="args">Event args.</param>
        private void InputEventMapper(object sender, MouseChangeEventArgs args)
        {
            if (!IsEnabled)
            {
                return;
            }

            if (!AnyEventListeners())
            {
                return;
            }

            // Split immediate and accumulator inputs.
            bool anyImmediate = false;

            var genArgs = new GenericInputEventArgs();

            if (args.EventType == MouseEventType.Button)
            {
                genArgs.Button2s.Add(new Button2()
                {
                    Id = (int)args.Button,
                    Name = args.Button.ToString(),
                    State = FromMouseButton(args.ButtonDirection),
                });
                anyImmediate = true;
            }
            else if (args.EventType == MouseEventType.Scroll)
            {
                genArgs.Button3s.Add(new Button3()
                {
                    Id = (int)args.Scroll,
                    Name = args.Scroll.ToString(),
                    State = FromMouseScroll(args.ScrollDirection),
                });
                anyImmediate = true;
            }
            else if (args.EventType == MouseEventType.Move)
            {
                _mouseMoveAccumulator.Enqueue(new WindowsPoint(args.DeltaPosition.X, args.DeltaPosition.Y));
            }

            if (anyImmediate)
            {
                FireEventHandler(sender, genArgs);
            }
        }

        /// <summary>
        /// Forwards sum of movements to app for display.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="args">Args.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1503:Braces should not be omitted", Justification = "SpinWait")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1106:Code should not contain empty statements", Justification = "SpinWait")]
        private void MouseMoveSmootherElapsed(object sender, EventArgs args)
        {
            Int64 xtotal = 0;
            Int64 ytotal = 0;
            int count = 0;
            var genArgs = new GenericInputEventArgs();

            while (!_mouseMoveAccumulator.IsEmpty)
            {
                WindowsPoint p;
                while (!_mouseMoveAccumulator.TryDequeue(out p))
                    ;

                // Shouldn't receive a move event that doesn't move ...
                if (p.X == 0 && p.Y == 0)
                {
                    continue;
                }

                xtotal += p.X;
                ytotal += p.Y;
                count++;
            }

            if (count == 0)
            {
                // Need something to trigger the processing logic
                genArgs.RangeableInput2s.Add(new MouseMoveRangeableInput(double.NaN, double.NaN)
                {
                    Id = 1,
                    IsEmpty = true,
                    Name = "MouseMove",
                });

                FireEventHandler(sender, genArgs);
                return;
            }

            double xaverage = xtotal / count;
            double yaverage = ytotal / count;

            genArgs.RangeableInput2s.Add(new MouseMoveRangeableInput(xaverage, yaverage)
            {
                Id = 1,
                IsEmpty = false,
                Name = "MouseMove",
            });

            FireEventHandler(sender, genArgs);
        }
    }
}
