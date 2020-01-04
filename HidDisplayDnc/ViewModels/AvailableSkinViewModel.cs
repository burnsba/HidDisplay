using System;
using System.Collections.Generic;
using System.Text;
using BurnsBac.WindowsAppToolkit.Mvvm;

namespace HidDisplayDnc.ViewModels
{
    /// <summary>
    /// Information about available skins.
    /// </summary>
    public class AvailableSkinViewModel : ViewModelBase
    {
        /// <summary>
        /// Gets or sets hash of source file.
        /// </summary>
        public string Sha256 { get; set; }

        /// <summary>
        /// Gets or sets display name to show.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets name of directory containing skin.
        /// </summary>
        public string DirectoryContainerName { get; set; }

        /// <summary>
        /// Gets or sets absolute path of directory containing skin.
        /// </summary>
        public string SkinDirectoryPath { get; set; }

        /// <summary>
        /// Gets or sets absolute path of skin xml file.
        /// </summary>
        public string SkinXmlPath { get; set; }

        /// <inheritdoc />
        public override string ToString()
        {
            return DisplayName;
        }
    }
}
