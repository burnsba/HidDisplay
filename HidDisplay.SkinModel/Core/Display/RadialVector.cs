using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using BurnsBac.HotConfig.Error;

namespace HidDisplay.SkinModel.Core.Display
{
    /// <summary>
    /// Used for something like a 2d joystick around a central point.
    /// Should always be displayed until hidden.
    /// </summary>
    public class RadialVector : IUiImageItem
    {
        /// <inheritdoc />
        public ImageInfo Image { get; set; }

        /// <summary>
        /// Gets or sets radial factor (multiplicative).
        /// </summary>
        public double RadialFactor { get; set; }

        /// <summary>
        /// Gets or sets the max value of the scale, to build scale factor.
        /// </summary>
        public double ScaleMax { get; set; } = double.NaN;

        /// <summary>
        /// Gets or sets the min value of the scale, to build scale factor.
        /// </summary>
        public double ScaleMin { get; set; } = double.NaN;

        /// <summary>
        /// Gets or sets scale factor norm (multiplicative).
        /// </summary>
        public double ScaleNorm { get; set; } = double.NaN;

        /// <summary>
        /// Gets or sets value to slide item out from radius (multiplicative).
        /// </summary>
        public double SlideFactor { get; set; } = double.NaN;

        /// <summary>
        /// Gets or sets max sliding value. This takes priority over scaling.
        /// </summary>
        public double SlideMax { get; set; } = double.NaN;

        /// <summary>
        /// Gets or sets a value indicating whether or not to use the scale factor.
        /// </summary>
        public bool UseScale { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether or not to use the sliding factor.
        /// </summary>
        public bool UseSlide { get; set; } = false;

        /// <summary>
        /// Processes xelement and creates <see cref="RadialVector"/>.
        /// </summary>
        /// <param name="node">Node to process.</param>
        /// <param name="parent">Parent info.</param>
        /// <returns>New <see cref="RadialVector"/>.</returns>
        public static RadialVector FromXElement(XElement node, Skin parent)
        {
            var rv = new RadialVector();

            var uiSettingsNode = node.Descendants("uiSettings").FirstOrDefault();
            if (!object.ReferenceEquals(null, uiSettingsNode))
            {
                rv.Image = ImageInfo.FromXElement(uiSettingsNode, parent.AbsoluteContainerPath);

                try
                {
                    rv.RadialFactor = (int)uiSettingsNode.Attribute("radialFactor");
                }
                catch (Exception ex)
                {
                    throw new InvalidConfiguration($"Could not read inputHandler.item.uiSettings.radialFactor (line: {Parsers.GetNodeLine(uiSettingsNode)})", ex);
                }

                try
                {
                    rv.ScaleMin = (double)uiSettingsNode.Attribute("scaleMin");
                }
                catch (Exception ex)
                {
                    throw new InvalidConfiguration($"Could not read inputHandler.item.uiSettings.scaleMin (line: {Parsers.GetNodeLine(uiSettingsNode)})", ex);
                }

                try
                {
                    rv.ScaleMax = (double)uiSettingsNode.Attribute("scaleMax");
                }
                catch (Exception ex)
                {
                    throw new InvalidConfiguration($"Could not read inputHandler.item.uiSettings.scaleMax (line: {Parsers.GetNodeLine(uiSettingsNode)})", ex);
                }

                try
                {
                    rv.ScaleNorm = (double)uiSettingsNode.Attribute("scaleNorm");
                }
                catch (Exception ex)
                {
                    throw new InvalidConfiguration($"Could not read inputHandler.item.uiSettings.scaleNorm (line: {Parsers.GetNodeLine(uiSettingsNode)})", ex);
                }

                try
                {
                    rv.UseSlide = Parsers.MakeBool(uiSettingsNode.Attribute("useSlide").Value);
                }
                catch
                {
                    // optional
                }

                try
                {
                    rv.UseScale = Parsers.MakeBool(uiSettingsNode.Attribute("useScale").Value);
                }
                catch
                {
                    // optional
                }

                try
                {
                    rv.SlideFactor = (double)uiSettingsNode.Attribute("slideFactor");
                }
                catch
                {
                    // optional
                }

                try
                {
                    rv.SlideMax = (double)uiSettingsNode.Attribute("slideMax");
                }
                catch
                {
                    // optional
                }
            }
            else
            {
                throw new InvalidConfiguration($"Missing inputHandler.item.uiSettings (line: {Parsers.GetNodeLine(node)})");
            }

            return rv;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (!object.ReferenceEquals(null, Image))
            {
                Image.Dispose();
            }
        }
    }
}