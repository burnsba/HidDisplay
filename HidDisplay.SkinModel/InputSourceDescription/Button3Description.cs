using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using HidDisplay.PluginDefinition;
using BurnsBac.HotConfig.Error;

namespace HidDisplay.SkinModel.InputSourceDescription
{
    /// <summary>
    /// Describes how to match/process incoming hardware events from <see cref="GenericInputEventArgs" />.
    /// </summary>
    public class Button3Description : IInputSourceDescription
    {
        /// <inheritdoc />
        public int Id { get; set; }

        /// <inheritdoc />
        public string Name { get; set; }

        /// <summary>
        /// Button state to match.
        /// </summary>
        public Button3State StateMatch { get; set; }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{Name}: {StateMatch}";
        }

        /// <summary>
        /// Processes xelement and creates <see cref="Button3Description"/>.
        /// </summary>
        /// <param name="node">Node to process.</param>
        /// <param name="containingDirectory">Parent directory containing images.</param>
        /// <returns>New <see cref="Button3Description"/>.</returns>
        public static Button3Description FromXElement(XElement node)
        {
            var inputSource = new Button3Description();

            inputSource.Name = (string)node.Attribute("name");

            var hwSettingsNode = node.Descendants("hwSettings").FirstOrDefault();
            if (!object.ReferenceEquals(null, hwSettingsNode))
            {
                try
                {
                    inputSource.Id = (int)hwSettingsNode.Attribute("id");
                }
                catch (Exception ex)
                {
                    throw new InvalidConfiguration($"Could not read inputHandler.item.hwSettings.id (line: {Parsers.GetNodeLine(hwSettingsNode)})", ex);
                }

                try
                {
                    inputSource.StateMatch = (Button3State)Enum.Parse(typeof(Button3State), hwSettingsNode.Attribute("stateMatch").Value, true);
                }
                catch (Exception ex)
                {
                    throw new InvalidConfiguration($"Could not read inputHandler.item.hwSettings.stateMatch (line: {Parsers.GetNodeLine(hwSettingsNode)})", ex);
                }
            }
            else
            {
                throw new InvalidConfiguration($"Missing inputHandler.item.hwSettings (line: {Parsers.GetNodeLine(node)})");
            }

            return inputSource;
        }
    }
}
