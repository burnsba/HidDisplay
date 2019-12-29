using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;
using HidDisplay.SkinModel.HotConfig;
using HidDisplayDnc.Dto;
using HidDisplayDnc.Mvvm;
using HidDisplayDnc.Windows;

namespace HidDisplayDnc.ViewModels
{
    /// <summary>
    /// View model for application configuration.
    /// </summary>
    public class AppConfigViewModel : WindowViewModelBase
    {
        private string _backgroundColorString = null;
        private bool _backgroundColorStringChange = false;
        private string _pluginsPath = null;
        private bool _pluginsPathChange = false;
        private string _skinsPath = null;
        private bool _skinsPathChange = false;

        private MainViewModel _mainVm;

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
                _skinsPath = value;
                _skinsPathChange = true;
                OnPropertyChanged(nameof(SkinsPath));
            }
        }

        /// <summary>
        /// Gets or sets path to directory containing skins.
        /// </summary>
        public string PluginsPath
        {
            get
            {
                return _pluginsPath;
            }

            set
            {
                _pluginsPath = value;
                _pluginsPathChange = true;
                OnPropertyChanged(nameof(PluginsPath));
            }
        }

        /// <summary>
        /// Gets or sets the main window background color, as a hex code string.
        /// </summary>
        public string BackgroundColorString
        {
            get
            {
                return _backgroundColorString;
            }

            set
            {
                var str = value;

                if (!string.IsNullOrEmpty(str) && str[0] != '#')
                {
                    str = "#" + str;
                }

                _backgroundColorString = str?.ToUpper();
                _backgroundColorStringChange = true;
                OnPropertyChanged(nameof(BackgroundColorString));
            }
        }

        /// <summary>
        /// Gets or sets ok button command.
        /// </summary>
        public ICommand OkCommand { get; set; }

        /// <summary>
        /// Gets or sets cancel button command.
        /// </summary>
        public ICommand CancelCommand { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppConfigViewModel"/> class.
        /// </summary>
        /// <param name="mainVm">Mainview model. Config options are proxied here until saved, at which point the main vm will be updated.</param>
        public AppConfigViewModel(MainViewModel mainVm)
        {
            _mainVm = mainVm;

            _backgroundColorString = mainVm.BackgroundColorString;
            _skinsPath = mainVm.SkinsPath;
            _pluginsPath = ConfigurationManager.AppSettings["PluginsPath"];

            CancelCommand = new RelayCommand<ICloseable>(CloseWindow);

            OkCommand = new RelayCommand<ICloseable>(w =>
            {
                if (SaveChanges())
                {
                    CloseWindow(w);
                }
            });
        }

        /// <summary>
        /// Writes config settings to settings json file.
        /// </summary>
        public bool SaveChanges()
        {
            string originalSkinsPath = ConfigurationManager.AppSettings["SkinsPath"];
            string originalBackgroundColorString = ConfigurationManager.AppSettings["BackgroundColor"];
            string originalPluginsPath = ConfigurationManager.AppSettings["PluginsPath"];

            bool rollback = false;

            Configuration configuration = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);

            try
            {
                try
                {
                    if (_skinsPathChange)
                    {
                        _mainVm.SkinsPath = SkinsPath;
                        configuration.AppSettings.Settings["SkinsPath"].Value = SkinsPath;
                    }
                }
                catch (Exception ex)
                {
                    Workspace.RecreateSingletonWindow<ErrorWindow>(new ErrorWindowViewModel(ex)
                    {
                        HeaderMessage = $"Error saving {nameof(SkinsPath)} back to app.config file",
                    });

                    return false;
                }

                try
                {
                    if (_backgroundColorStringChange)
                    {
                        _mainVm.BackgroundColorString = BackgroundColorString;
                        configuration.AppSettings.Settings["BackgroundColor"].Value = BackgroundColorString;
                    }
                }
                catch (Exception ex)
                {
                    Workspace.RecreateSingletonWindow<ErrorWindow>(new ErrorWindowViewModel(ex)
                    {
                        HeaderMessage = $"Error saving BackgroundColor back to app.config file",
                    });

                    return false;
                }

                try
                {
                    if (_pluginsPathChange)
                    {
                        // no plugins setting in main VM
                        configuration.AppSettings.Settings["PluginsPath"].Value = PluginsPath;
                    }
                }
                catch (Exception ex)
                {
                    Workspace.RecreateSingletonWindow<ErrorWindow>(new ErrorWindowViewModel(ex)
                    {
                        HeaderMessage = $"Error saving {nameof(PluginsPath)} back to app.config file",
                    });

                    return false;
                }

                configuration.Save();
                ConfigurationManager.RefreshSection("appSettings");

                return true;
            }
            finally
            {
                if (rollback)
                {
                    _mainVm.SkinsPath = originalSkinsPath;
                    _mainVm.BackgroundColorString = originalBackgroundColorString;
                    // no plugins setting in main VM

                    ConfigurationManager.AppSettings["SkinsPath"] = originalSkinsPath;
                    ConfigurationManager.AppSettings["BackgroundColor"] = originalBackgroundColorString;
                    ConfigurationManager.AppSettings["PluginsPath"] = originalPluginsPath;
                }
            }
        }
    }
}
