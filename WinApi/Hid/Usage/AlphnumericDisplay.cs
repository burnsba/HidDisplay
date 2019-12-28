using System;
using System.Collections.Generic;
using System.Text;

namespace WinApi.Hid.Usage
{
    /// <summary>
    /// Usages for AlphnumericDisplay usage page (20).
    /// </summary>
    /// <remarks>
    /// https://www.freebsddiary.org/APC/usb_hid_usages.php
    /// </remarks>
    public enum AlphnumericDisplay
    {
        Undefined = 0x00,
        AlphanumericDisplay = 0x01,
        DisplayAttributesReport = 0x20,
        ASCIICharacterSet = 0x21,
        DataReadBack = 0x22,
        FontReadBack = 0x23,
        DisplayControlReport = 0x24,
        ClearDisplay = 0x25,
        DisplayEnable = 0x26,
        ScreenSaverDelay = 0x27,
        ScreenSaverEnable = 0x28,
        VerticalScroll = 0x29,
        HorizontalScroll = 0x2A,
        CharacterReport = 0x2B,
        DisplayData = 0x2C,
        DisplayStatus = 0x2D,
        StatNotReady = 0x2E,
        StatReady = 0x2F,
        ErrNotaloadablecharacter = 0x30,
        ErrFontdatacannotberead = 0x31,
        CursorPositionReport = 0x32,
        Row = 0x33,
        Column = 0x34,
        Rows = 0x35,
        Columns = 0x36,
        CursorPixelPositioning = 0x37,
        CursorMode = 0x38,
        CursorEnable = 0x39,
        CursorBlink = 0x3A,
        FontReport = 0x3B,
        FontData = 0x3C,
        CharacterWidth = 0x3D,
        CharacterHeight = 0x3E,
        CharacterSpacingHorizontal = 0x3F,
        CharacterSpacingVertical = 0x40,
        UnicodeCharacterSet = 0x41,
    }
}
