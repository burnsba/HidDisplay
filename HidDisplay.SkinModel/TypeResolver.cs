using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using HidDisplay.PluginDefinition;
using HidDisplay.SkinModel.Core;
using HidDisplay.SkinModel.Core.Display;
using HidDisplay.SkinModel.Error;
using HidDisplay.SkinModel.HotConfig.DataSource;
using HidDisplay.SkinModel.InputSourceDescription;

namespace HidDisplay.SkinModel
{
    /// <summary>
    /// Core of this library. Instantiates correct hw/ui types from skin xml. Resolves
    /// data sources from skin config json.
    /// </summary>
    public static class TypeResolver
    {
        private const string InputSourceAssemblyName = "HidDisplay.PluginDefinition";

        /// <summary>
        /// List of types of known input handlers.
        /// </summary>
        private static List<Type> _eventSourceTypes = new List<Type>();

        /// <summary>
        /// List of types of known config data providers.
        /// </summary>
        private static List<Type> _configDataProviderTypes = new List<Type>();

        /// <summary>
        /// Whether or not plugins have already been loaded.
        /// </summary>
        private static bool _pluginsLoaded = false;

        /// <summary>
        /// Whether or not data providers have already been loaded.
        /// </summary>
        private static bool _configDataProvidersLoaded = false;

        /// <summary>
        /// Parent assembly for the input source types.
        /// </summary>
        private static Assembly _inputSourceAssembly = null;

        /// <summary>
        /// Gets the parent assembly for the input source types (<see cref="Button2"/>, etc).
        /// </summary>
        public static Assembly InputSourceAssembly
        {
            get
            {
                if (object.ReferenceEquals(null, _inputSourceAssembly))
                {
                    _inputSourceAssembly = typeof(Button2).Assembly;
                }

                return _inputSourceAssembly;
            }
        }

        /// <summary>
        /// Gets or sets directory to load assemblies from.
        /// </summary>
        public static string PluginsDirectory { get; set; }

        /// <summary>
        /// Converts a type name of an input source into a type.
        /// </summary>
        /// <param name="inputSourceTypeName"></param>
        /// <returns></returns>
        public static Type GetInputSourceType(string inputSourceTypeName)
        {
            var fullname = inputSourceTypeName;
            if (!fullname.StartsWith(TypeResolver.InputSourceAssemblyName))
            {
                fullname = InputSourceAssemblyName + "." + fullname;
            }

            return InputSourceAssembly.GetType(fullname);
        }

        /// <summary>
        /// Resolves type name and assembly name to a type from the list
        /// of known event source types.
        /// </summary>
        /// <param name="shortTypeName">Type name without assembly or version.</param>
        /// <param name="assemblyName">Name of hosting assembly.</param>
        /// <returns>Type. First() is called, so this will throw an exception if not found.</returns>
        public static Type GetPluginType(string shortTypeName, string assemblyName)
        {
            LoadPlugins();

            return _eventSourceTypes
                .Where(x =>
                    x.Assembly.FullName.IndexOf(assemblyName, 0, StringComparison.OrdinalIgnoreCase) >= 0
                    && x.FullName.IndexOf(shortTypeName, 0, StringComparison.OrdinalIgnoreCase) >= 0)
                .First();
        }

        /// <summary>
        /// Resolves type name and assembly name to a type from the list
        /// of known data provider types.
        /// </summary>
        /// <param name="shortTypeName">Type name without assembly or version.</param>
        /// <param name="assemblyName">Name of hosting assembly.</param>
        /// <returns>Type. First() is called, so this will throw an exception if not found.</returns>
        public static Type GetConfigDataProviderType(string shortTypeName, string assemblyName)
        {
            LoadConfigDataProviders();

            return _configDataProviderTypes
                .Where(x =>
                    x.Assembly.FullName.IndexOf(assemblyName, 0, StringComparison.OrdinalIgnoreCase) >= 0
                    && x.FullName.IndexOf(shortTypeName, 0, StringComparison.OrdinalIgnoreCase) >= 0)
                .First();
        }

        /// <summary>
        /// Creates instance of plugin event source.
        /// </summary>
        /// <param name="shortTypeName">Type name without assembly or version.</param>
        /// <param name="assemblyName">Name of hosting assembly.</param>
        /// <returns>New instance of plugin.</returns>
        public static IPlugin CreatePluginInstance(string shortTypeName, string assemblyName)
        {
            LoadPlugins();

            var type = GetPluginType(shortTypeName, assemblyName);
            return (IPlugin)Activator.CreateInstance(type);
        }

        /// <summary>
        /// Creates instance of skin data providor source.
        /// </summary>
        /// <param name="shortTypeName">Type name without assembly or version.</param>
        /// <param name="assemblyName">Name of hosting assembly.</param>
        /// <returns>New instance of data providor.</returns>
        public static IConfigDataProvider CreateConfigDataProviderInstance(string shortTypeName, string assemblyName)
        {
            LoadConfigDataProviders();

            var type = GetConfigDataProviderType(shortTypeName, assemblyName);
            return (IConfigDataProvider)Activator.CreateInstance(type);
        }

