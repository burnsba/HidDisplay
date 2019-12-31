using System;
using System.Collections.Generic;
using System.Text;

namespace HidDisplay.SkinModel.Core
{
    /// <summary>
    /// Supported display types.
    /// </summary>
    public enum UiType
    {
        /// <summary>
        /// Default/unknown.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// <see cref="HidDisplay.SkinModel.Core.Display.ToggleButton" />.
        /// </summary>
        ToggleButton = 1,

        /// <summary>
        /// <see cref="HidDisplay.SkinModel.Core.Display.RadialVector" />.
        /// </summary>
        RadialVector = 2,

        /// <summary>
        /// <see cref="HidDisplay.SkinModel.Core.Display.FlashButton" />.
        /// </summary>
        FlashButton = 3,

        /// <summary>
        /// <see cref="HidDisplay.SkinModel.Core.Display.SimpleToString" />.
        /// </summary>
        SimpleToString = 4,
    }
}
