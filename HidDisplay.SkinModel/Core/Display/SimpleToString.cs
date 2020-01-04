using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using BurnsBac.HotConfig.Error;

namespace HidDisplay.SkinModel.Core.Display
{
    /// <summary>
    /// Should be displayed until state changes or hidden.
    /// </summary>
    public class SimpleToString : IUiTextItem
    {
        /// <inheritdoc />
        public TextInfo TextInfo { get; set; }

        /// <summary>
        /// Processes xelement and creates <see cref="SimpleToString"/>.
        /// </summary>
        /// <param name="node">Node to process.</param>
        /// <param name="parent">Parent info.</param>
        /// <returns>New <see cref="SimpleToString"/>.</returns>
        public static SimpleToString FromXElement(XElement node, Skin parent)
        {
            var fts = new SimpleToString();

            var uiSettingsNode = node.Descendants("uiSettings").FirstOrDefault();
            if (!object.ReferenceEquals(null, uiSettingsNode))
            {
                fts.TextInfo = TextInfo.FromXElement(uiSettingsNode, parent.AbsoluteContainerPath);

                // no other settings to parse here
            }
            else
            {
                throw new InvalidConfiguration($"Missing inputHandler.item.uiSettings (line: {Parsers.GetNodeLine(node)})");
            }

            return fts;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            // nothing to do
        }
    }
}
