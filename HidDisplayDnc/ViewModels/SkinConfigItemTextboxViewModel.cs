using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HidDisplay.SkinModel.HotConfig;
using HidDisplayDnc.Dto;

namespace HidDisplayDnc.ViewModels
{
    /// <summary>
    /// View model for skin config textbox setting.
    /// </summary>
    public class SkinConfigItemTextboxViewModel : SkinConfigItemBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SkinConfigItemTextboxViewModel"/> class.
        /// </summary>
        /// <param name="item">Source item.</param>
        public SkinConfigItemTextboxViewModel(SettingsItem item)
            : base(item)
        {

        }
    }
}
