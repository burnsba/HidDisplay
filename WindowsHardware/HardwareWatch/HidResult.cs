using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsHardware.HardwareWatch
{
    /// <summary>
    /// Ties together HID device information with current button/range values.
    /// </summary>
    public class HidResult
    {
        /// <summary>
        /// Gets information about the HID device.
        /// </summary>
        public HidDeviceInfo HidDeviceInfo { get; internal set; }

        /// <summary>
        /// Gets or sets the list of buttons that are currently active.
        /// </summary>
        public ushort[] ButtonIndexActive { get; set; }

        /// <summary>
        /// Gets or sets the state of all buttons.
        /// </summary>
        public bool[] ButtonStates { get; set; }

        /// <summary>
        /// Gets or sets the range values.
        /// </summary>
        public List<UsagePageUsageValue> UsageValues { get; set; } = new List<UsagePageUsageValue>();

        /// <summary>
        /// Ties together a UsagePage, a Usage, and a value.
        /// </summary>
        public struct UsagePageUsageValue
        {
            /// <summary>
            /// Usage page.
            /// </summary>
            public WinApi.Hid.HidUsagePages UsagePage;

            /// <summary>
            /// Usage for the usage page.
            /// </summary>
            public uint Usage;

            /// <summary>
            /// Value for the input.
            /// </summary>
            public uint Value;

            public override string ToString()
            {
                return $"{WinApi.Hid.Utility.UsagePageAndUsageToString((int)UsagePage, (int)Usage)} = {Value}";
            }
        }
    }
}
