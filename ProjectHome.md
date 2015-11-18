# Scripting Machine #

### Introduction ###
Scripting Machine is a smart IDE developed specialy for SA-MP scripters, that try to simplify the scripter's task, so he can het focused on what he's scripting.

**Why it's smart?**

The IDE can detect variables/arrays (native types: 'Float:', 'Text:', 'File:', etc) and show them on each function (native or not) that has a param with the defined type; same with colors, and on the dialog maker you can use defined colors (only embedded ones); syntax highligt for native and custom functions.

Note: The aplication is not under development for not (but will be in the near feature), this is due the lack of time of the main (only) developer.

### IDE Functions ###
  * Unlimited tabs (one per file)
  * Syntax highlight (for native and custom functions)
  * Folding
  * Single instance (no more problems with includes, 1 IDE for all your files)
  * List with all avaliable includes
  * List with the document includes
  * Restore files on crash
  * Custom syntax highlight colors
  * Custom background color
  * Folding
  * Smart Indentation
  * File association
  * Help box with current function's parameters
  * Autocomplete:
    1. Native and user defined functions
    1. Native and user defined callbacks
    1. User defined types for native and user defined functions
  * Detection System:
    1. Detect typed vars/arrays and functions from loaded includes
    1. Detect typed vars/arrays and functions from selected file (working file)
    1. Detect colors (embedded or not)
    1. Detect custom public definitions by macros (ex: "#define PUBLIC:%0(%1)    forward public %0(%1);   public %0(%1)"
  * Primary color picker:
    1. RGB, CMYK and HSL formats
    1. Embedded or normal colors
    1. Option to define the generated color
    1. Option to export the generated color to the working file
  * Dialog maker/previewer
    1. Previewer that show how the dialog will look like on the game
    1. Embedded colors from working file will work on the dialog
    1. 4 native types of dialogs.
    1. Easy to use, and very accurate
  * Areas:
    1. Diferent colors for diferent areas
    1. Zoom
    1. Option to choose between GangZone and WorldBounds
    1. Show rectangle or fill them with the color
    1. Export generated areas to working file
  * Converter:
    1. Object & vehicle converter
    1. Diferent input/output formats
    1. Set all objetcs/vehicles on a specific array
    1. Option to convert only objects
    1. Load the map from a file
    1. Fix objects IDs (0.3c objects from MTA)
    1. Custom Input Object output
    1. Convert interior & virtual world (if possible)
  * Teleports:
    1. strcmp, DCMD, ZCMD, YCMD
    1. send message with custom color
    1. set interior, world
    1. choose with or without vehicle
  * Secondary Pickup:
    1. When you need to generate a color for a message or sth wick, you can use this pickup
    1. RGB only
  * Gates:
    1. strcmp, DCMD, ZCMD
    1. 4 different types:
      1. 2 Commands (One for open the gate and one for close the gate)
      1. 1 Command (Open and close the gate with the same command)
      1. Semi-automatic (Open the gate with the command, and it closes automaticaly)
      1. Full-automatic (Open and close the gate automaticaly)
    1. restrictions for specified teams
    1. Message when opening the gate (otpional)
    1. Message when clossing the gate (optional)
  * Compiler:
    1. Default compiler is pawncc.exe
    1. Manage error in a detailed list
    1. Option to configure compiler args
  * Error processing:
    1. Real time error detection for includes (when a include can't be founded)
    1. Goto line on error/warning selection
  * Full info about skins, vehicles, anims, map icons, sounds, sprites, weapons
  * Extra:
    1. Multi-Language: English, Español, Portuguêse, Deutsch
    1. SetPlayerSkin will display a tab from where you can select the skin id (with image)
    1. Selecting a text will display a tab where you can see the selection length
    1. Extra tab will show you the lines of the current file
    1. Unsaved names will display a ' `*`' at the end of it's name
    1. Option to select where to load images (skins, vehicles, etc): file, url, default
    1. Option to view the header of the selected function (From "Includes" & "Current")
    1. Opening multiple files at the same time
    1. Opening files by drag and drop them on the main tool bars
    1. Multiple encoding types (UTF-8, ASCII, Big Endian and Unicode)

---
### Images ###
![http://i.imgur.com/MEJqU.png](http://i.imgur.com/MEJqU.png)
![http://i.imgur.com/Xu2C0.png](http://i.imgur.com/Xu2C0.png)
![http://i.imgur.com/hya5L.png](http://i.imgur.com/hya5L.png)

Note: Scripting Machine is an IDE from 2.0 or higher versions. Before it was just a SA-MP scripting application.