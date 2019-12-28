# HidDisplay.PluginDefinition

Each skin defines an `inputHandler` to use, here referred to as a plugin. The plugin will receive native hardware events (keypress, serial port data) and translate them to `GenericInputEventArgs`. These arguments are raised to an event monitored by the main application which will then process them according to the skin definition.

Each plugin should implement the `IPlugin` definition. Plugins are marked `IDisposable` and should release any resources being used in the `Dispose` method.

### IPlugin.UpdateEvent

This is the event monitored by the main application. The plugin will accept native input, translate to `GenericInputEventArgs`, and pass that to this event.

### IPlugin.Setup

The settings.json file is read by the main application and passed to the plugin as key value pairs in a dictionary. This is called first, before Start.

### IPlugin.Start

Should mark the plugin `IsEnabled` and start receiving/translating input. Will be called after Setup is called.

### IPlugin.Stop

Stops accepting or receiving events. Events will stop being translated. Should set `IsEnabled` to false.

## Types of plugins

There are two broad plugin types, active and passive.

### Active plugins

Active plugins are supposed to be self contained. For example, the `WindowsMousePlugin` is capable of calling `SetWindowsHookEx`(user32.dll) on its own and receiving OS events.

- Must implement `IActiveMonitorPlugin`


### Passive plugins

Passive plugins depend on the host application to receive events. The type parameter in `IPassiveTranslate<T>` explains to the main application what input it will accept to translate to `GenericInputEventArgs`.

- Must implement `IPassiveTranslatePlugin`
- Must implement `IPassiveTranslate<T>`

Currently the only `IPassiveTranslate<T>` Type supported is `WindowsHardware.HardwareWatch.HidResult`.

### `IPassiveTranslate<HidResult>`

The main application hooks WndProc in `OnSourceInitialized`. It calls `RegisterRawInputDevices` (user32.dll) to monitor for `GamePad` and `Joystick` RawInput. In WndProc, if there are any plugins that require it, the application will extract the HID InputReport details from the RawInput notification event. This is where the `HidResult` comes from. This object is then passed to the plugin `AcceptMessage(object sender, HidResult message)` event. The plugin then evaluates the messsage, e.g. check the product name to determine if this is from the correct source, and then notifies the main application by raising `GenericInputEventArgs` the same way an active plugin does.