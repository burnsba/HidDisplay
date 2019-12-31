using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsHardware.Bluetooth
{
    /// <summary>
    /// Helper methods.
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Extracts the assigned number from a UUID.
        /// </summary>
        /// <param name="g">UUID to extract assigned number from.</param>
        /// <returns>Assigned number.</returns>
        public static ushort UuidToAssignedNumber(Guid g)
        {
            var bytes = g.ToByteArray();
            return (ushort)((ushort)(bytes[1] << 8) | (ushort)bytes[0]);
        }
    }
}
