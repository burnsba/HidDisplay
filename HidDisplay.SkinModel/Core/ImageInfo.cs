using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using HidDisplay.SkinModel.Error;

namespace HidDisplay.SkinModel.Core
{
    /// <summary>
    /// Describes image information.
    /// </summary>
    public class ImageInfo : IPositionable, IDisposable
    {
        /// <summary>
        /// Gets or sets absolute path to image file.
        /// </summary>
        private string AbsolutePathFilename { get; set; }

        /// <summary>
        /// Gets or sets image filename.
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// Gets or sets the width of the image according to the file source.
        /// </summary>
        public int OriginalWidth { get; set; }

        /// <summary>
        /// Gets or sets the height of the image according to the file source.
        /// </summary>
        public int OriginalHeight { get; set; }

        /// <summary>
        /// Gets or sets width image should be resized to.
        /// </summary>
        public int? OverrideWidth { get; set; }

        /// <summary>
        /// Gets or sets the height the image should be resized to.
        /// </summary>
        public int? OverrideHeight { get; set; }

        /// <summary>
        /// Gets or sets the x offfset used to display the image in the main area.
        /// </summary>
        public int XOffset { get; set; }

        /// <summary>
        /// Gets or sets the y offset used to display the image in the main area.
        /// </summary>
        public int YOffset { get; set; }

        /// <summary>
        /// Gets the width used for the image.
        /// </summary>
        public int Width
        {
            get
            {
                if (OverrideWidth.HasValue)
                {
                    return OverrideWidth.Value;
                }

                return OriginalWidth;
            }
        }

        /// <summary>
        /// Gets the height used for the image.
        /// </summary>
        public int Height
        {
            get
            {
                if (OverrideHeight.HasValue)
                {
                    return OverrideHeight.Value;
                }

                return OriginalHeight;
            }
        }

        /// <summary>
        /// Gets associate image data for the file.
        /// </summary>
        public BitmapImage ImageData { get; private set; }

        /// <summary>
        /// Loads image from disk into memory.
        /// </summary>
        public void LoadImageFromDisk()
        {
            ImageData = new BitmapImage(new Uri(AbsolutePathFilename));
            OriginalHeight = ImageData.PixelHeight;
            OriginalWidth = ImageData.PixelWidth;
        }

        /// <summary>
        /// Unloads image from memory.
        /// </summary>
        public void FreeImageResources()
        {
            ImageData = null;
            OriginalHeight = 0;
            OriginalWidth = 0;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            FreeImageResources();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return Filename;
        }

        /// <summary>
        /// Processes xelement and creates <see cref="ImageInfo"/>.
        /// </summary>
        /// <param name="node">Node to process.</param>
        /// <param name="containingDirectory">Parent directory containing images.</param>
        /// <returns>New <see cref="ImageInfo"/>.</returns>
        public static ImageInfo FromXElement(XElement node, string containingDirectory)
        {
            var image = new ImageInfo();

            image.Filename = (string)node.Attribute("image");

            image.AbsolutePathFilename = System.IO.Path.Combine(containingDirectory, image.Filename);
            if (!System.IO.File.Exists(image.AbsolutePathFilename))
            {
                throw new InvalidConfiguration($"File not found: {image.AbsolutePathFilename} (line: {Parsers.GetNodeLine(node)})");
            }

            if (node.Attribute("overrideHeight") != null
                && !string.IsNullOrEmpty(node.Attribute("overrideHeight").Value))
            {
                image.OverrideHeight = (int)node.Attribute("overrideHeight");
            }
            else
            {
                image.OverrideHeight = null;
            }

            if (node.Attribute("overrideWidth") != null
                && !string.IsNullOrEmpty(node.Attribute("overrideWidth").Value))
            {
                image.OverrideWidth = (int)node.Attribute("overrideWidth");
            }
            else
            {
                image.OverrideWidth = null;
            }

            if (node.Attribute("xOffset") != null
                && !string.IsNullOrEmpty(node.Attribute("xOffset").Value))
            {
                image.XOffset = (int)node.Attribute("xOffset");
            }
            else
            {
                image.XOffset = 0;
            }

            if (node.Attribute("yOffset") != null
                && !string.IsNullOrEmpty(node.Attribute("yOffset").Value))
            {
                image.YOffset = (int)node.Attribute("yOffset");
            }
            else
            {
                image.YOffset = 0;
            }

            return image;
        }
    }
}
