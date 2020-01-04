using System;
using System.Collections.Generic;
using System.Linq;
using BurnsBac.WinApi.Hid;
using BurnsBac.WindowsHardware.HardwareWatch;
using HidDisplay.Controller.ControllerState.Nintendo64;
using HidDisplay.PluginDefinition;

namespace HidDisplay.Controller.Plugins
{
    /// <summary>
    /// Plugin to provice input from raphnet n64 to usb device.
    /// </summary>
    public class Raphnet64Plugin : PluginBase, IPlugin, IPassiveTranslatePlugin, IPassiveTranslate<HidResult>
    {
        private bool _isSetup = false;

        /// <inheritdoc />
        public override void InstanceDispose()
        {
            IsEnabled = false;

            _isSetup = false;
        }

        /// <inheritdoc />
        public override void Setup(Dictionary<string, string> configOptions)
        {
            if (_isSetup)
            {
                return;
            }

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
        }

        /// <inheritdoc />
        public void AcceptMessage(object sender, HidResult message)
        {
            if (!IsEnabled)
            {
                return;
            }

            if (!AnyEventListeners())
            {
                return;
            }

            // really simple sanity check on HID input manufacturer
            if (message.HidDeviceInfo.GetManufacturer().IndexOf("raphnet", StringComparison.OrdinalIgnoreCase) == -1)
            {
                return;
            }

            // really simple sanity check on HID product description
            if (message.HidDeviceInfo.GetProduct().IndexOf("n64", StringComparison.OrdinalIgnoreCase) == -1)
            {
                return;
            }

            var ux = message.UsageValues.First(x => x.UsagePage == HidUsagePages.GenericDesktop && x.Usage == (uint)BurnsBac.WinApi.Hid.Usage.GenericDesktop.X);
            var uy = message.UsageValues.First(x => x.UsagePage == HidUsagePages.GenericDesktop && x.Usage == (uint)BurnsBac.WinApi.Hid.Usage.GenericDesktop.Y);

            short analogx = (short)((((Int64)ux.Value * (Int64)256) / (Int64)32000) - 128);

            // y is inverted
            short analogy = (short)(-1 * ((((Int64)uy.Value * (Int64)256) / (Int64)32000) - 128));

            var genArgs = new GenericInputEventArgs();
            int controllerPort = 1;

            int portOffset = controllerPort * 100;

            genArgs.Button2s.Add(new Button2()
            {
                Id = portOffset + Nintendo64Constants.Button_A_Id,
                Name = "Button_A",
                State = FromBool(message.ButtonStates[0]),
            });

            genArgs.Button2s.Add(new Button2()
            {
                Id = portOffset + Nintendo64Constants.Button_B_Id,
                Name = "Button_B",
                State = FromBool(message.ButtonStates[1]),
            });

            genArgs.Button2s.Add(new Button2()
            {
                Id = portOffset + Nintendo64Constants.Button_Z_Id,
                Name = "Button_Z",
                State = FromBool(message.ButtonStates[2]),
            });

            genArgs.Button2s.Add(new Button2()
            {
                Id = portOffset + Nintendo64Constants.Button_Start_Id,
                Name = "Button_Start",
                State = FromBool(message.ButtonStates[3]),
            });

            genArgs.Button2s.Add(new Button2()
            {
                Id = portOffset + Nintendo64Constants.Button_C_Up_Id,
                Name = "Button_C_Up",
                State = FromBool(message.ButtonStates[6]),
            });

            genArgs.Button2s.Add(new Button2()
            {
                Id = portOffset + Nintendo64Constants.Button_C_Down_Id,
                Name = "Button_C_Down",
                State = FromBool(message.ButtonStates[7]),
            });

            genArgs.Button2s.Add(new Button2()
            {
                Id = portOffset + Nintendo64Constants.Button_C_Left_Id,
                Name = "Button_C_Left",
                State = FromBool(message.ButtonStates[8]),
            });

            genArgs.Button2s.Add(new Button2()
            {
                Id = portOffset + Nintendo64Constants.Button_C_Right_Id,
                Name = "Button_C_Right",
                State = FromBool(message.ButtonStates[9]),
            });

            genArgs.Button2s.Add(new Button2()
            {
                Id = portOffset + Nintendo64Constants.Button_L_Shoulder_Id,
                Name = "Button_L_Shoulder",
                State = FromBool(message.ButtonStates[4]),
            });

            genArgs.Button2s.Add(new Button2()
            {
                Id = portOffset + Nintendo64Constants.Button_R_Shoulder_Id,
                Name = "Button_R_Shoulder",
                State = FromBool(message.ButtonStates[5]),
            });

            genArgs.Button2s.Add(new Button2()
            {
                Id = portOffset + Nintendo64Constants.Button_D_Up_Id,
                Name = "Button_D_Up",
                State = FromBool(message.ButtonStates[10]),
            });

            genArgs.Button2s.Add(new Button2()
            {
                Id = portOffset + Nintendo64Constants.Button_D_Down_Id,
                Name = "Button_D_Down",
                State = FromBool(message.ButtonStates[11]),
            });

            genArgs.Button2s.Add(new Button2()
            {
                Id = portOffset + Nintendo64Constants.Button_D_Left_Id,
                Name = "Button_D_Left",
                State = FromBool(message.ButtonStates[12]),
            });

            genArgs.Button2s.Add(new Button2()
            {
                Id = portOffset + Nintendo64Constants.Button_D_Right_Id,
                Name = "Button_D_Right",
                State = FromBool(message.ButtonStates[13]),
            });

            genArgs.RangeableInput2s.Add(new Nintendo64RangeableInput(analogx, analogy)
            {
                Id = controllerPort,
                IsEmpty = false,
                Name = $"Analog_{controllerPort}",
            });

            FireEventHandler(sender, genArgs);
        }

        private Button2State FromBool(bool b)
        {
            return b ? Button2State.Active : Button2State.Released;
        }
    }
}
