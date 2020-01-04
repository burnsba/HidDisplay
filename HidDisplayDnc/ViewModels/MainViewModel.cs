using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;
using BurnsBac.WindowsAppToolkit;
using HidDisplay.SkinModel.Core;
using BurnsBac.WindowsAppToolkit.Mvvm;
using HidDisplayDnc.Windows;
using BurnsBac.WindowsHardware.HardwareWatch.Enums;
using HidDisplay.SkinModel;

namespace HidDisplayDnc.ViewModels
{
    /// <summary>
    /// Main window view model.
    /// </summary>
    public class MainViewModel : WindowViewModelBase
    {
        private Skin _activeSkin = null;
        private string _skinsPath = null;
        private AvailableSkinViewModel _availableSkinViewModel = null;
        private SolidColorBrush _backgroundColor = new SolidColorBrush(Color.FromRgb(byte.MaxValue, byte.MaxValue, byte.MaxValue));
        private string _backgroundColorString = null;
        private ObservableCollection<AvailableSkinViewModel> _availableSkins = new ObservableCollection<AvailableSkinViewModel>();

        /// <summary>
        /// Gets list of available skins.
        /// </summary>
        public ObservableCollection<AvailableSkinViewModel> AvailableSkins
        {
            get
            {
                return _availableSkins;
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

        /// <summary>
        /// Gets or sets the command to show the application configuration window.
        /// </summary>
        public ICommand ShowAppConfigWindowCommand { get; set; }

        /// <summary>
        /// Gets or sets path to directory containing skins.
        /// </summary>
        public string SkinsPath
        {
            get
            {
                return _skinsPath;
            }

            set
            {
                if (string.Compare(_skinsPath, value, false) == 0)
                {
                    return;
                }

                _skinsPath = value;

                ReloadSkins();

                OnPropertyChanged(nameof(SkinsPath));
                OnPropertyChanged(nameof(AvailableSkins));
            }
        }

        /// <summary>
        /// Gets or sets the main window background color.
        /// </summary>
        public SolidColorBrush BackgroundColor
        {
            get
            {
                return _backgroundColor;
            }

            set
            {
                _backgroundColor = value;
                _backgroundColorString = $"#{value.Color.R.ToString("X2")}{value.Color.G.ToString("X2")}{value.Color.B.ToString("X2")}";
                OnPropertyChanged(nameof(BackgroundColor));
                OnPropertyChanged(nameof(BackgroundColorString));
            }
        }

        /// <summary>
        /// Gets or sets the main window background color, as a hex string.
        /// </summary>
        public string BackgroundColorString
        {
            get
            {
                return _backgroundColorString;
            }

            set
            {
                _backgroundColorString = value;
                _backgroundColor = (SolidColorBrush)(new BrushConverter().ConvertFrom(value));
                OnPropertyChanged(nameof(BackgroundColorString));
                OnPropertyChanged(nameof(BackgroundColor));
            }
        }

        public MainViewModel()
        {
            ReadConfig();

            ShowAppConfigWindowCommand = new CommandHandler(() => Workspace.CreateSingletonWindow<AppConfigWindow>(this));
        }

        /// <summary>
        /// Reads app.config and sets global settings. Creates list of available skins.
        /// </summary>
        private void ReadConfig()
        {
            // Settings SkinsPath triggers loading skins, skip that for just a moment.
            _skinsPath = ConfigurationManager.AppSettings["SkinsPath"];
            BackgroundColorString = ConfigurationManager.AppSettings["BackgroundColor"];

            ReloadSkins();
        }

        private void ReloadSkins()
        {
            // Need an absolute path to iterate over the sub directories, but if
            // the user supplied a relative path in app.config, don't change it.
            var skinPath = Path.GetFullPath(SkinsPath);
            var dirs = Directory.EnumerateDirectories(skinPath);

            _availableSkins.Clear();

            var seen = new HashSet<string>();

            foreach (var dirname in dirs)
            {
                var dirpath = Path.Combine(skinPath, dirname);
                var xmlfile = Path.Combine(dirpath, Constants.SkinDefinitionFilename);

                if (File.Exists(xmlfile))
                {
                    string fileHash;

                    using (FileStream stream = File.OpenRead(xmlfile))
                    {
                        var sha = new SHA256Managed();
                        byte[] checksum = sha.ComputeHash(stream);
                        fileHash = BitConverter.ToString(checksum).Replace("-", String.Empty);
                    }

                    if (seen.Contains(fileHash))
                    {
                        continue;
                    }

                    seen.Add(fileHash);

                    var xmlraw = File.ReadAllText(xmlfile);
                    var skin = Skin.InfoFromXmlRaw(xmlraw, dirpath);

                    var availableSkin = new AvailableSkinViewModel()
                    {
                        DisplayName = skin.DisplayName,
                        SkinDirectoryPath = dirpath,
                        DirectoryContainerName = dirname,
                        SkinXmlPath = xmlfile,
                        Sha256 = fileHash,
                    };

                    _availableSkins.Add(availableSkin);
                }
            }
        }
    }
}
