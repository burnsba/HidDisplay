using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using HidDisplay.PluginDefinition;
using HidDisplay.SkinModel.Error;

namespace HidDisplay.SkinModel.InputSourceDescription
{
    /// <summary>
    /// Describes how to match/process incoming hardware events from <see cref="GenericInputEventArgs" />.
    /// </summary>
    public class RangeableInputDescription : IInputSourceDescription
    {
        /// <inheritdoc />
        public int Id { get; set; }

        /// <inheritdoc />
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets input ceiling. This value will be used for any larger input.
        /// </summary>
        public double InputCeiling { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether input should be inverted.
        /// </summary>
        public bool Invert1 { get; set; } = false;

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{Name}";
        }

        /// <summary>
        /// Processes xelement and creates <see cref="RangeableInputDescription"/>.
        /// </summary>
        /// <param name="node">Node to process.</param>
        /// <returns>New <see cref="RangeableInputDescription"/>.</returns>
        public static RangeableInputDescription FromXElement(XElement node)
        {
            var inputSource = new RangeableInputDescription();

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
                    inputSource.InputCeiling = (double)hwSettingsNode.Attribute("inputCeiling");
                }
                catch (Exception ex)
                {
                    throw new InvalidConfiguration($"Could not read inputHandler.item.hwSettings.inputCeiling (line: {Parsers.GetNodeLine(hwSettingsNode)})", ex);
                }

                try
                {
                    inputSource.Invert1 = Parsers.MakeBool(hwSettingsNode.Attribute("invert1").Value);
                }
                catch
                {
                    // optional
                    inputSource.Invert1 = false;
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
