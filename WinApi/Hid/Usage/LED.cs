using System;
using System.Collections.Generic;
using System.Text;

namespace WinApi.Hid.Usage
{
    /// <summary>
    /// Usages for LED usage page (8).
    /// </summary>
    /// <remarks>
    /// https://www.freebsddiary.org/APC/usb_hid_usages.php
    /// </remarks>
    public enum LED
    {
        Undefined = 0x00,
        NumLock = 0x01,
        CapsLock = 0x02,
        ScrollLock = 0x03,
        Compose = 0x04,
        Kana = 0x05,
        Power = 0x06,
        Shift = 0x07,
        DoNotDisturb = 0x08,
        Mute = 0x09,
        ToneEnable = 0x0A,
        HighCutFilter = 0x0B,
        LowCutFilter = 0x0C,
        EqualizerEnable = 0x0D,
        SoundFieldOn = 0x0E,
        SurroundFieldOn = 0x0F,
        Repeat = 0x10,
        Stereo = 0x11,
        SamplingRateDetect = 0x12,
        Spinning = 0x13,
        CAV = 0x14,
        CLV = 0x15,
        RecordingFormatDetect = 0x16,
        OffHook = 0x17,
        Ring = 0x18,
        MessageWaiting = 0x19,
        DataMode = 0x1A,
        BatteryOperation = 0x1B,
        BatteryOK = 0x1C,
        BatteryLow = 0x1D,
        Speaker = 0x1E,
        HeadSet = 0x1F,
        Hold = 0x20,
        Microphone = 0x21,
        Coverage = 0x22,
        NightMode = 0x23,
        SendCalls = 0x24,
        CallPickup = 0x25,
        Conference = 0x26,
        Standby = 0x27,
        CameraOn = 0x28,
        CameraOff = 0x29,
        OnLine = 0x2A,
        OffLine = 0x2B,
        Busy = 0x2C,
        Ready = 0x2D,
        PaperOut = 0x2E,
        PaperJam = 0x2F,
        Remote = 0x30,
        Forward = 0x31,
        Reverse = 0x32,
        Stop = 0x33,
        Rewind = 0x34,
        FastForward = 0x35,
        Play = 0x36,
        Pause = 0x37,
        Record = 0x38,
        Error = 0x39,
        UsageSelectedIndicator = 0x3A,
        UsageInUseIndicator = 0x3B,
        UsageMultiModeIndicator = 0x3C,
        IndicatorOn = 0x3D,
        IndicatorFlash = 0x3E,
        IndicatorSlowBlink = 0x3F,
        IndicatorFastBlink = 0x40,
        IndicatorOff = 0x41,
        FlashOnTime = 0x42,
        SlowBlinkOnTime = 0x43,
        SlowBlinkOffTime = 0x44,
        FastBlinkOnTime = 0x45,
        FastBlinkOffTime = 0x46,
        UsageIndicatorColor = 0x47,
        Red = 0x48,
        Green = 0x49,
        Amber = 0x4A,
        GenericIndicator = 0x4B,
        SystemSuspend = 0x4C,
        ExternalPowerConnected = 0x4D,

        /// <summary>
        /// 0x4C-FFFF = Reserved
        /// </summary>
        Reserved_generic = 0x10000,
    }
}
