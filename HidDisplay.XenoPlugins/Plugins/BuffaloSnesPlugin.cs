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
    /// Plugin to provide input from buffalo generic "classic gaming" controller.
    /// </summary>
    public class BuffaloSnesPlugin : PluginBase, IPlugin, IPassiveTranslatePlugin, IPassiveTranslate<HidResult>
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

            // really simple sanity check on HID product description
            if (message.HidDeviceInfo.GetProduct().IndexOf("8-button", StringComparison.OrdinalIgnoreCase) == -1)
            {
                return;
            }

            var ux = message.UsageValues.First(x => x.UsagePage == HidUsagePages.GenericDesktop && x.Usage == (uint)BurnsBac.WinApi.Hid.Usage.GenericDesktop.X);
            var uy = message.UsageValues.First(x => x.UsagePage == HidUsagePages.GenericDesktop && x.Usage == (uint)BurnsBac.WinApi.Hid.Usage.GenericDesktop.Y);

            var genArgs = new GenericInputEventArgs();
            int controllerPort = 1;

            int portOffset = controllerPort * 100;

            genArgs.Button2s.Add(new Button2()
            {
                Id = portOffset + SnesConstants.Button_A_Id,
                Name = "Button_A",
                State = FromBool(message.ButtonStates[0]),
            });

            genArgs.Button2s.Add(new Button2()
            {
                Id = portOffset + SnesConstants.Button_B_Id,
                Name = "Button_B",
                State = FromBool(message.ButtonStates[1]),
            });

            genArgs.Button2s.Add(new Button2()
            {
                Id = portOffset + SnesConstants.Button_X_Id,
                Name = "Button_X",
                State = FromBool(message.ButtonStates[2]),
            });

            genArgs.Button2s.Add(new Button2()
            {
                Id = portOffset + SnesConstants.Button_Y_Id,
                Name = "Button_Y",
                State = FromBool(message.ButtonStates[3]),
            });

            genArgs.Button2s.Add(new Button2()
            {
                Id = portOffset + SnesConstants.Button_L_Shoulder_Id,
                Name = "Button_L_Shoulder",
                State = FromBool(message.ButtonStates[4]),
            });

            genArgs.Button2s.Add(new Button2()
            {
                Id = portOffset + SnesConstants.Button_R_Shoulder_Id,
                Name = "Button_R_Shoulder",
                State = FromBool(message.ButtonStates[5]),
            });

            genArgs.Button2s.Add(new Button2()
            {
                Id = portOffset + SnesConstants.Button_Select_Id,
                Name = "Button_Select",
                State = FromBool(message.ButtonStates[6]),
            });

            genArgs.Button2s.Add(new Button2()
            {
                Id = portOffset + SnesConstants.Button_Start_Id,
                Name = "Button_Start",
                State = FromBool(message.ButtonStates[7]),
            });

            genArgs.Button2s.Add(new Button2()
            {
                Id = portOffset + SnesConstants.Button_D_Up_Id,
                Name = "Button_D_Up",
                State = DupFromRange(ux, uy),
            });

            genArgs.Button2s.Add(new Button2()
            {
                Id = portOffset + SnesConstants.Button_D_Down_Id,
                Name = "Button_D_Down",
                State = DdownFromRange(ux, uy),
            });

            genArgs.Button2s.Add(new Button2()
            {
                Id = portOffset + SnesConstants.Button_D_Left_Id,
                Name = "Button_D_Left",
                State = DleftFromRange(ux, uy),
            });

            genArgs.Button2s.Add(new Button2()
            {
                Id = portOffset + SnesConstants.Button_D_Right_Id,
                Name = "Button_D_Right",
                State = DrightFromRange(ux, uy),
            });

            FireEventHandler(sender, genArgs);
        }

        private Button2State FromBool(bool b)
        {
            return b ? Button2State.Active : Button2State.Released;
        }

        private Button2State DupFromRange(HidResult.UsagePageUsageValue ux, HidResult.UsagePageUsageValue uy)
        {
            return uy.Value == 0 ? Button2State.Active : Button2State.Released;
        }

        private Button2State DdownFromRange(HidResult.UsagePageUsageValue ux, HidResult.UsagePageUsageValue uy)
        {
            return uy.Value == 255 ? Button2State.Active : Button2State.Released;
        }

        private Button2State DleftFromRange(HidResult.UsagePageUsageValue ux, HidResult.UsagePageUsageValue uy)
        {
            return ux.Value == 0 ? Button2State.Active : Button2State.Released;
        }

        private Button2State DrightFromRange(HidResult.UsagePageUsageValue ux, HidResult.UsagePageUsageValue uy)
        {
            return ux.Value == 255 ? Button2State.Active : Button2State.Released;
        }
    }
}
