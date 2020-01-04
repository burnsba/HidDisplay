using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using BurnsBac.HotConfig.Error;
using HidDisplay.PluginDefinition;

namespace HidDisplay.SkinModel.Core
{
    /// <summary>
    /// Input handler skin definition and hardware monitor type information.
    /// </summary>
    public class InputHandler : IDisposable
    {
        /// <summary>
        /// Gets or sets input handler description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the event handler.
        /// </summary>
        public IPlugin Handler { get; set; }

        /// <summary>
        /// Gets or sets the assembly name containing hardware event handler.
        /// </summary>
        public string HandlerAssemblyName { get; set; }

        /// <summary>
        /// Gets or sets the type name of hardware event handler.
        /// </summary>
        public string HandlerTypeName { get; set; }

        /// <summary>
        /// Gets or sets list of items to monitor.
        /// </summary>
        public List<InputHandlerItem> Items { get; set; } = new List<InputHandlerItem>();

        /// <summary>
        /// Processes xelement and creates <see cref="InputHandler"/>.
        /// </summary>
        /// <param name="node">Node to process.</param>
        /// <param name="parent">Parent info.</param>
        /// <returns>New <see cref="InputHandler"/>.</returns>
        public static InputHandler FromXElement(XElement node, Skin parent)
        {
            var ih = new InputHandler();

            try
            {
                ih.HandlerTypeName = (string)node.Attribute("handlerType");
            }
            catch (Exception ex)
            {
                throw new InvalidConfiguration($"Could not read inputHandler.handlerType (line: {Parsers.GetNodeLine(node)})", ex);
            }

            try
            {
                ih.HandlerAssemblyName = (string)node.Attribute("handlerAssembly");
            }
            catch (Exception ex)
            {
                throw new InvalidConfiguration($"Could not read inputHandler.handlerAssembly (line: {Parsers.GetNodeLine(node)})", ex);
            }

            try
            {
                ih.Handler = TypeResolver.CreatePluginInstance(ih.HandlerTypeName, ih.HandlerAssemblyName);
            }
            catch (Exception ex)
            {
                var message = $"Could not resolve input handler. HandlerTypeName='{ih.HandlerTypeName}', HandlerAssemblyName='{ih.HandlerAssemblyName}'. (line: {Parsers.GetNodeLine(node)})";
                throw new InvalidConfiguration(message, ex);
            }

            try
            {
                ih.Description = (string)node.Attribute("description");
            }
            catch
            {
                // optional
            }

            var inputHandlerItemsNodes = node.Descendants("item");
            foreach (var inputHandlerItemsNode in inputHandlerItemsNodes)
            {
                ih.Items.Add(InputHandlerItem.FromXElement(inputHandlerItemsNode, parent));
            }

            return ih;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (!object.ReferenceEquals(null, Handler))
            {
                Handler.Dispose();
            }

            if (!object.ReferenceEquals(null, Items) && Items.Any())
            {
                foreach (var x in Items)
                {
                    x.Dispose();
                }
            }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return Description;
        }
    }
}
