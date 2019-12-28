using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace HidDisplay.SkinModel
{
    /// <summary>
    /// Utility class.
    /// </summary>
    public static class Parsers
    {
        /// <summary>
        /// Converts a string to bool.
        /// </summary>
        /// <param name="s">String to parse.</param>
        /// <returns>True, if string contains "true" or "1", false if it contains "false" or "0", and false otherwise.</returns>
        public static bool MakeBool(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return false;
            }

            if (s.IndexOf("true", 0, StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                return true;
            }

            if (s.IndexOf("false", 0, StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                return false;
            }

            if (s.IndexOf("1", 0, StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                return true;
            }

            if (s.IndexOf("0", 0, StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                return false;
            }

            return false;
        }

        /// <summary>
        /// Gets line number for given node. Xml must have been parsed with LoadOptions.SetLineInfo set.
        /// </summary>
        /// <param name="node">Node to get line number from.</param>
        /// <returns>Line number or -1.</returns>
        public static int GetNodeLine(XElement node)
        {
            IXmlLineInfo ix = node;

            if (!object.ReferenceEquals(null, ix))
            {
                if (ix.HasLineInfo())
                {
                    return ix.LineNumber;
                }
            }

            return -1;
        }
    }
}
