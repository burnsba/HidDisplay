using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using HidDisplay.SkinModel;
using HidDisplay.SkinModel.Core;
using WindowsHardwareWatch.HardwareWatch.Enums;

namespace HidDisplayDnc.ViewModels
{
    /// <summary>
    /// Main window view model.
    /// </summary>
    public class MainViewModel : BaseViewModel
    {
        private Skin _activeSkin = null;
        private AvailableSkinViewModel _availableSkinViewModel = null;

        /// <summary>
        /// Gets list of available skins.
        /// </summary>
        public List<AvailableSkinViewModel> AvailableSkins
        {
            get
            {
                return Context.AvailableSkins;
            }
        }

        /// <summary>
        /// Gets or sets currently selected skin.
        /// </summary>
        public AvailableSkinViewModel SelectedSkin
        {
            get
            {
                return _availableSkinViewModel;
            }

            set
            {
                _availableSkinViewModel = value;
                OnPropertyChanged(nameof(SelectedSkin));
                OnPropertyChanged(nameof(CanLoadSkin));
                OnPropertyChanged(nameof(CanConfigSkin));
                OnPropertyChanged(nameof(CanUnloadSkin));
            }
        }

        /// <summary>
        /// Gets or sets currently active skin.
        /// </summary>
        public Skin ActiveSkin
        {
            get
            {
                return _activeSkin;
            }

            set
            {
                _activeSkin = value;
                OnPropertyChanged(nameof(ActiveSkin));
                OnPropertyChanged(nameof(CanLoadSkin));
                OnPropertyChanged(nameof(CanConfigSkin));
                OnPropertyChanged(nameof(CanUnloadSkin));
            }
        }

        /// <summary>
        /// Gets a value indicating whether skin can be loaded.
        /// Can only load a skin if no skin is currently loaded.
        /// </summary>
        public bool CanLoadSkin
        {
            get
            {
                return !(object.ReferenceEquals(null, SelectedSkin));
            }
        }

        /// <summary>
        /// Gets a value indicating whether skin can be configured.
        /// Can only congfigure a selected skin that isn't loaded.
        /// </summary>
        public bool CanConfigSkin
        {
            get
            {
                return object.ReferenceEquals(null, ActiveSkin) && !(object.ReferenceEquals(null, SelectedSkin));
            }
        }

        /// <summary>
        /// Gets a value indicating whether skin can be unloaded.
        /// Can only unload a skin if one is currently loaded.
        /// </summary>
        public bool CanUnloadSkin
        {
            get
            {
                return !(object.ReferenceEquals(null, ActiveSkin));
            }
        }
    }
}
