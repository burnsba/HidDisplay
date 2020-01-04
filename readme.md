This program shows button or key presses on screen while a user interacts with a device. Supported hardware:

- Mouse (via [SetWindowsHookEx](https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setwindowshookexa), WH_MOUSE_LL; and RawInput)
- Keyboard (via SetWindowsHookEx, WH_KEYBOARD_LL)
- Serial port input
- RawInput HID ("Gamepad" and "Joystick" only; not supported: RawInput keyboard)
- bluetooth sensor

The serial port and RawInput HID are used to display keypresses and interaction from the following devices:

- [NintendoSpy](https://github.com/jaburns/NintendoSpy) - serial port (N64 only; not supported: SNES, NES)
- [DuoWatch64](https://github.com/burnsba/DuoWatch64) - serial port (simultaneous dual N64 controllers)
- [Raphnet](https://www.raphnet-tech.com/products/n64_usb_adapter_gen3/index.php) - HID USB adapter (single N64 controller only; not supported: dual controllers, gamecube)
- Buffalo "classic USB gamepad" - HID USB adapter (it's an SNES controller)

Support for bluetooth devices must be add individually for each [characteristic](https://www.bluetooth.com/specifications/gatt/characteristics/). Currently, the following are supported:

- Heart Rate Measurement (0x2a37)

A simple plugin interface is defined to interact with these different kinds of hardware. The application manages loading and unloading plugins. The key thing to note is that a plugin will accept native input and then translate it into a "generic" input definition to send to the application (I designed this before I know about HID capabilities reports, but the concept is similar). More details on plugins are available in the HidDisplay.PluginDefinition readme.

A simple customization capability is provided to define which plugin(s) to use and how to show input on screen. This is bundled in a directory referred to as a Skin. More details on skins are available in the HidDisplay.SkinModel readme.

When I started this project, I was vaguely aware Windows (OS) referred to input devices as "Human Interface Device," which seemed like a nice generic way to refer to them. I borrowed the name, but it wasn't until later I started using hid.dll.

Note: Bluetooth support pulls in a dependency from the Windows SDK, a file called "windows.winmd". After installing the SDK, you can find it (depending on the actual version) at

    C:\Program Files (x86)\Windows Kits\10\UnionMetadata\10.0.17763.0\Windows.winmd

# Application settings

The path to the skin folder, path to the plugin folder, and application background and configured in the `app.config`. These can be changed from the "Settings" option in the window menu. Due to the way Type information is handled in dotnet, changing the plugin folder will not take effect until the program is restarted.

# Files
The top level directory structure:

- EventFinder
- EventFinderHid
- HidDisplay.Controller
- HidDisplay.DefaultConfigDataProviders
- HidDisplay.DefaultPlugins
- HidDisplay.PluginDefinition
- HidDisplay.SkinModel
- HidDisplayDnc
- img
- plugins
- Skins

Contents of each directory:

### EventFinder
CSharp project. Debug console app to show keycodes and mouse events.

### EventFinderHid
CSharp project. Debug WPF app. Dumps RawInput HID (joystick and gamepad) events to the System.Diagnostics.Debug console.

### HidDisplay.DefaultConfigDataProviders
CSharp project, library. The plugins might need the application to supply some information for configuration or startup. For example, any plugin using the serial port needs a method to choose the port and baudrate.

This contains data providers used by the plugins.

### HidDisplay.DefaultPlugins
CSharp project, library. This is supposed to be a repository to support input from native (to the host OS) devices, which is just the mouse and keyboard.

### HidDisplay.PluginDefinition
CSharp project, library. Interface and common files used by plugins.

### HidDisplay.SkinModel
CSharp project, library. Contains Skin class definitions. Loading and parsing from XML. Types of UI display objects used by the application to show inputs.

### HidDisplay.XenoPlugins
CSharp project, library. This is supposed to be a repository to support input from non-native or 3rd party devices.

Contains plugins to support receiving NintendoSpy, DuoWatch64, Raphnet, and Buffalo gamepad messages (Serial port, and USB). Contains Bluetooth plugins.

### HidDisplayDnc
CSharp application, WPF. The main WPF app, for dotnet. core. Handles loading and unloading skins and plugins. Capturing hooks, e.g.  SetWindowsHookEx, are handled in the plugins but RawInput HID events need to be captured and forwarded in some kind of [WndProc](https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.control.wndproc?view=netframework-4.8) handler in a Window. The app handles hooking that, and if a currently loaded skin needs it, will forward that to the plugin handler.

Currently the app will accept WM_INPUT, try to process it as a HID input event, and if successful will forward the processed HID result to associated plugins. Currently, only RawInput  with a UsagePage of Gamepad or Joystick is captured.

### img
Some program image data, like the app icon.

### plugins
Default plugin directory (when running from the default build folder).

### Skins
Default skin directory (when running from the default build folder). Contains some sample skins.
