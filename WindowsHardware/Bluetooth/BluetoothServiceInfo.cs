using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsHardware.Bluetooth
{
    /// <summary>
    /// High level information for a bluetooth service.
    /// </summary>
    public class BluetoothServiceInfo
    {
        /// <summary>
        /// Gets or sets service UUID.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets characteristics associated with service.
        /// </summary>
        public List<BluetoothCharacteristicInfo> Characteristics { get; set; } = new List<BluetoothCharacteristicInfo>();
    }
}
