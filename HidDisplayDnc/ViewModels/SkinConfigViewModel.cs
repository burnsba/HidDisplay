using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HidDisplay.SkinModel.HotConfig;
using HidDisplayDnc.Dto;

namespace HidDisplayDnc.ViewModels
{
    /// <summary>
    /// View model for skin config window.
    /// </summary>
    public class SkinConfigViewModel
    {
        private Settings _settingSource = null;

        /// <summary>
        /// Gets or sets list of settings items.
        /// </summary>
        public List<ISkinConfigItem> SettingItems { get; set; }

        /// <summary>
        /// Gets or sets skin currently being configured.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkinConfigViewModel"/> class.
        /// </summary>
        /// <param name="askv">Source info.</param>
        public SkinConfigViewModel(AvailableSkinViewModel askv)
        {
            DisplayName = askv.DisplayName;
            _settingSource = Settings.FromFile(System.IO.Path.Combine(askv.SkinDirectoryPath, HidDisplay.SkinModel.Constants.SkinSettingsFilename));

            if (!object.ReferenceEquals(null, _settingSource))
            {
                try
                {
                    SettingItems = _settingSource.Items.Select(x => SettingsItemConverter(x)).ToList();
                }
                catch (Exception ex)
                {
                    SettingItems = new List<ISkinConfigItem>();

                    Context.CreateSingletonChildWindow<ErrorWindow>(new ErrorWindowViewModel(ex)
                    {
                        FriendlyMessage = "Error loading config settings",
                    });

                    return;
                }
            }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return DisplayName;
        }

        /// <summary>
        /// Writes config settings to settings json file.
        /// </summary>
        public void SaveChanges()
        {
            foreach (var uiitem in SettingItems)
            {
                var settingItem = _settingSource.Items.Where(x => x.Key == uiitem.Key).First();

                settingItem.CurrentValue = uiitem.CurrentValue ?? string.Empty;
            }

            _settingSource.SaveChanges();
        }

        /// <summary>
        /// Converts a json setting item to better typed object.
        /// </summary>
        /// <param name="item">Item to convert.</param>
        /// <returns>Config item.</returns>
        private ISkinConfigItem SettingsItemConverter(SettingsItem item)
        {
            InputTypes type = InputTypesConverter.StringToInputTypes(item.Input);

            if (type == InputTypes.Textbox)
            {
                return new SkinConfigItemTextboxViewModel(item);
            }
            else if (type == InputTypes.Dropdown)
            {
                return new SkinConfigItemDropdownViewModel(item);
            }
            else
            {
                throw new NotSupportedException();
            }
        }
    }
}
