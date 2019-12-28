using System;
using System.Collections.Generic;
using System.Text;

namespace WinApi.Hid.Usage
{
    /// <summary>
    /// Usages for Sports Control usage page (4).
    /// </summary>
    /// <remarks>
    /// https://www.freebsddiary.org/APC/usb_hid_usages.php
    /// </remarks>
    public enum SportsControl
    {
        Unidentified = 0x00,
        BaseballBat = 0x01,
        GolfClub = 0x02,
        RowingMachine = 0x03,
        Treadmill = 0x04,
        Oar = 0x30,
        Slope = 0x31,
        Rate = 0x32,
        StickSpeed = 0x33,
        StickFaceAngle = 0x34,
        StickHeelToe = 0x35,
        StickFollowThrough = 0x36,
        StickTempo = 0x37,
        StickType = 0x38,
        StickHeight = 0x39,
        Putter = 0x50,
        _1Iron = 0x51,
        _2Iron = 0x52,
        _3Iron = 0x53,
        _4Iron = 0x54,
        _5Iron = 0x55,
        _6Iron = 0x56,
        _7Iron = 0x57,
        _8Iron = 0x58,
        _9Iron = 0x59,
        _10Iron = 0x5A,
        _11Iron = 0x5B,
        SandWedge = 0x5C,
        LoftWedge = 0x5D,
        PowerWedge = 0x5E,
        _1Wood = 0x5F,
        _3Wood = 0x60,
        _5Wood = 0x61,
        _7Wood = 0x62,
        _9Wood = 0x63,
    }
}
