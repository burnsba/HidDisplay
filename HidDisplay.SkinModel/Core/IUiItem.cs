using System;
using System.Collections.Generic;
using System.Text;

namespace HidDisplay.SkinModel.Core
{
    /// <summary>
    /// Item to display on screen.
    /// </summary>
    public interface IUiItem : IDisposable
    {
        /// <summary>
        /// Gets or sets image to associate with button.
        /// </summary>
        ImageInfo Image { get; set; }
    }
}
