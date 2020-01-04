using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BurnsBac.WindowsHardware.Bluetooth.Sensors;
using BurnsBac.WindowsHardware.HardwareWatch;
using HidDisplay.Controller.ControllerState;
using HidDisplay.Controller.ControllerState.Nintendo64;
using HidDisplay.PluginDefinition;
using HidDisplay.XenoPlugins.SensorRange;

namespace HidDisplay.XenoPlugins.Plugins
{
    /// <summary>
    /// Plugin to provide input from bluetooth low energy heartrate device.
    /// </summary>
    public class BluetoothLowEnergyHeartratePlugin : PluginBase, IPlugin, IActiveMonitorPlugin
    {
        private const string BluetoothAddress = "BluetoothHeartRate.DeviceAddress";

        LowEnergyHeartrateSensor _sensor = null;
        private ulong _bluetoothAddress;
        private bool _isSetup = false;

        /// <inheritdoc />
        public override void InstanceDispose()
        {
            _sensor.Dispose();
            _sensor = null;
        }

        /// <inheritdoc />
        public override void Setup(Dictionary<string, string> configOptions)
        {
            if (_isSetup)
            {
                return;
            }

            if (!configOptions.ContainsKey(BluetoothAddress))
            {
                throw new ArgumentException($"Missing required config setting: {BluetoothAddress}");
            }

            _bluetoothAddress = ulong.Parse(configOptions[BluetoothAddress]);

            _sensor = new LowEnergyHeartrateSensor(_bluetoothAddress);
            _sensor.HeartRateReceivedEvent += InputEventMapper;

            _isSetup = true;
        }

        /// <inheritdoc />
        public override async void Start()
        {
            System.Diagnostics.Debug.WriteLine($"Starting {nameof(BluetoothLowEnergyHeartratePlugin)}");

            await _sensor.FindCharacteristic();
            await _sensor.EnableNotifications();

            IsEnabled = true;
        }

        /// <inheritdoc />
        public override async void Stop()
        {
            System.Diagnostics.Debug.WriteLine($"Stopping {nameof(BluetoothLowEnergyHeartratePlugin)}");

            await _sensor.DisableNotifications();

            IsEnabled = false;
        }

        /// <summary>
        /// Accepts <see cref="WindowsHardware.Bluetooth.Characteristics.HeartRateMeasurement"/> and translates to <see cref="GenericInputEventArgs"/>.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="state">Event args.</param>
        private void InputEventMapper(object sender, BurnsBac.WindowsHardware.Bluetooth.Characteristics.HeartRateMeasurement state)
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

            genArgs.RangeableInputs.Add(new GenericIntRangeableInput(state.HeartRate) { Id = 1 });

            FireEventHandler(sender, genArgs);
        }
    }
}
