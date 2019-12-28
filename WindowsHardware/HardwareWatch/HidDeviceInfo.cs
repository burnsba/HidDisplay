using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32;
using WinApi.Error;
using WinApi.Hid;
using WinApi.User32;

namespace WindowsHardware.HardwareWatch
{
    /// <summary>
    /// This class organizes information about a HID device.
    /// </summary>
    /// <remarks>
    /// Raw input processing should happen elsewhere, like <see cref="RawInputHandler"/>.
    /// </remarks>
    public class HidDeviceInfo : IDisposable
    {
        private string _registryDescription;
        private bool _registryDescriptionLoaded = false;
        private SafeHandle _fileHandle;
        private bool _fileHandleLoaded = false;
        private string _deviceNameFull;
        private bool _deviceNameFullLoaded = false;
        private string _serialNumber;
        private bool _serialNumberLoaded = false;
        private string _physicalDescriptor;
        private bool _physicalDescriptorLoaded = false;
        private string _manufacturer;
        private bool _manufacturerLoaded = false;
        private string _product;
        private bool _productLoaded = false;
        private bool _hidpCapsLoaded = false;
        private HidpButtonCaps[] _buttonCaps = null;
        private bool _hidpButtonCapsLoaded = false;
        private HidpValueCaps[] _valueCaps = null;
        private bool _hidpValueCapsLoaded = false;
        private ReadOnlyCollection<HidpButtonCaps> _buttonCapsReadonly = null;
        private ReadOnlyCollection<HidpValueCaps> _valueCapsReadonly = null;

        /// <summary>
        /// Gets HID device capabilities.
        /// </summary>
        public HidpCaps HidpCapabilities { get; internal set; }

        /// <summary>
        /// Gets HID device button capabilities.
        /// </summary>
        public ReadOnlyCollection<HidpButtonCaps> ButtonCapabilities
        {
            get
            {
                if (object.ReferenceEquals(null, _buttonCapsReadonly))
                {
                    throw new InvalidOperationException("Button capabilities have not been set yet.");
                }

                return _buttonCapsReadonly;
            }
        }

        /// <summary>
        /// Gets HID device value capabilities.
        /// </summary>
        public ReadOnlyCollection<HidpValueCaps> ValueCapabilities
        {
            get
            {
                if (object.ReferenceEquals(null, _valueCapsReadonly))
                {
                    throw new InvalidOperationException("Value capabilities have not been set yet.");
                }

                return _valueCapsReadonly;
            }
        }

        /// <summary>
        /// Gets or sets raw input device handle (not file handle).
        /// </summary>
        internal IntPtr DeviceHandle { get; set; }

        /// <summary>
        /// Gets button capabilities. Shortcut for <see cref="GetButtonCapabilities"/>.
        /// </summary>
        internal HidpButtonCaps[] ButtonCaps
        {
            get
            {
                if (!_hidpButtonCapsLoaded)
                {
                    throw new InvalidOperationException($"Call {nameof(GetButtonCapabilities)} first.");
                }

                return _buttonCaps;
            }
        }

