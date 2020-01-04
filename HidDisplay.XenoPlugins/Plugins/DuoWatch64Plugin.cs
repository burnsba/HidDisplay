using System;
using System.Collections.Generic;
using BurnsBac.WindowsHardware.SerialPort;
using HidDisplay.Controller.ControllerState.Nintendo64;
using HidDisplay.Controller.Readers;
using HidDisplay.PluginDefinition;

namespace HidDisplay.Controller.Plugins
{
    /// <summary>
    /// Plugin to provide input from duowatch64 arduino uno device.
    /// </summary>
    public class DuoWatch64Plugin : PluginBase, IPlugin, IActiveMonitorPlugin
    {
        private const string ConfigBaud = "DuoWatch64.Baudrate";
        private const string ConfigComPort = "DuoWatch64.ComPort";
        private const string ConfigPollInterval = "DuoWatch64.PollIntervalMs";

        private int _baudrate;
        private string _comPort;
        private DuoWatch64 _duoWatch64;
        private bool _isSetup = false;
        private int _pollInterval;
        private SerialPortProxy _serialPortProxy;

        /// <summary>
        /// Initializes a new instance of the <see cref="DuoWatch64Plugin"/> class.
        /// </summary>
        public DuoWatch64Plugin()
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

            if (!configOptions.ContainsKey(ConfigComPort))
            {
                throw new ArgumentException($"Missing required config setting: {ConfigComPort}");
            }

            if (!configOptions.ContainsKey(ConfigBaud))
            {
                throw new ArgumentException($"Missing required config setting: {ConfigBaud}");
            }

            if (!configOptions.ContainsKey(ConfigPollInterval))
            {
                throw new ArgumentException($"Missing required config setting: {ConfigPollInterval}");
            }

            _comPort = configOptions[ConfigComPort];
            _baudrate = int.Parse(configOptions[ConfigBaud]);
            _pollInterval = int.Parse(configOptions[ConfigPollInterval]);

            _serialPortProxy = new SerialPortProxy(_comPort, _baudrate, _pollInterval);
            _duoWatch64 = new DuoWatch64();
            _duoWatch64.Hook(_serialPortProxy);

            _duoWatch64.Nintendo64ControllerStateChange += InputEventMapper;

            _isSetup = true;
        }

        /// <inheritdoc />
        public override void Start()
        {
            IsEnabled = true;

            _serialPortProxy.Start();
        }

        /// <inheritdoc />
        public override void Stop()
        {
            IsEnabled = false;

            _isSetup = false;

            if (!object.ReferenceEquals(null, _serialPortProxy))
            {
                _serialPortProxy.Stop();
                _serialPortProxy = null;
            }

            _duoWatch64 = null;
        }

        private Button2State FromBool(bool b)
        {
            return b ? Button2State.Active : Button2State.Released;
        }

        /// <summary>
        /// Accepts events from the hardware watch and translates them to plugin event format.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="state">Event args.</param>
        private void InputEventMapper(object sender, Nintendo64ControllerState state)
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

            int portOffset = state.ControllerPort * 100;

            genArgs.Button2s.Add(new Button2()
            {
                Id = portOffset + Nintendo64Constants.Button_A_Id,
                Name = "Button_A",
                State = FromBool(state.A),
            });

            genArgs.Button2s.Add(new Button2()
            {
                Id = portOffset + Nintendo64Constants.Button_B_Id,
                Name = "Button_B",
                State = FromBool(state.B),
            });

            genArgs.Button2s.Add(new Button2()
            {
                Id = portOffset + Nintendo64Constants.Button_Z_Id,
                Name = "Button_Z",
                State = FromBool(state.Z),
            });

            genArgs.Button2s.Add(new Button2()
            {
                Id = portOffset + Nintendo64Constants.Button_Start_Id,
                Name = "Button_Start",
                State = FromBool(state.Start),
            });

            genArgs.Button2s.Add(new Button2()
            {
                Id = portOffset + Nintendo64Constants.Button_C_Up_Id,
                Name = "Button_C_Up",
                State = FromBool(state.C_Up),
            });

            genArgs.Button2s.Add(new Button2()
            {
                Id = portOffset + Nintendo64Constants.Button_C_Down_Id,
                Name = "Button_C_Down",
                State = FromBool(state.C_Down),
            });

            genArgs.Button2s.Add(new Button2()
            {
                Id = portOffset + Nintendo64Constants.Button_C_Left_Id,
                Name = "Button_C_Left",
                State = FromBool(state.C_Left),
            });

            genArgs.Button2s.Add(new Button2()
            {
                Id = portOffset + Nintendo64Constants.Button_C_Right_Id,
                Name = "Button_C_Right",
                State = FromBool(state.C_Right),
            });

            genArgs.Button2s.Add(new Button2()
            {
                Id = portOffset + Nintendo64Constants.Button_L_Shoulder_Id,
                Name = "Button_L_Shoulder",
                State = FromBool(state.L_Shoulder),
            });

            genArgs.Button2s.Add(new Button2()
            {
                Id = portOffset + Nintendo64Constants.Button_R_Shoulder_Id,
                Name = "Button_R_Shoulder",
                State = FromBool(state.R_Shoulder),
            });

            genArgs.Button2s.Add(new Button2()
            {
                Id = portOffset + Nintendo64Constants.Button_D_Up_Id,
                Name = "Button_D_Up",
                State = FromBool(state.D_Up),
            });

            genArgs.Button2s.Add(new Button2()
            {
                Id = portOffset + Nintendo64Constants.Button_D_Down_Id,
                Name = "Button_D_Down",
                State = FromBool(state.D_Down),
            });

            genArgs.Button2s.Add(new Button2()
            {
                Id = portOffset + Nintendo64Constants.Button_D_Left_Id,
                Name = "Button_D_Left",
                State = FromBool(state.D_Left),
            });

            genArgs.Button2s.Add(new Button2()
            {
                Id = portOffset + Nintendo64Constants.Button_D_Right_Id,
                Name = "Button_D_Right",
                State = FromBool(state.D_Right),
            });

            genArgs.RangeableInput2s.Add(new Nintendo64RangeableInput(state.AnalogX, state.AnalogY)
            {
                Id = state.ControllerPort,
                IsEmpty = false,
                Name = $"Analog_{state.ControllerPort}",
            });

            FireEventHandler(sender, genArgs);
        }
    }
}
