using System;
using System.Collections.Generic;
using System.Text;
using WinApi.Windows;
using WindowsHardwareWatch.HardwareWatch.Enums;
using WindowsHardwareWatch.Windows;

namespace WindowsHardwareWatch.HardwareWatch
{
    /// <summary>
    /// Arguments for keyboard change event.
    /// </summary>
    public class KeyboardChangeEventArgs
    {
        /// <summary>
        /// Type of key change, either key up or key down.
        /// </summary>
        public KeyChangeDirection Direction { get; internal set; }

        /// <summary>
        /// Which key changed.
        /// </summary>
        public Keys Key { get; internal set; }

        /// <summary>
        /// Whether or not the alt key is currently being held.
        /// </summary>
        public bool Alt { get; internal set; }
    }
}
