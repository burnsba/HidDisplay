# HidDisplay.SkinModel

Each skin should be contained in it's own directory. There must be a file named `define.xml`. This is the main skin file and explains the layout and program interaction.

# define.xml

The definition file contains three sections, one for information about the skin, one for main settings, and one or more sections devoted to input handlers. This is XML, which is case sensitive. The schema looks like

    <?xml version="1.0" encoding="utf-8"?>
    <skin format="v1">
        <info>
        
        	<!-- displayName is what gets shown in the main application -->
            <displayName>Counter-Strike: Global Offensive</displayName>
            <author>Ben Burns</author>
            
            <!-- other tags aren't currently suppored -->
            <version>1.0</version>
            <versionDate>2019-12-26</versionDate>
        </info>
        <main>
        
            <!-- background image info -->
            <background ...></background>
        </main>
        
        <!-- input plugin, what it is, how to display items from this source -->
        <inputHandler ... >
            ...
        </inputHandler>
    </skin>

## info
Display and meta information about the skin.

Node: `displayName`: defines what is shown in the application.
Node: `author`: author information

## main
Currently just background file information.

Node: `background`
Attributes:
- `image` (string): image filename
- `overrideWidth` (int): resizes image to this width
- `overrideHeight` (int): resizes image to this height
- `xOffset` (int): x offset (from left) where this image is placed
- `yOffset` (int): y offset (from top) where this image is placed

## inputHandler

This describes where input comes from. When a specific input is received, a button or similar can be shown on the main user interface.

Node: `inputHandler`: this is the plugin (see the plugin area documentation for more info).
Attributes:
- `handlerType`: C# Type name, without assembly prefix.
- `handlerAssembly`: C# assembly name containing the Type (this usually, but not always, has the same file name).
- `description`: optional description, useful for debugging.

### inputHandler.item

Each `inputHandler` can have one or more children. Each child corresponds to an input event.

Node: `item`: Specific input event to respond to
Attributes:
- `uiType`: Explains how the user interface should be updated on an input event.
- `hwType`: Explains what kind of possible input events to expect.
- `name`: Helps debugging and explaining what each item is. Shared with child items.

`uiType` can be:
- `toggleButton` (case insensitive): Image is shown when button is pressed, otherwise hidden.
- `flashButton` (case insensitive): Image is shown for a brief period of time on input.
- `radialVector` (case insensitive): Image is centered at a location, the orientation is rotated or translated about the central point. This is used for showing mouse or joystick position.

Note the type names are parsed as an enum ignoring case, so the xml doesn't need to match case either.

### inputHandler.item.uiSettings

Each `inputHandler.item` has settings to explain what the `uiType` should look like. Specific attributes depend on the `uiType`.

### inputHandler.item.uiSettings.toggleButton

- `image` (string): image filename
- `overrideWidth` (int): resizes image to this width
- `overrideHeight` (int): resizes image to this height
- `xOffset` (int): x offset (from left) where this image is placed
- `yOffset` (int): y offset (from top) where this image is placed

### inputHandler.item.uiSettings.flashButton

- Same attributes as `toggleButton`
- `duration`: number of milliseconds image should be shown on input

### inputHandler.item.uiSettings.radialVector

This is more complicated than it needs to be, and the values interact with each other  more than they should. If using scale adjustment, internal `scaleFactor` amount is calculated by

    scaleFactor = (norm * scaleNorm / scaleMax) + scaleMin;

Where norm is the magnitude of the input (for 2d input, `Math.Sqrt(x*x + y*y)`).

- `image` (string): image filename
- `xOffset` (int): x offset (from left) where this image is placed
- `yOffset` (int): y offset (from top) where this image is placed
- `radialFactor` (double): Rescales offset/transform by this amount (multiplicative)
- `scaleMin` (double): lower end rescaled value
- `scaleMax` (double): high end of rescaled value
- `scaleNorm` (double): multiplier for rescaling
- `useSlide` (bool): whether or not to move image radially inward/outward based on input
- `useScale` (bool): whether or not to resize image based on magnitude of input (higher input value = larger image)
- `slideFactor` (double): factor to adjust position radially inward or outward (multiplicative)
- `slideMax` (double): max value to slide outward

