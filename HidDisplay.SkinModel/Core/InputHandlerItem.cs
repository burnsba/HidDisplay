using System;
using System.Linq;
using System.Xml.Linq;
using BurnsBac.HotConfig.Error;
using HidDisplay.PluginDefinition;
using HidDisplay.SkinModel.InputSourceDescription;

namespace HidDisplay.SkinModel.Core
{
    /// <summary>
    /// Skin definition of single item to monitor.
    /// </summary>
    public class InputHandlerItem : IDisposable
    {
        /// <summary>
        /// Gets or sets skin defition of what to monitor for.
        /// </summary>
        public IInputSourceDescription Hw { get; set; }

        /// <summary>
        /// Gets or sets input event type (what comes from the <see cref="GenericInputEventArgs" />).
        /// </summary>
        public Type HwType { get; set; }

        /// <summary>
        /// Gets or sets the hardware event type name.
        /// </summary>
        public string HwTypeName { get; set; }

        /// <summary>
        /// Gets or sets the friendly name of the item being monitored.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets wpf ui element.
        /// </summary>
        public IUiItem Ui { get; set; }

        /// <summary>
        /// Gets or sets ui display type.
        /// </summary>
        public UiType UiType { get; set; }

        /// <summary>
        /// Processes xelement and creates <see cref="InputHandlerItem"/>.
        /// </summary>
        /// <param name="node">Node to process.</param>
        /// <param name="parent">Parent info.</param>
        /// <returns>New <see cref="InputHandlerItem"/>.</returns>
        public static InputHandlerItem FromXElement(XElement node, Skin parent)
        {
            var ihi = new InputHandlerItem();

            try
            {
                ihi.UiType = (UiType)Enum.Parse(typeof(UiType), node.Attribute("uiType").Value, true);

                if (object.ReferenceEquals(null, ihi.UiType))
                {
                    throw new InvalidConfiguration($"Could not read inputHandler.item.uiType (line: {Parsers.GetNodeLine(node)})");
                }
            }
            catch (InvalidConfiguration)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidConfiguration($"Could not read inputHandler.item.uiType (line: {Parsers.GetNodeLine(node)})", ex);
            }

            try
            {
                ihi.HwTypeName = (string)node.Attribute("hwType").Value;
            }
            catch (Exception ex)
            {
                throw new InvalidConfiguration($"Could not read inputHandler.item.hwType (line: {Parsers.GetNodeLine(node)})", ex);
            }

            try
            {
                ihi.HwType = TypeResolver.GetInputSourceType(ihi.HwTypeName);

                if (object.ReferenceEquals(null, ihi.HwType))
                {
                    throw new InvalidConfiguration($"Could not load type (check case): {ihi.HwTypeName} (line: {Parsers.GetNodeLine(node)})");
                }
            }
            catch (InvalidConfiguration)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidConfiguration($"Could not load type (check case): {ihi.HwTypeName} (line: {Parsers.GetNodeLine(node)})", ex);
            }

            try
            {
                ihi.Name = (string)node.Attribute("name");
            }
            catch
            {
                // optional
            }

            var uiSettingsNode = node.Descendants("uiSettings").FirstOrDefault();
            if (!object.ReferenceEquals(null, uiSettingsNode))
            {
                ihi.Ui = TypeResolver.CreateUiItemFromXElement(ihi.UiType, node, parent);
            }
            else
            {
                throw new InvalidConfiguration($"Missing inputHandler.item.uiSettings (line: {Parsers.GetNodeLine(node)})");
            }

            var hwSettingsNode = node.Descendants("hwSettings").FirstOrDefault();
            if (!object.ReferenceEquals(null, hwSettingsNode))
            {
                ihi.Hw = TypeResolver.CreateInputSourceFromXElement(ihi.HwType, node, parent);
            }
            else
            {
                throw new InvalidConfiguration($"Missing inputHandler.item.uiSettings (line: {Parsers.GetNodeLine(node)})");
            }

            return ihi;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (!object.ReferenceEquals(null, Ui))
            {
                Ui.Dispose();
            }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return Name;
        }
    }
}
