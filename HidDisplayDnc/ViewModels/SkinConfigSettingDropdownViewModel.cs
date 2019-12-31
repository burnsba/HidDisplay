﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public class SkinConfigSettingDropdownViewModel : ConfigSettingBase, INotifyPropertyChanged
    {
        private DropdownItem _selectedItem;
        IConfigDataProvider _dataProvider = null;
        private bool _isPoll = false;

        /// <summary>
        /// Property changed event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Property changed notifier.
        /// </summary>
        /// <param name="property">Name of property that changed.</param>
        protected void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        /// <summary>
        /// Gets or sets list of dropdown data items.
        /// </summary>
        public List<DropdownItem> Items { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating whether any poll results have been received yet.
        /// </summary>
        public bool WaitingForFirstPollResult { get; set; } = false;

        /// <summary>
        /// Gets a flag indicating whether the UI should notify the user the application is waiting for results.
        /// </summary>
        public bool ShowScanningInfo
        {
            get
            {
                return WaitingForFirstPollResult == true && _isPoll == true;
            }
        }

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
        /// Initializes a new instance of the <see cref="SkinConfigSettingDropdownViewModel"/> class.
        /// </summary>
        /// <param name="item">Source item.</param>
        public SkinConfigSettingDropdownViewModel(Setting item)
            : base(item)
        {
            try
            {
                _dataProvider = HidDisplay.SkinModel.TypeResolver.CreateConfigDataProviderInstance(item.Datasource, item.DatasourceAssembly);
            }
            catch (Exception ex)
            {
                var message = $"Could not resolve datasource. Datasource='{item.Datasource}', DatasourceAssembly='{item.DatasourceAssembly}'.";
                throw new HidDisplay.SkinModel.Error.InvalidConfiguration(message, ex);
            }

            var dataProviderType = _dataProvider.GetType();

            if (typeof(IConfigDataProviderOnce).IsAssignableFrom(dataProviderType))
            {
                var onceProvider = (IConfigDataProviderOnce)_dataProvider;
                Items = onceProvider.FetchData().Select(x => new DropdownItem() { Id = x.Key, Text = x.Value }).ToList();
            }
            else if (typeof(IConfigDataProviderPoll).IsAssignableFrom(dataProviderType))
            {
                Items = new List<DropdownItem>();

                _isPoll = true;
                WaitingForFirstPollResult = true;

                var pollProvider = (IConfigDataProviderPoll)_dataProvider;
                pollProvider.DataItems.CollectionChanged += DataProviderPollCollectionChanged;

                pollProvider.Start();
            }
            else
            {
                throw new HidDisplay.SkinModel.Error.InvalidConfiguration("Settings dropdown doesn't implement a known interface.");
            }

            SelectedItem = Items.Where(x => x.Id == CurrentValue).FirstOrDefault();
        }

        private void DataProviderPollCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            bool notify = false;

            if (e.NewItems != null && e.NewItems.Count > 0)
            {
                foreach (ConfigSettingItem item in e.NewItems)
                {
                    if (Items.Any(x => string.Compare(x.Id, item.Key, false) == 0))
                    {
                        continue;
                    }

                    WaitingForFirstPollResult = false;
                    Items.Add(new DropdownItem() { Id = item.Key, Text = item.Display });
                    notify = true;
                }
            }

            if (e.OldItems != null && e.OldItems.Count > 0)
            {
                foreach (ConfigSettingItem item in e.OldItems)
                {
                    if (Items.RemoveAll(x => string.Compare(x.Id, item.Key, false) == 0) > 0)
                    {
                        notify = true;
                    }
                }
            }

            if (notify)
            {
                OnPropertyChanged(nameof(Items));
                OnPropertyChanged(nameof(WaitingForFirstPollResult));
                OnPropertyChanged(nameof(ShowScanningInfo));
            }
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            var dataProviderType = _dataProvider.GetType();

            if (typeof(IConfigDataProviderPoll).IsAssignableFrom(dataProviderType))
            {
                var pollProvider = (IConfigDataProviderPoll)_dataProvider;

                pollProvider.Stop();
                pollProvider.Dispose();
            }
        }
    }
}