The `radialVector` image is positioned per the top left pixel, but all transformations and rotations are relative to the center of the image.

### inputHandler.item.hwSettings

Each `inputHandler.item` has settings to explain what input events should drive UI updates. These generic types are provided by the plugin (see the project documentation for more details). Specific attributes depend on the `hwType`. Supported types are

- `Button2`: button with two states (e.g., on, off)
- `Button3`: button with three states (e.g., neutral, scroll up, scroll down)
- `IRangeableInput`: a single input that can range over a number of possible values
- `IRangeableInput2`: input that can range over a number of possible values, along two dimensions
- `IRangeableInput3`: input that can range over a number of possible values, along three dimensions

Note that unlike `uiType`, `hwType` type names are case sensitive.
Note that the rangeable input Type will also provide range information, see the plugin project readme for more information.

### inputHandler.item.hwSettings.Button2

When a `GenericInputEventArgs` is received, each `Button2` will be compared. Items matching the state attributes below will get UI updates according to the `uiType`. The `toggleButton` is designed to be used with this hardware type.

Attributes:
- `id`: id to match.
- `stateMatch` (case insensitive): state to match. Possible values are `Active`, `Released`.

### inputHandler.item.hwSettings.Button3

When a `GenericInputEventArgs` is received, each `Button3` will be compared. Items matching the state attributes below will get UI updates according to the `uiType`. The `flashButton` is designed to be used with this hardware type, but `toggleButton` can also be used.

Attributes:
- `id`: id to match.
- `stateMatch` (case insensitive): state to match. Possible values are `StateDefault`, `State2`, and `State3`.

### inputHandler.item.hwSettings.IRangeableInput

Not implemented. For one dimensional ranges, like temperature or heartrate.

### inputHandler.item.hwSettings.IRangeableInput2

When a `GenericInputEventArgs` is received, each `IRangeableInput2` will be compared. Items with the given `id` attributes below will get UI updates according to the `uiType`. The `radialVector` is the only supported `uiType`.

Attributes:
- `id`: id to match.
- `inputCeiling`: max allowed hardware input value. This is used by the norm calculation in `radialVector`.
- `invert1` (bool): whether or not to invert first rangable value when calculating ui transform.
- `invert2` (bool): whether or not to invert second rangable value when calculating ui transform.

### inputHandler.item.hwSettings.IRangeableInput3

Not implemented. For three dimensional ranges, like x,y,z position.

### other notes

Images are oriented to the top and to the left, so `xOffset` should be the left-most pixel, and `yOffset` should be the top-most pixel.

Parsing bool values is rather loose and ignores case. Any string similar/contains to `1` or`true` parses to true, any string similar/contains to `0` or `false` parses to false, and anything else parses to false.

# settings.json

There's an optional `settings.json` file to configure specific aspects of the skin. These can be changed from the main application. Changes are saved back to the file.

The schema looks like

    {
      "settings": [
        {
          "key": "NintendoSpy64.ComPort",
          "display": "Controller COM port",
          "input": "dropdown",
          "datasource": "SerialComProvider",
          "assembly": "HidDisplay.DefaultConfigDataProviders",
          "currentValue": "COM5"
        },
        {
            ...
        }
        ]
    }

## settings.key

How the config item is identified in the settings file. This just needs to be a unique string, prefixing the plugin name seems like a good idea.

## settings.display

What label or description to show the user on the config screen.

## settings.input

The type of input to show to the user. Supported types are `Textbox` and `Dropdown`.

## settings.datasource

Only required if the `settings.input` needs to retrieve additional data. If so, this is the C# Type name of the input provider, without the assembly prefix.

The datasource needs to implement the `IConfigDataProvider` interface.

## settings.assembly

Only required if the `settings.input` needs to retrieve additional data. If so, this is the C# assembly name containing the Type (this usually, but not always, has the same file name).

## settings.currentValue

Current value of the setting.