using System;
using System.Collections.Generic;
using System.Text;

namespace HidDisplay.SkinModel.Core
{
    /// <summary>
    /// UI item to display an image.
    /// </summary>
    public interface IUiImageItem : IUiItem, IDisposable
    {
        /// <summary>
        /// Gets or sets image to associate with button.
        /// </summary>
        ImageInfo Image { get; set; }
    }
}