        /// <summary>
        /// Gets value capabilities. Shortcut for <see cref="GetValueCapabilities"/>.
        /// </summary>
        internal HidpValueCaps[] ValueCaps
        {
            get
            {
                if (!_hidpValueCapsLoaded)
                {
                    throw new InvalidOperationException($"Call {nameof(GetValueCapabilities)} first.");
                }

                return _valueCaps;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HidDeviceInfo"/> class.
        /// </summary>
        /// <param name="deviceHandle">Pointer to raw input device handle (not file handle).</param>
        internal HidDeviceInfo(IntPtr deviceHandle)
        {
            DeviceHandle = deviceHandle;
        }

        /// <summary>
        /// This may not be available on some devices.
        /// Gets HID device serial number. Calls hid.dll if necessary.
        /// </summary>
        /// <returns>Serial number.</returns>
        public string GetSerialNumber()
        {
            if (!_serialNumberLoaded)
            {
                try
                {
                    _serialNumber = WinApi.Hid.Managed.HidD_GetSerialNumberString(GetFileHandle());
                }
                catch (BadResultException)
                {
                    _serialNumber = string.Empty;
                }

                _serialNumberLoaded = true;
            }

            return _serialNumber;
        }

        /// <summary>
        /// This may not be available on all (most?) devices.
        /// Gets HID device physical descriptor. Calls hid.dll if necessary.
        /// </summary>
        /// <returns>Physical descriptor.</returns>
        public string GetPhysicalDescriptor()
        {
            if (!_physicalDescriptorLoaded)
            {
                try
                {
                    // Some guy on the internet says, 
                    //     You can capture a usb etw log trace if you want to see if
                    //     it is a device error. There are very few devices that’s actually 
                    //     report a physical descriptor. Are you sure your device reports one ?
                    _physicalDescriptor = WinApi.Hid.Managed.HidD_GetPhysicalDescriptor(GetFileHandle());
                }
                catch (BadResultException)
                {
                    _physicalDescriptor = string.Empty;
                }

                _physicalDescriptorLoaded = true;
            }

            return _physicalDescriptor;
        }

        /// <summary>
        /// This may not be available on some devices.
        /// Gets HID device manufacturer. Calls hid.dll if necessary.
        /// </summary>
        /// <returns>Manufacturer.</returns>
        public string GetManufacturer()
        {
            if (!_manufacturerLoaded)
            {
                try
                {
                    _manufacturer = WinApi.Hid.Managed.HidD_GetManufacturerString(GetFileHandle());
                }
                catch (BadResultException)
                {
                    _manufacturer = string.Empty;
                }

                _manufacturerLoaded = true;
            }

            return _manufacturer;
        }

        /// <summary>
        /// This may not be available on some devices.
        /// Gets HID device product name. Calls hid.dll if necessary.
        /// </summary>
        /// <returns>Product name.</returns>
        public string GetProduct()
        {
            if (!_productLoaded)
            {
                try
                {
                    _product = WinApi.Hid.Managed.HidD_GetProductString(GetFileHandle());
                }
                catch (BadResultException)
                {
                    _product = string.Empty;
                }

                _productLoaded = true;
            }

            return _product;
        }

        /// <summary>
        /// Gets HID device name/path according to Windows operating system. Calls hid.dll if necessary.
        /// This should contain Class code, SubClass code, Protocol code, and ClassGUID.
        /// </summary>
        /// <returns>Device name/path.</returns>
        public string GetDeviceNameFull()
        {
            if (!_deviceNameFullLoaded)
            {
                _deviceNameFull = WinApi.User32.Managed.GetRawInputDeviceName(DeviceHandle);

                _deviceNameFullLoaded = true;
            }

            return _deviceNameFull;
        }

        /// <summary>
        /// Gets HID device description from windows registry.
        /// </summary>
        /// <returns>Device description.</returns>
        public string GetRegistryDescription()
        {
            if (!_registryDescriptionLoaded)
            {
                var dn = GetDeviceNameFull();

                if (string.IsNullOrEmpty(dn))
                {
                    throw new InvalidOperationException($"DeviceName has not been set yet.");
                }

                var s = dn.Substring(4);

                string[] split = s.Split('#');

                string id_01 = split[0];    // ACPI (Class code)
                string id_02 = split[1];    // PNP0303 (SubClass code)
                string id_03 = split[2];    // 3&13c0b0c5&0 (Protocol code)

                RegistryKey rk;
                if (Environment.Is64BitOperatingSystem)
                    rk = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                else
                    rk = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);

                _registryDescription = rk
                    .OpenSubKey("System")
                    .OpenSubKey("CurrentControlSet")
                    .OpenSubKey("Enum")
                    .OpenSubKey(id_01)
                    .OpenSubKey(id_02)
                    .OpenSubKey(id_03)
                    .GetValue("DeviceDesc").ToString();

                // other registry keys:
                // Address
                // Capabilities
                // ClassGUID
                // CompatibleIDs
                // ConfigFlags
                // ContainerID
                // DeviceCharacteristics
                // Driver
                // HardwareID
                // Mfg
                // Security

                _registryDescriptionLoaded = true;
            }

            return _registryDescription;
        }

        /// <summary>
        /// Releases resources.
        /// </summary>
        public void Dispose()
        {
            if (!object.ReferenceEquals(null, _fileHandle))
            {
                if (!_fileHandle.IsClosed)
                {
                    _fileHandle.Close();
                }
            }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            if (_productLoaded)
            {
                return _product;
            }

            return DeviceHandle.ToString();
        }

        /// <summary>
        /// Calls <see cref="WinApi.Kernel32.Api.CreateFile"/> to get a file handle for the device.
        /// </summary>
        /// <returns>File handle.</returns>
        internal SafeHandle GetFileHandle()
        {
            if (!_fileHandleLoaded)
            {
                _fileHandle = WinApi.Kernel32.Api.CreateFile(
                    GetDeviceNameFull(),
                    (uint)WinApi.Kernel32.CreateFileDesiredAccess.GENERIC_READ,
                    (uint)(FileShare.Read | FileShare.Write),
                    IntPtr.Zero,
                    (uint)FileMode.Open,
                    0,
                    IntPtr.Zero);

                if (_fileHandle.IsInvalid || _fileHandle.IsClosed)
                {
                    throw new BadResultException("Failed to open HID device.");
                }

                _fileHandleLoaded = true;
            }

            return _fileHandle;
        }

        /// <summary>
        /// Gets HID device capabilities. If this has been called before, the
        /// cached information is returned unless <paramref name="forceLoad"/> is set.
        /// </summary>
        /// <param name="preparsedData">Preparsed data.</param>
        /// <param name="forceLoad">Optional override to force retrieving data from perparsed data again.</param>
        /// <returns>Capabilities.</returns>
        internal HidpCaps GetCapabilities(IntPtr preparsedData, bool forceLoad = false)
        {
            if (forceLoad || !_hidpCapsLoaded)
            {
                HidpCapabilities = WinApi.User32.Managed.HidpGetCapabilities(preparsedData);
                _hidpCapsLoaded = true;
            }
            
            return HidpCapabilities;
        }

        /// <summary>
        /// Gets HID device button capabilities. If this has been called before, the
        /// cached information is returned unless <paramref name="forceLoad"/> is set.
        /// </summary>
        /// <param name="preparsedData">Preparsed data.</param>
        /// <param name="forceLoad">Optional override to force retrieving data from perparsed data again.</param>
        /// <returns>Button capabilities.</returns>
        internal HidpButtonCaps[] GetButtonCapabilities(IntPtr preparsedData, bool forceLoad = false)
        {
            if (forceLoad || !_hidpButtonCapsLoaded)
            {
                _buttonCaps = WinApi.User32.Managed.HidpGetButtonCapabilities(HidpCapabilities, preparsedData);
                _hidpButtonCapsLoaded = true;

                _buttonCapsReadonly = new ReadOnlyCollection<HidpButtonCaps>(_buttonCaps.ToList());
            }

            return _buttonCaps;
        }

        /// <summary>
        /// Gets HID device value capabilities. If this has been called before, the
        /// cached information is returned unless <paramref name="forceLoad"/> is set.
        /// </summary>
        /// <param name="preparsedData">Preparsed data.</param>
        /// <param name="forceLoad">Optional override to force retrieving data from perparsed data again.</param>
        /// <returns>Value capabilities.</returns>
        internal HidpValueCaps[] GetValueCapabilities(IntPtr preparsedData, bool forceLoad = false)
        {
            if (forceLoad || !_hidpValueCapsLoaded)
            {
                _valueCaps = WinApi.User32.Managed.HidpGetValueCapabilities(HidpCapabilities, preparsedData);
                _hidpValueCapsLoaded = true;

                _valueCapsReadonly = new ReadOnlyCollection<HidpValueCaps>(_valueCaps.ToList());
            }

            return _valueCaps;
        }
    }
}
