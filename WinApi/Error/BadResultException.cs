using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace WinApi.Error
{
    /// <summary>
    /// Thrown when a method call does not return success.
    /// </summary>
    public class BadResultException : Exception
    {
        /// <summary>
        /// Result from call.
        /// </summary>
        public object CallResult { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BadResultException"/> class.
        /// </summary>
        public BadResultException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BadResultException"/> class.
        /// </summary>
        /// <param name="message">Error message.</param>
        public BadResultException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BadResultException"/> class.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <param name="innerException">Inner exception.</param>
        public BadResultException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BadResultException"/> class.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Streaming context.</param>
        protected BadResultException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
