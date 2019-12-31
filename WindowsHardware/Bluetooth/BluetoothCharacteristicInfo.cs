using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsHardware.Bluetooth
{
    /// <summary>
    /// High level information for a bluetooth characteristic.
    /// </summary>
    public class BluetoothCharacteristicInfo
    {
        private ushort _assignedNumber;
        private Guid _id;

        /// <summary>
        /// Gets or sets UUID of characteristic.
        /// </summary>
        public Guid Id
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
                _assignedNumber = Utility.UuidToAssignedNumber(value);
            }
        }

        /// <summary>
        /// Gets assigned number of characteristic.
        /// </summary>
        public ushort AssignedNumber
        {
            get
            {
                return Utility.UuidToAssignedNumber(_id);
            }
        }
    }
}
