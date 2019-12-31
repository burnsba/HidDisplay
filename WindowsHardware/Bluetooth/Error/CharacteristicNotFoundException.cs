using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace WindowsHardware.Bluetooth.Error
{
    /// <summary>
    /// Thrown when characteristic can't be found on bluetooth device.
    /// </summary>
    public class CharacteristicNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CharacteristicNotFoundException"/> class.
        /// </summary>
        public CharacteristicNotFoundException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacteristicNotFoundException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public CharacteristicNotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacteristicNotFoundException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Inner exception.</param>
        public CharacteristicNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacteristicNotFoundException"/> class.
        /// </summary>
        /// <param name="info">SerializationInfo.</param>
        /// <param name="context">StreamingContext.</param>
        protected CharacteristicNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
