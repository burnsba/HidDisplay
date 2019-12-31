using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsHardware.Bluetooth
{
    /// <summary>
    /// Event arguments when receiving raw data from Bluetooth device.
    /// </summary>
    public class SesnsorReadEventArgs : EventArgs
    {
        /// <summary>
        /// Gets data received from device event.
        /// </summary>
        public byte[] Data { get; internal set; }

        /// <summary>
        /// Gets number of bytes in event data.
        /// </summary>
        public uint Length { get; internal set; }
    }
}
