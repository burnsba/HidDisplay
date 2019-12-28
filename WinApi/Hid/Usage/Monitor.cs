using System;
using System.Collections.Generic;
using System.Text;

namespace WinApi.Hid.Usage
{
    /// <summary>
    /// Usages for Monitor usage page (128).
    /// </summary>
    /// <remarks>
    /// https://www.freebsddiary.org/APC/usb_hid_usages.php
    /// </remarks>
    public enum Monitor
    {
        Undefined = 0x00,
        MonitorControl = 0x01,
        EDIDInformation = 0x02,
        VDIFInformation = 0x03,
        VESAVersion = 0x04,
        OnScreenDisplay = 0x05,
        AutoSizeCenter = 0x06,
        PolarityHorzSynch = 0x07,
        PolarityVertSynch = 0x08,
        SyncType = 0x09,
        ScreenPosition = 0x0A,
        HorizontalFrequency = 0x0B,
        VerticalFrequency = 0x0C,
    }
}
