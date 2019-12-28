using System;
using System.Collections.Generic;
using System.Text;

namespace WinApi.Hid.Usage
{
    /// <summary>
    /// Usages for VESACommand usage page (131).
    /// </summary>
    /// <remarks>
    /// https://www.freebsddiary.org/APC/usb_hid_usages.php
    /// </remarks>
    public enum VESACommand
    {
        Undefined = 0x00,
        Settings = 0x01,
        Degauss = 0x02,
    }
}
