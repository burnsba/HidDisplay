using System;
using System.Collections.Generic;
using System.Text;

namespace HidDisplay.SkinModel.HotConfig
{
    /// <summary>
    /// Single setting from the settings.json file. This is the UI item interface.
    /// </summary>
    public interface IConfigSetting : IDisposable
    {
        /// <summary>
        /// Gets or sets settings key name.
        /// </summary>
        string Key { get; }

        /// <summary>
        /// Gets or sets text to show to user.
        /// </summary>
        string Display { get; }

        /// <summary>
        /// Gets or sets the type of input.
        /// </summary>
        InputTypes InputType { get; }

        /// <summary>
        /// Gets or sets the current value.
        /// </summary>
        string CurrentValue { get; set; }
    }
}