        /// <summary>
        /// Resolves parameters to a ui item.
        /// </summary>
        /// <param name="uiType">Type of item to create.</param>
        /// <param name="itemNode">Base node containing item information.</param>
        /// <param name="parent">Associate parent.</param>
        /// <returns>New ui item.</returns>
        public static IUiItem CreateUiItemFromXElement(UiType uiType, XElement itemNode, Skin parent)
        {
            if (uiType == UiType.ToggleButton)
            {
                return ToggleButton.FromXElement(itemNode, parent);
            }
            else if (uiType == UiType.FlashButton)
            {
                return FlashButton.FromXElement(itemNode, parent);
            }
            else if (uiType == UiType.RadialVector)
            {
                return RadialVector.FromXElement(itemNode, parent);
            }
            else
            {
                throw new UiNotSupported($"Could not load type {uiType.ToString()} (line: {Parsers.GetNodeLine(itemNode)})");
            }
        }

        /// <summary>
        /// Resolves parameters to a hadrware input source description
        /// </summary>
        /// <param name="hwType">Type of item to create.</param>
        /// <param name="itemNode">Base node containing item information.</param>
        /// <param name="parent">Associate parent.</param>
        /// <returns>New input description item.</returns>
        public static IInputSourceDescription CreateInputSourceFromXElement(Type hwType, XElement itemNode, Skin parent)
        {
            if (hwType == typeof(Button2))
            {
                return Button2Description.FromXElement(itemNode);
            }
            else if (hwType == typeof(Button3))
            {
                return Button3Description.FromXElement(itemNode);
            }
            else if (hwType == typeof(IRangeableInput))
            {
                return RangeableInputDescription.FromXElement(itemNode);
            }
            else if (hwType == typeof(IRangeableInput2))
            {
                return RangeableInput2Description.FromXElement(itemNode);
            }
            else if (hwType == typeof(IRangeableInput3))
            {
                return RangeableInput3Description.FromXElement(itemNode);
            }
            else
            {
                throw new GenericHardwareNotSupported($"Could not load type (check case) {hwType.ToString()} (line: {Parsers.GetNodeLine(itemNode)})");
            }
        }

        /// <summary>
        /// Loads assemblies from specified directory. Looks for items of type <see cref="InputListenerBase"/>.
        /// This can only be performed once.
        /// </summary>
        private static void LoadPlugins()
        {
            if (_pluginsLoaded)
            {
                return;
            }

            // Need an absolute path to iterate over the sub directories
            var directory = Path.GetFullPath(PluginsDirectory);

            if (string.IsNullOrEmpty(directory))
            {
                throw new ArgumentNullException($"{nameof(PluginsDirectory)}");
            }

            if (!Directory.Exists(directory))
            {
                throw new InvalidOperationException($"Missing plugins directory: {directory}");
            }
            
            var files = Directory.EnumerateFiles(directory);
            
            foreach (var file in files)
            {
                if (!file.EndsWith(".dll"))
                {
                    continue;
                }

                var dllPath = Path.Combine(directory, file);
                Assembly assembly = null;

                try
                {
                    assembly = Assembly.LoadFrom(dllPath);
                }
                catch (System.IO.FileLoadException)
                {
                    System.Console.WriteLine($"The assembly {dllPath} has already been loaded.");
                    continue;
                }
                catch (System.BadImageFormatException)
                {
                    System.Console.WriteLine($"The file {dllPath} is not an assembly.");
                    continue;
                }
                catch
                {
                    throw;
                }

                var types = assembly.GetTypes();

                foreach (var type in types)
                {
                    if ((typeof(IPlugin)).IsAssignableFrom(type))
                    {
                        _eventSourceTypes.Add(type);
                    }
                }
            }

            _pluginsLoaded = true;
        }

        /// <summary>
        /// Loads assemblies from specified directory. Looks for items of type <see cref="IConfigDataProvider"/>.
        /// This can only be performed once.
        /// </summary>
        private static void LoadConfigDataProviders()
        {
            if (_configDataProvidersLoaded)
            {
                return;
            }

            var directory = PluginsDirectory;

            if (string.IsNullOrEmpty(directory))
            {
                throw new ArgumentNullException($"{nameof(PluginsDirectory)}");
            }

            if (!Directory.Exists(directory))
            {
                throw new InvalidOperationException($"Missing plugins directory: {directory}");
            }

            var files = Directory.EnumerateFiles(directory);

            foreach (var file in files)
            {
                if (!file.EndsWith(".dll"))
                {
                    continue;
                }

                var dllPath = Path.Combine(directory, file);
                Assembly assembly = null;

                try
                {
                    assembly = Assembly.LoadFrom(dllPath);
                }
                catch (System.IO.FileLoadException)
                {
                    System.Console.WriteLine($"The assembly {dllPath} has already been loaded.");
                    continue;
                }
                catch (System.BadImageFormatException)
                {
                    System.Console.WriteLine($"The file {dllPath} is not an assembly.");
                    continue;
                }
                catch
                {
                    throw;
                }

                var types = assembly.GetTypes();

                foreach (var type in types)
                {
                    if ((typeof(IConfigDataProvider)).IsAssignableFrom(type))
                    {
                        _configDataProviderTypes.Add(type);
                    }
                }
            }

            _configDataProvidersLoaded = true;
        }
    }
}
