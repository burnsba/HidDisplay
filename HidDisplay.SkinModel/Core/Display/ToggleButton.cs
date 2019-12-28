using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using HidDisplay.SkinModel.Error;

namespace HidDisplay.SkinModel.Core.Display
{
    /// <summary>
    /// Should be displayed until state changes or hidden.
    /// </summary>
    public class ToggleButton : IUiItem
    {
        /// <inheritdoc />
        public ImageInfo Image { get; set; }

        /// <inheritdoc />
        public void Dispose()
        {
            if (!object.ReferenceEquals(null, Image))
            {
                Image.Dispose();
            }
        }

        /// <summary>
        /// Processes xelement and creates <see cref="ToggleButton"/>.
        /// </summary>
        /// <param name="node">Node to process.</param>
        /// <param name="parent">Parent info.</param>
        /// <returns>New <see cref="ToggleButton"/>.</returns>
        public static ToggleButton FromXElement(XElement node, Skin parent)
        {
            var tb = new ToggleButton();

            var uiSettingsNode = node.Descendants("uiSettings").FirstOrDefault();
            if (!object.ReferenceEquals(null, uiSettingsNode))
            {
                tb.Image = ImageInfo.FromXElement(uiSettingsNode, parent.AbsoluteContainerPath);
            }
            else
            {
                throw new InvalidConfiguration($"Missing inputHandler.item.uiSettings (line: {Parsers.GetNodeLine(node)})");
            }

            return tb;
        }
    }
}
