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
    public class Button2Description : IInputSourceDescription
    {
        /// <inheritdoc />
        public int Id { get; set; }

        /// <inheritdoc />
        public string Name { get; set; }

        /// <summary>
        /// Button state to match.
        /// </summary>
        public Button2State StateMatch { get; set; }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{Name}: {StateMatch}";
        }

        /// <summary>
        /// Processes xelement and creates <see cref="Button2Description"/>.
        /// </summary>
        /// <param name="node">Node to process.</param>
        /// <returns>New <see cref="Button2Description"/>.</returns>
        public static Button2Description FromXElement(XElement node)
        {
            var inputSource = new Button2Description();

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
                    inputSource.StateMatch = (Button2State)Enum.Parse(typeof(Button2State), hwSettingsNode.Attribute("stateMatch").Value, true);
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
