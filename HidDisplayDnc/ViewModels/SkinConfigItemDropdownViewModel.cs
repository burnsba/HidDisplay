using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HidDisplay.SkinModel.HotConfig;
using HidDisplay.SkinModel.HotConfig.DataSource;
using HidDisplayDnc.Dto;

namespace HidDisplayDnc.ViewModels
{
    /// <summary>
    /// View model for skin config dropdown setting.
    /// </summary>
    public class SkinConfigItemDropdownViewModel : SkinConfigItemBase
    {
        private DropdownItem _selectedItem;

        /// <summary>
        /// Gets or sets list of dropdown data items.
        /// </summary>
        public List<DropdownItem> Items { get; set; }

        /// <summary>
        /// Gets or sets currently selected data item.
        /// </summary>
        public DropdownItem SelectedItem
        {
            get
            {
                return _selectedItem;
            }

            set
            {
                _selectedItem = value;
                if (!object.ReferenceEquals(null, _selectedItem))
                {
                    _settingsItem.CurrentValue = _selectedItem.Id;
                }
                else
                {
                    _settingsItem.CurrentValue = string.Empty;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkinConfigItemDropdownViewModel"/> class.
        /// </summary>
        /// <param name="item">Source item.</param>
        public SkinConfigItemDropdownViewModel(SettingsItem item)
            : base(item)
        {
            IConfigDataProvider dataProvider = null;

            try
            {
                dataProvider = HidDisplay.SkinModel.TypeResolver.CreateConfigDataProviderInstance(item.Datasource, item.DatasourceAssembly);
            }
            catch (Exception ex)
            {
                var message = $"Could not resolve datasource. Datasource='{item.Datasource}', DatasourceAssembly='{item.DatasourceAssembly}'.";
                throw new HidDisplay.SkinModel.Error.InvalidConfiguration(message, ex);
            }

            Items = dataProvider.FetchData().Select(x => new DropdownItem() { Id = x.Key, Text = x.Value }).ToList();

            SelectedItem = Items.Where(x => x.Id == CurrentValue).FirstOrDefault();
        }
    }
}
