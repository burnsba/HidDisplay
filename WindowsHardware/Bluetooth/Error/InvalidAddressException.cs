using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace WindowsHardware.Bluetooth.Error
{
    /// <summary>
    /// Thrown when address doesn't resolve to a bluetooth device.
    /// </summary>
    public class InvalidAddressException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidAddressException"/> class.
        /// </summary>
        public InvalidAddressException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidAddressException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public InvalidAddressException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidAddressException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Inner exception.</param>
        public InvalidAddressException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidAddressException"/> class.
        /// </summary>
        /// <param name="info">SerializationInfo.</param>
        /// <param name="context">StreamingContext.</param>
        protected InvalidAddressException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
