using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Storage.Streams;
using WindowsHardware.Bluetooth.Error;

namespace WindowsHardware.Bluetooth
{
    /// <summary>
    /// Base class to interact with bluetooth sensor devices.
    /// </summary>
    /// <remarks>
    /// https://blogs.msdn.microsoft.com/cdndevs/2017/04/28/uwp-working-with-bluetooth-devices-part-1/
    /// </remarks>
    public abstract class BluetoothSensorBase : IDisposable
    {
        private int _setupState = 0;
        private GattCharacteristic _dataCharacteristic = null;
        private bool _notificationsIsEnabled = false;

        private ulong _deviceAddress;
        private ushort _serviceAssignedNumber;
        private ushort _characteristicAssignedNumber;

        /// <summary>
        /// Event notification for data received from device.
        /// </summary>
        public event EventHandler<SesnsorReadEventArgs> DataReceivedEvent;

        /// <summary>
        /// Event delegate to notify state change events.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="state">Device event data.</param>
        protected void OnDataReceived(SesnsorReadEventArgs args)
        {
            DataReceivedEvent?.Invoke(this, args);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BluetoothSensorBase"/> class.
        /// </summary>
        /// <param name="deviceAddress">Bluetooth device address.</param>
        /// <param name="characteristicAssignedNumber">Assigned number of the chactertistic to be read from device.</param>
        public BluetoothSensorBase(ulong deviceAddress, ushort characteristicAssignedNumber)
        {
            _deviceAddress = deviceAddress;
            _serviceAssignedNumber = 0;
            _characteristicAssignedNumber = characteristicAssignedNumber;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BluetoothSensorBase"/> class.
        /// </summary>
        /// <param name="deviceAddress">Bluetooth device address.</param>
        /// <param name="serviceAssignedNumber">Assigned number of the parent service of the characteristic to be read from device.</param>
        /// <param name="characteristicAssignedNumber">Assigned number of the chactertistic to be read from device.</param>
        public BluetoothSensorBase(ulong deviceAddress, ushort serviceAssignedNumber, ushort characteristicAssignedNumber)
        {
            _deviceAddress = deviceAddress;
            _serviceAssignedNumber = serviceAssignedNumber;
            _characteristicAssignedNumber = characteristicAssignedNumber;
        }

        /// <summary>
        /// Finds the characteristic on the bluetooth device. If the characteristic was previously found,
        /// nothing happens.
        /// </summary>
        /// <returns>Connection status. 0 is unitialized, 1 is connecting, 2 is connected.</returns>
        public async Task<int> FindCharacteristic()
        {
            if (_setupState == 0)
            {
                await SetupCharacteristicHook();
            }

            return _setupState;
        }

        /// <summary>
        /// Tries to find the desired characteristic on the device.
        /// </summary>
        /// <returns>Void.</returns>
        protected async Task SetupCharacteristicHook()
        {
            _setupState = 1;
            var device = await BluetoothLEDevice.FromBluetoothAddressAsync(_deviceAddress);
            var services = await device.GetGattServicesAsync();

            GattCharacteristic foundCharacteristic = null;

            if (_serviceAssignedNumber == 0)
            {
                foreach (var service in services.Services)
                {
                    System.Diagnostics.Debug.WriteLine($"Service: {service.Uuid}");

                    foundCharacteristic = await FindCharacteristicInService(service, _characteristicAssignedNumber);
                    if (!object.ReferenceEquals(null, foundCharacteristic))
                    {
                        _serviceAssignedNumber = Bluetooth.Utility.UuidToAssignedNumber(service.Uuid);
                        break;
                    }
                }
            }
            else
            {
                var service = services.Services.Where(x => Utility.UuidToAssignedNumber(x.Uuid) == _serviceAssignedNumber).FirstOrDefault();

                if (object.ReferenceEquals(null, service))
                {
                    throw new Bluetooth.Error.ServiceNotFoundException($"Could not find service with assigned number 0x{_serviceAssignedNumber.ToString("X2")}");
                }

                foundCharacteristic = await FindCharacteristicInService(service, _characteristicAssignedNumber);
            }

            if (object.ReferenceEquals(null, foundCharacteristic))
            {
                throw new Bluetooth.Error.CharacteristicNotFoundException($"Could not find characteristic with assigned number 0x{_characteristicAssignedNumber.ToString("X2")}");
            }

            _dataCharacteristic = foundCharacteristic;
            _setupState = 2;
        }

        /// <summary>
        /// Helper function, enumerates a service searching for a specific characteristic.
        /// </summary>
        /// <param name="service">Service to enumerate.</param>
        /// <param name="characteristicAssignedNumber">Characteristic to find.</param>
        /// <returns><see cref="GattCharacteristic"/> of found characteristic, or null.</returns>
        private async Task<GattCharacteristic> FindCharacteristicInService(GattDeviceService service, ushort characteristicAssignedNumber)
        {
            var characteristics = await service.GetCharacteristicsAsync();
            foreach (var characteristic in characteristics.Characteristics)
            {
                var can = Bluetooth.Utility.UuidToAssignedNumber(characteristic.Uuid);
                if (can == characteristicAssignedNumber)
                {
                    return characteristic;
                }
            }

            return null;
        }

        /// <summary>
        /// Enables active notifications from the device.
        /// </summary>
        /// <returns>Status from trying to change ConfigurationDescriptor on device.</returns>
        public virtual async Task<GattCommunicationStatus> EnableNotifications()
        {
            if (_setupState != 2)
            {
                throw new InvalidStateException("Data characteristic is not connected.");
            }

            if (_notificationsIsEnabled)
            {
                return GattCommunicationStatus.Success;
            }

            GattCommunicationStatus status =
                await _dataCharacteristic.WriteClientCharacteristicConfigurationDescriptorAsync(
                    GattClientCharacteristicConfigurationDescriptorValue.Notify);

            if (status == GattCommunicationStatus.Success)
            {
                _notificationsIsEnabled = true;
                _dataCharacteristic.ValueChanged += dataCharacteristic_ValueChanged;
            }

            return status;
        }

        /// <summary>
        /// Disables active notifications from the device.
        /// </summary>
        /// <returns>Status from trying to change ConfigurationDescriptor on device.</returns>
        public virtual async Task<GattCommunicationStatus> DisableNotifications()
        {
            if (_notificationsIsEnabled == false)
            {
                return GattCommunicationStatus.Success;
            }

            GattCommunicationStatus status =
                await _dataCharacteristic.WriteClientCharacteristicConfigurationDescriptorAsync(
                    GattClientCharacteristicConfigurationDescriptorValue.None);

            if (status == GattCommunicationStatus.Success)
            {
                _notificationsIsEnabled = false;
                _dataCharacteristic.ValueChanged -= dataCharacteristic_ValueChanged;
            }

            return status;
        }

        /// <summary>
        /// Reads data from connected characteristic.
        /// </summary>
        /// <returns>Data from device.</returns>
        public async Task<byte[]> ReadValue()
        {
            if (_setupState != 2)
            {
                throw new InvalidStateException("Data characteristic is not connected.");
            }

            GattReadResult readResult = await _dataCharacteristic.ReadValueAsync(BluetoothCacheMode.Uncached);
            var data = new byte[readResult.Value.Length];
            DataReader.FromBuffer(readResult.Value).ReadBytes(data);
            return data;
        }

        /// <inheritdoc />
        public async void Dispose()
        {
            if (!object.ReferenceEquals(null, _dataCharacteristic) && _setupState == 2)
            {
                var status = await _dataCharacteristic.WriteClientCharacteristicConfigurationDescriptorAsync(
                GattClientCharacteristicConfigurationDescriptorValue.None);

                // try one more time if that failed
                if (status != GattCommunicationStatus.Success)
                {
                    await _dataCharacteristic.WriteClientCharacteristicConfigurationDescriptorAsync(
                        GattClientCharacteristicConfigurationDescriptorValue.None);
                }
            }

            _dataCharacteristic = null;
            _setupState = 0;
        }

        /// <summary>
        /// Event handler when received data via notification from device.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="args">Data.</param>
        private void dataCharacteristic_ValueChanged(
            GattCharacteristic sender,
            GattValueChangedEventArgs args)
        {
            var data = new byte[args.CharacteristicValue.Length];
            DataReader.FromBuffer(args.CharacteristicValue).ReadBytes(data);

            OnDataReceived(new SesnsorReadEventArgs()
            {
                Data = data,
                Length = args.CharacteristicValue.Length
            });
        }
    }
}
