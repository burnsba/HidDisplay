using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsHardware.Bluetooth
{
    /// <summary>
    /// High level information for a bluetooth device.
    /// </summary>
    public class BluetoothDeviceInfo
    {
        /// <summary>
        /// Gets or sets device address.
        /// </summary>
        public ulong Address { get; set; }

        /// <summary>
        /// Gets or sets device local name.
        /// </summary>
        public string LocalName { get; set; }

        /// <summary>
        /// Gets or sets service UUIDs for device.
        /// </summary>
        public List<Guid> ServiceUuids { get; set; } = new List<Guid>();

        /// <summary>
        /// Gets or sets services for device.
        /// </summary>
        public List<BluetoothServiceInfo> Services { get; set; } = new List<BluetoothServiceInfo>();

        /// <inheritdoc />
        public override string ToString()
        {
            return LocalName;
        }
    }
}
