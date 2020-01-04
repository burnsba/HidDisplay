using System;
using System.Linq;
using System.Xml.Linq;
using BurnsBac.HotConfig.Error;
using HidDisplay.PluginDefinition;

namespace HidDisplay.SkinModel.InputSourceDescription
{
    /// <summary>
    /// Describes how to match/process incoming hardware events from <see cref="GenericInputEventArgs" />.
    /// </summary>
    public class RangeableInput3Description : IInputSourceDescription
    {
        /// <inheritdoc />
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets input ceiling. This value will be used for any larger input.
        /// </summary>
        public double InputCeiling { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether input 1/x should be inverted.
        /// </summary>
        public bool Invert1 { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether input 2/y should be inverted.
        /// </summary>
        public bool Invert2 { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether input 3/z should be inverted.
        /// </summary>
        public bool Invert3 { get; set; } = false;

        /// <inheritdoc />
        public string Name { get; set; }

        /// <summary>
        /// Processes xelement and creates <see cref="RangeableInput3Description"/>.
        /// </summary>
        /// <param name="node">Node to process.</param>
        /// <returns>New <see cref="RangeableInput3Description"/>.</returns>
        public static RangeableInput3Description FromXElement(XElement node)
        {
            var inputSource = new RangeableInput3Description();

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

                try
                {
                    inputSource.Invert2 = Parsers.MakeBool(hwSettingsNode.Attribute("invert2").Value);
                }
                catch
                {
                    // optional
                    inputSource.Invert2 = false;
                }

                try
                {
                    inputSource.Invert3 = Parsers.MakeBool(hwSettingsNode.Attribute("invert3").Value);
                }
                catch
                {
                    // optional
                    inputSource.Invert3 = false;
                }
            }
            else
            {
                throw new InvalidConfiguration($"Missing inputHandler.item.hwSettings (line: {Parsers.GetNodeLine(node)})");
            }

            return inputSource;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
