using HidDisplay.SkinModel.Error;
using HidDisplay.SkinModel.HotConfig;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace HidDisplay.SkinModel.Core
{
    /// <summary>
    /// Defines how to display hardware event interactions on screen.
    /// </summary>
    public class Skin : IDisposable
    {
        /// <summary>
        /// Event to hook for setup to initialize wpf ui during activation.
        /// </summary>
        public event EventHandler<InputHandler> SetupCallback;

        /// <summary>
        /// Gets or sets the name of this skin.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the author of the skin.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets the format version used.
        /// </summary>
        public string MetaFormat { get; set; }

        /// <summary>
        /// Gets or sets the absolute path of the directory containing this skin.
        /// </summary>
        public string AbsoluteContainerPath { get; set; }

        /// <summary>
        /// Gets or sets the name of the directory containing this skin.
        /// </summary>
        public string DirectoryContainerName { get; set; }

        /// <summary>
        /// Gets or sets the background image.
        /// </summary>
        public ImageInfo BackgroundImage { get; set; }

        /// <summary>
        /// Gets or sets associated input handlers to be used by this skin.
        /// </summary>
        public List<InputHandler> InputHandlers { get; set; } = new List<InputHandler>();

        /// <summary>
        /// Gets or sets list of configuration options forwarded to input handlers during activation/startup.
        /// </summary>
        public Dictionary<string, string> ConfigOptions { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Skin"/> class.
        /// </summary>
        /// <param name="containingDirectory">Absolute path of directory containing skin.</param>
        public Skin(string containingDirectory)
        {
            AbsoluteContainerPath = containingDirectory;

            var index = AbsoluteContainerPath.LastIndexOf(System.IO.Path.DirectorySeparatorChar);
            
            DirectoryContainerName = AbsoluteContainerPath.Substring(index + 1, AbsoluteContainerPath.Length - index - 1);
        }

        /// <summary>
        /// Looks in the skin directory and attempts to read settings json file into ConfigOptions.
        /// </summary>
        public void LoadConfigOptions()
        {
            var jsonSettings = Settings.FromFile(Path.Combine(AbsoluteContainerPath, SkinModel.Constants.SkinSettingsFilename));
            
            if (!object.ReferenceEquals(null, jsonSettings))
            {
                ConfigOptions = jsonSettings.ToSettingsDictionary(); 
            }
            else
            {
                ConfigOptions = new Dictionary<string, string>();
            }
        }

        /// <summary>
        /// Loads settings json file from disk, calls setup on each input handler, fires
        /// the SetupInputEvent for wpf app to setup, then starts each input handler.
        /// </summary>
        public void Activate()
        {
            try
            {
                LoadConfigOptions();
            }
            catch (Exception ex)
            {
                throw new ActivationException("Could not load config options when activating skin.", ex);
            }

            foreach (var inputHandler in InputHandlers)
            {
                try
                {
                    inputHandler.Handler.Setup(ConfigOptions);
                }
                catch (Exception ex)
                {
                    throw new ActivationException($"Exception seting up input handler {inputHandler.Description}", ex);
                }

                try
                {
                    if (SetupCallback != null)
                    {
                        SetupCallback(this, inputHandler);
                    }
                }
                catch (Exception ex)
                {
                    throw new ActivationException($"Exception hooking main app UI updates for input handler {inputHandler.Description}", ex);
                }

                try
                {
                    inputHandler.Handler.Start();
                }
                catch (Exception ex)
                {
                    throw new ActivationException($"Exception starting input handler {inputHandler.Description}", ex);
                }
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (!object.ReferenceEquals(null, BackgroundImage))
            {
                BackgroundImage.Dispose();
            }

            if (!object.ReferenceEquals(null, InputHandlers) && InputHandlers.Any())
            {
                foreach (var x in InputHandlers)
                {
                    x.Dispose();
                }
            }
        }

        /// <summary>
        /// Processes xml file and creates <see cref="Skin"/>.
        /// </summary>
        /// <param name="path">Absolute path of skin definition file.</param>
        /// <returns>New <see cref="Skin"/>.</returns>
        public static Skin FromXmlFile(string path)
        {
            var xmlraw = File.ReadAllText(path);

            var d2 = path.LastIndexOf(Path.DirectorySeparatorChar);
            var dirpath = path.Substring(0, d2);

            var skin = Skin.FromXmlRaw(xmlraw, dirpath);

            return skin;
        }

        /// <summary>
        /// Partially loads skin, just enough to read the meta-data associated.
        /// </summary>
        /// <param name="xml">Full xml string to parse.</param>
        /// <param name="containingDirectory">Absolute path of directory containing skin.</param>
        /// <returns>New <see cref="Skin"/>.</returns>
        public static Skin InfoFromXmlRaw(string xml, string containingDirectory)
        {
            if (string.IsNullOrEmpty(xml))
            {
                throw new ArgumentNullException(nameof(xml));
            }

            var xdoc = XDocument.Parse(xml, LoadOptions.SetLineInfo);
            var skin = new Skin(containingDirectory);

            var root = xdoc.Descendants("skin").First();
            skin.MetaFormat = (string)root.Attribute("format");

            var infoNode = root.Descendants("info").FirstOrDefault();
            if (!object.ReferenceEquals(null, infoNode))
            {
                skin.DisplayName = (string)infoNode.Element("displayName");
                skin.Author = (string)infoNode.Element("author");
            }

            return skin;
        }

        /// <summary>
        /// Processes xml and creates <see cref="Skin"/>.
        /// </summary>
        /// <param name="xml">Full xml string to parse.</param>
        /// <param name="containingDirectory">Absolute path of directory containing skin.</param>
        /// <returns>New <see cref="Skin"/>.</returns>
        public static Skin FromXmlRaw(string xml, string containingDirectory)
        {
            if (string.IsNullOrEmpty(xml))
            {
                throw new ArgumentNullException(nameof(xml));
            }

            var xdoc = XDocument.Parse(xml, LoadOptions.SetLineInfo);
            var skin = new Skin(containingDirectory);

            var root = xdoc.Descendants("skin").First();
            skin.MetaFormat = (string)root.Attribute("format");

            var infoNode = root.Descendants("info").FirstOrDefault();
            if (!object.ReferenceEquals(null, infoNode))
            {
                skin.DisplayName = (string)infoNode.Element("displayName");
                skin.Author = (string)infoNode.Element("author");
            }
            
            var mainNode = root.Descendants("main").FirstOrDefault();
            if (!object.ReferenceEquals(null, mainNode))
            {
                skin.BackgroundImage = ImageInfo.FromXElement(mainNode.Element("background"), containingDirectory);
            }

            var inputHandlersNodes = root.Descendants("inputHandler");
            foreach (var inputHandlerNode in inputHandlersNodes)
            {
                skin.InputHandlers.Add(InputHandler.FromXElement(inputHandlerNode, skin));
            }

            return skin;
        }
    }
}
