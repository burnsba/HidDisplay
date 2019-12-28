using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsHardware.SerialPort
{
    /// <summary>
    /// Describes how to handle errors when receiving packets pet protocol.
    /// </summary>
    public enum ReadErrorHandling
    {
        /// <summary>
        /// Returns from read, will try again next interval.
        /// </summary>
        IgnoreRetry,

        /// <summary>
        /// Disconnects the port.
        /// </summary>
        Stop,

        /// <summary>
        /// Throws exception.
        /// </summary>
        Throw,
    }
}
