using System;
using System.Collections.Generic;
using System.Text;

namespace WinApi.Hid
{
    /// <summary>
    /// HID Usage pages.
    /// </summary>
    /// <remarks>
    /// https://www.freebsddiary.org/APC/usb_hid_usages.php
    /// </remarks>
    public enum HidUsagePages : ushort
    {
        GenericDesktop = 1,
        SimulationControl = 2,
        VRControl = 3,
        SportsControl = 4,
        GameControl = 5,
        Keyboard = 7,
        LED = 8,
        Button = 9,
        Ordinal = 10,
        Telephony = 11,
        Consumer = 12,
        Digitizer = 13,
        PhysicalInterfaceDevice = 15,
        Unicode = 16,
        AlphnumericDisplay = 20,
        Monitor = 128,
        MonitorEnumeratedValues = 129,
        VESAVirtualControls = 130,
        VESACommand = 131,
        PowerDevice = 132,
        BatterySystem = 133,
        BarCodeScanner = 140,
        ScaleDevice = 141,
        CameraControl = 144,
        ArcadeDevice = 145,
    }
}
