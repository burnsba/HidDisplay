﻿using HidDisplay.SkinModel.HotConfig;
using HidDisplay.SkinModel.HotConfig.DataSource;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Timers;
using Windows.Devices.Bluetooth.Advertisement;
using WindowsHardware.Bluetooth;

namespace HidDisplay.DefaultConfigDataProviders
{
    /// <summary>
    /// Scans for connected bluetooth devices.
    /// </summary>
    public class BluetoothAddressProvider : IConfigDataProviderPoll, IDisposable
    {
        private HashSet<ulong> _seenDeviceAddressess = new HashSet<ulong>();

        private List<BluetoothDeviceInfo> _bluetoothDevices = new List<BluetoothDeviceInfo>();

        private BluetoothLEAdvertisementWatcher _watcher;
        private bool _currentlyScanning = false;
        private Timer _scanTimer;

        /// <inheritdoc />
        public ObservableCollection<ConfigSettingItem> DataItems { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BluetoothAddressProvider"/> class.
        /// </summary>
        public BluetoothAddressProvider()
        {
            DataItems = new ObservableCollection<ConfigSettingItem>();

            // Create Bluetooth Listener
            _watcher = new BluetoothLEAdvertisementWatcher();

            _watcher.ScanningMode = BluetoothLEScanningMode.Active;

            // Only activate the watcher when we're recieving values >= -80
            _watcher.SignalStrengthFilter.InRangeThresholdInDBm = -80;

            // Stop watching if the value drops below -90 (user walked away)
            _watcher.SignalStrengthFilter.OutOfRangeThresholdInDBm = -90;

            // Register callback for when we see an advertisements
            _watcher.Received += OnAdvertisementReceived;

            // Wait 5 seconds to make sure the device is really out of range
            _watcher.SignalStrengthFilter.OutOfRangeTimeout = TimeSpan.FromMilliseconds(5000);
            _watcher.SignalStrengthFilter.SamplingInterval = TimeSpan.FromMilliseconds(1000);
        }

        /// <summary>
        /// Event handler when a new advertisement is received.
        /// </summary>
        /// <param name="watcher">Watcher.</param>
        /// <param name="eventArgs">Args.</param>
        private void OnAdvertisementReceived(BluetoothLEAdvertisementWatcher watcher, BluetoothLEAdvertisementReceivedEventArgs eventArgs)
        {
            if (_seenDeviceAddressess.Contains(eventArgs.BluetoothAddress))
            {
                return;
            }

            if (!string.IsNullOrEmpty(eventArgs.Advertisement.LocalName))
            {
                var info = new BluetoothDeviceInfo()
                {
                    Address = eventArgs.BluetoothAddress,
                    LocalName = eventArgs.Advertisement.LocalName,
                    ServiceUuids = new List<Guid>(eventArgs.Advertisement.ServiceUuids), // probably empty
                };

                _bluetoothDevices.Add(info);
                _seenDeviceAddressess.Add(eventArgs.BluetoothAddress);

                DataItems.Add(new ConfigSettingItem()
                {
                    Data = info,
                    Key = info.Address.ToString(),
                    Display = info.LocalName,
                });
            }

            // Tell the user we see an advertisement and print some properties
            System.Diagnostics.Debug.WriteLine(String.Format($"Advertisement: ({eventArgs.AdvertisementType.ToString()})"));
            System.Diagnostics.Debug.WriteLine(String.Format("  BT_ADDR: {0}", eventArgs.BluetoothAddress));
            System.Diagnostics.Debug.WriteLine(String.Format("  FR_NAME: {0}", eventArgs.Advertisement.LocalName));
            System.Diagnostics.Debug.WriteLine(String.Format("  ServiceUuids: {0}", String.Join(",", eventArgs.Advertisement.ServiceUuids.Select(x => x.ToString()))));
            System.Diagnostics.Debug.WriteLine("");
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Stop();
        }

        /// <summary>
        /// Starts scanning for bluetooth devices.
        /// </summary>
        public void Start()
        {
            if (_currentlyScanning)
            {
                return;
            }

            _currentlyScanning = true;
            _watcher.Start();

            if (object.ReferenceEquals(null, _scanTimer))
            {
                _scanTimer = new Timer();
                _scanTimer.Interval = 20000;
                _scanTimer.AutoReset = false;
                _scanTimer.Elapsed += (s, e) => { Stop(); };
                _scanTimer.Start();
            }
            else
            {
                _scanTimer.Stop();
                _scanTimer.Start();
            }
        }

        /// <summary>
        /// Stops scanning for bluetooth devices.
        /// </summary>
        public void Stop()
        {
            _watcher.Stop();
            _scanTimer.Stop();
            _currentlyScanning = false;
        }
    }
}
