using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using HidDisplay.SkinModel.Error;

namespace HidDisplay.SkinModel.Core.Display
{
    /// <summary>
    /// Should be displayed for a brief period of time and then disappear.
    /// </summary>
    public class FlashButton : IUiImageItem
    {
        /// <inheritdoc />
        public ImageInfo Image { get; set; }

        /// <summary>
        /// Gets or sets duration in milliseconds for how long to display the item.
        /// </summary>
        public int DisplayDurationMs { get; set; }

        /// <inheritdoc />
        public void Dispose()
        {
            if (!object.ReferenceEquals(null, Image))
            {
                Image.Dispose();
            }
        }

        /// <summary>
        /// Processes xelement and creates <see cref="FlashButton"/>.
        /// </summary>
        /// <param name="node">Node to process.</param>
        /// <param name="parent">Parent info.</param>
        /// <returns>New <see cref="FlashButton"/>.</returns>
        public static FlashButton FromXElement(XElement node, Skin parent)
        {
            var fb = new FlashButton();

            var uiSettingsNode = node.Descendants("uiSettings").FirstOrDefault();
            if (!object.ReferenceEquals(null, uiSettingsNode))
            {
                fb.Image = ImageInfo.FromXElement(uiSettingsNode, parent.AbsoluteContainerPath);

                try
                {
                    fb.DisplayDurationMs = (int)uiSettingsNode.Attribute("duration");
                }
                catch (Exception ex)
                {
                    throw new InvalidConfiguration($"Could not read inputHandler.item.uiSettings.duration (line: {Parsers.GetNodeLine(uiSettingsNode)})", ex);
                }
            }
            else
            {
                throw new InvalidConfiguration($"Missing inputHandler.item.uiSettings (line: {Parsers.GetNodeLine(node)})");
            }

            return fb;
        }
    }
}
