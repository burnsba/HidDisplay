using System;
using System.Collections.Generic;
using System.Text;

namespace WinApi.Hid.Usage
{
    /// <summary>
    /// Usages for VESAVirtualControls usage page (130).
    /// </summary>
    /// <remarks>
    /// https://www.freebsddiary.org/APC/usb_hid_usages.php
    /// </remarks>
    public enum VESAVirtualControls
    {
        Brightness = 0x10,
        Contrast = 0x12,
        VideoGainRed = 0x16,
        VideoGainGreen = 0x18,
        VideoGainBlue = 0x1A,
        Focus = 0x1C,
        HorizontalPosition = 0x20,
        HorizontalSize = 0x22,
        HorizontalPincushion = 0x24,
        HorizontalPincushionBalance = 0x26,
        HorizontalMisconvergence = 0x28,
        HorizontalLinearity = 0x2A,
        HorizontalLinearityBalance = 0x2C,
        VerticalPosition = 0x30,
        VerticalSize = 0x32,
        VerticalPincushion = 0x34,
        VerticalPincushionBalance = 0x36,
        VerticalMisconvergence = 0x38,
        VerticalLinearity = 0x3A,
        VerticalLinearityBalance = 0x3C,
        ParallelogramDistortion = 0x40,
        TrapezoidalDistortion = 0x42,
        Tilt = 0x44,
        TopCornerDistortionControl = 0x46,
        TopCornerDistortionBalance = 0x48,
        BottomCornerDistortionControl = 0x4A,
        BottomCornerDistortionBalance = 0x4C,
        MoirHorizontal = 0x56,
        MoirVertical = 0x58,
        InputLevelSelect = 0x5E,
        InputSourceSelect = 0x60,
        StereoMode = 0x62,
        VideoBlackLevelRed = 0x6C,
        VideoBlackLevelGreen = 0x6E,
        VideoBlackLevelBlue = 0x70,
    }
}
