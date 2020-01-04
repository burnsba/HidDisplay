using System;
using System.Collections.Generic;
using System.Text;

namespace HidDisplay.SkinModel.Core
{
    /// <summary>
    /// Interface definition for an object that can be drawn in the display area.
    /// </summary>
    /// <remarks>
    /// Should this be merged into IUiItem? .
    /// </remarks>
    public interface IPositionable
    {
        /// <summary>
        /// Gets or sets the x offfset used to display the item in the main area.
        /// </summary>
        int XOffset { get; set; }

        /// <summary>
        /// Gets or sets the y offset used to display the item in the main area.
        /// </summary>
        int YOffset { get; set; }
    }
}
