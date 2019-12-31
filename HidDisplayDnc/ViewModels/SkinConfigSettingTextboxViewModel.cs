﻿using System;
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
    public class SkinConfigSettingTextboxViewModel : ConfigSettingBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SkinConfigSettingTextboxViewModel"/> class.
        /// </summary>
        /// <param name="item">Source item.</param>
        public SkinConfigSettingTextboxViewModel(Setting item)
            : base(item)
        {

        }

        /// <inheritdoc />
        public override void Dispose()
        {
            // nothing to do.
        }
    }
}
