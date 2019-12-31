using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using HidDisplay.SkinModel.Error;

namespace HidDisplay.SkinModel.Core
{
    /// <summary>
    /// Describes text information.
    /// </summary>
    public class TextInfo : IPositionable
    {
        /// <summary>
        /// Default font to use if the user doesn't supply one.
        /// </summary>
        private const string DefaultFontIfNotFound = "Segoe UI";

        /// <summary>
        /// Gets or sets the x offfset used to display the text in the main area.
        /// </summary>
        public int XOffset { get; set; }

        /// <summary>
        /// Gets or sets the y offset used to display the text in the main area.
        /// </summary>
        public int YOffset { get; set; }

        /// <summary>
        /// Gets or sets the font size to display text.
        /// </summary>
        public int FontSize { get; set; }
        
        /// <summary>
        /// Gets or sets name of the font to display text.
        /// </summary>
        public string Font { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating whether bold styling should be applied to the font.
        /// </summary>
        public bool IsBold { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating whether italic styling should be applied to the font.
        /// </summary>
        public bool IsItalic { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating whether underline styling should be applied to the font.
        /// </summary>
        public bool IsUnderline { get; set; }

        /// <summary>
        /// Gets or sets optional parameters to pass to ToString.
        /// </summary>
        public string ToStringFormatParameters { get; set; }

        /// <summary>
        /// Processes xelement and creates <see cref="TextInfo"/>.
        /// </summary>
        /// <param name="node">Node to process.</param>
        /// <param name="containingDirectory">Parent directory containing skin.</param>
        /// <returns>New <see cref="TextInfo"/>.</returns>
        public static TextInfo FromXElement(XElement node, string containingDirectory)
        {
            var text = new TextInfo();

            if (node.Attribute("xOffset") != null
                && !string.IsNullOrEmpty(node.Attribute("xOffset").Value))
            {
                text.XOffset = (int)node.Attribute("xOffset");
            }
            else
            {
                text.XOffset = 0;
            }

            if (node.Attribute("yOffset") != null
                && !string.IsNullOrEmpty(node.Attribute("yOffset").Value))
            {
                text.YOffset = (int)node.Attribute("yOffset");
            }
            else
            {
                text.YOffset = 0;
            }

            if (node.Attribute("fontSize") != null
                && !string.IsNullOrEmpty(node.Attribute("fontSize").Value))
            {
                text.FontSize = (int)node.Attribute("fontSize");
            }
            else
            {
                text.FontSize = 14;
            }

            if (node.Attribute("font") != null
                && !string.IsNullOrEmpty(node.Attribute("font").Value))
            {
                text.Font = (string)node.Attribute("font");
            }
            else
            {
                text.Font = DefaultFontIfNotFound;
            }

            if (node.Attribute("isBold") != null
                && !string.IsNullOrEmpty(node.Attribute("isBold").Value))
            {
                text.IsBold = Parsers.MakeBool(node.Attribute("isBold").ToString());
            }
            else
            {
                text.IsBold = false;
            }

            if (node.Attribute("isItalic") != null
                && !string.IsNullOrEmpty(node.Attribute("isItalic").Value))
            {
                text.IsItalic = Parsers.MakeBool(node.Attribute("isItalic").ToString());
            }
            else
            {
                text.IsItalic = false;
            }

            if (node.Attribute("isUnderline") != null
                && !string.IsNullOrEmpty(node.Attribute("isUnderline").Value))
            {
                text.IsUnderline = Parsers.MakeBool(node.Attribute("isUnderline").ToString());
            }
            else
            {
                text.IsUnderline = false;
            }

            if (node.Attribute("toStringFormatParameters") != null
                && !string.IsNullOrEmpty(node.Attribute("toStringFormatParameters").Value))
            {
                text.ToStringFormatParameters = (string)node.Attribute("toStringFormatParameters");
            }
            else
            {
                text.ToStringFormatParameters = string.Empty;
            }

            return text;
        }
    }
}
