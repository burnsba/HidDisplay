using System;
using System.Collections.Generic;
using System.Text;

namespace HidDisplay.SkinModel.Core
{
    /// <summary>
    /// UI item to display text.
    /// </summary>
    public interface IUiTextItem : IUiItem
    {
        /// <summary>
        /// Gets or sets information about how to display text.
        /// </summary>
        TextInfo TextInfo { get; set; }
    }
}
