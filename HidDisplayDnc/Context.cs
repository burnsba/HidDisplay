using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using HidDisplay.SkinModel;
using HidDisplay.SkinModel.Core;
using HidDisplay.SkinModel.Core.Display;
using HidDisplayDnc.ViewModels;
using WindowsHardwareWatch.HardwareWatch.Enums;

namespace HidDisplayDnc
{
    /// <summary>
    /// App context.
    /// </summary>
    public class Context
    {
        /// <summary>
        /// Gets or sets directory containing skins.
        /// </summary>
        public static string SkinPath { get; set; }

        /// <summary>
        /// Gets or sets list of available skins.
        /// </summary>
        public static List<AvailableSkinViewModel> AvailableSkins { get; set; } = new List<AvailableSkinViewModel>();

        /// <summary>
        /// Creates a new window, with arguments. If a window with the same type
        /// already exists, it will be shown instead.
        /// </summary>
        /// <typeparam name="T">Type of window to create/show.</typeparam>
        /// <param name="constructorArgs">Arguments to pass to constructor.</param>
        public static void CreateSingletonChildWindow<T>(params object[] constructorArgs)
        {
            if (!(typeof(System.Windows.Window).IsAssignableFrom(typeof(T))))
            {
                throw new ArgumentException($"{typeof(T).FullName} must be System.Windows.Window");
            }

            foreach (System.Windows.Window w in App.Current.Windows)
            {
                if (w.GetType() == typeof(T))
                {
                    w.Show();
                    w.Activate();
                    return;
                }
            }

            System.Windows.Window window = (System.Windows.Window)Activator.CreateInstance(typeof(T), constructorArgs);

            window.Show();
        }

        /// <summary>
        /// Reads app.config and sets global settings. Creates list of available skins.
        /// </summary>
        public static void ReadConfig()
        {
            SkinPath = ConfigurationManager.AppSettings["SkinsPath"];

            TypeResolver.PluginsDirectory = ConfigurationManager.AppSettings["PluginsPath"];

            var dirs = Directory.EnumerateDirectories(SkinPath);

            foreach (var dirname in dirs)
            {
                var dirpath = Path.Combine(SkinPath, dirname);
                var xmlfile = Path.Combine(dirpath, HidDisplay.SkinModel.Constants.SkinDefinitionFilename);

                if (File.Exists(xmlfile))
                {
                    var xmlraw = File.ReadAllText(xmlfile);
                    var skin = Skin.InfoFromXmlRaw(xmlraw, dirpath);

                    var availableSkin = new AvailableSkinViewModel()
                    {
                        DisplayName = skin.DisplayName,
                        SkinDirectoryPath = dirpath,
                        DirectoryContainerName = dirname,
                        SkinXmlPath = xmlfile,
                    };

                    AvailableSkins.Add(availableSkin);
                }
            }
        }
    }
}
