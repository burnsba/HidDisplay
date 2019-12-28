using System;
using System.Collections.Generic;
using System.Text;

namespace WinApi.Hid.Usage
{
    /// <summary>
    /// Usages for VRControl usage page (3).
    /// </summary>
    /// <remarks>
    /// https://www.freebsddiary.org/APC/usb_hid_usages.php
    /// </remarks>
    public enum VRControl
    {
        Unidentified = 0x00,
        Belt = 0x01,
        BodySuit = 0x02,
        Flexor = 0x03,
        Glove = 0x04,
        HeadTracker = 0x05,
        HeadMountedDisplay = 0x06,
        HandTracker = 0x07,
        Oculometer = 0x08,
        Vest = 0x09,
        AnimatronicDevice = 0x0A,
        StereoEnable = 0x20,
        DisplayEnable = 0x21,
    }
}
