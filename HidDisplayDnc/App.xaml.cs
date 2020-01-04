using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using BurnsBac.WindowsAppToolkit;
using BurnsBac.WindowsAppToolkit.ViewModels;
using BurnsBac.WindowsAppToolkit.Windows;

namespace HidDisplayDnc
{
    /// <summary>
    /// Interaction logic for App.xaml .
    /// </summary>
    public partial class App : Application
    {
        private void App_Startup(object sender, StartupEventArgs e)
        {
            // I was going to make this configurable, but there are possible issues with multiple conflicting types
            // existing in the CLR at the same time, I don't want to sort that out now.
            HidDisplay.SkinModel.TypeResolver.PluginsDirectory = ConfigurationManager.AppSettings["PluginsPath"];

            // Make sure the config window know where to find type information
            BurnsBac.HotConfig.TypeResolver.ConfigDataProvidersDirectory = ConfigurationManager.AppSettings["PluginsPath"];

            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            // Hook general uncaught exception events to show user. (this won't help with some of the CLR/pinvoke errors that might occur)
            Dispatcher.UnhandledException += (s, e) =>
            {
                e.Handled = true;
                ShowUnhandledException(e.Exception, "Dispatcher.UnhandledException");
            };

            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                e.SetObserved();
                ShowUnhandledException(e.Exception, "TaskScheduler.UnobservedTaskException");
            };
        }

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            // Ignore missing resources
            if (args.Name.Contains(".resources"))
            {
                return null;
            }

            var names = AppDomain.CurrentDomain.GetAssemblies().Select(x => x.FullName).OrderBy(x => x).ToList();

            // check for assemblies already loaded
            Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName == args.Name);
            if (assembly != null)
            {
                return assembly;
            }

            return null;
        }

        /// <summary>
        /// Shows error window for uncaught exceptions. Closes application once
        /// the error window is closed.
        /// </summary>
        /// <param name="ex">Exception to display.</param>
        /// <param name="source">Source of exception.</param>
        private void ShowUnhandledException(Exception ex, string source)
        {
            var ewvm = new ErrorWindowViewModel($"Unhandled exception in application: {source}", ex)
            {
                ExitOnClose = true,
            };

            Workspace.RecreateSingletonWindow<ErrorWindow>(ewvm);
        }
    }
}
