using System;
using System.Collections.Generic;
using System.Text;

namespace HidDisplay.SkinModel.Core
{
    /// <summary>
    /// Helper functions for UI interaction code.
    /// </summary>
    public static class UiHelper
    {
        /// <summary>
        /// Determines whether the item is an image.
        /// </summary>
        /// <param name="item">Item to resolve.</param>
        /// <returns>True if <see cref="IUiImageItem"/>, false otherwise.</returns>
        public static bool IsImage(IUiItem item)
        {
            if (object.ReferenceEquals(null, item))
            {
                return false;
            }

            if (typeof(IUiImageItem).IsAssignableFrom(item.GetType()))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether the item is text.
        /// </summary>
        /// <param name="item">Item to resolve.</param>
        /// <returns>True if <see cref="IUiTextItem"/>, false otherwise.</returns>
        public static bool IsText(IUiItem item)
        {
            if (object.ReferenceEquals(null, item))
            {
                return false;
            }

            if (typeof(IUiTextItem).IsAssignableFrom(item.GetType()))
            {
                return true;
            }

            return false;
        }
    }
}
