# unity-toolbar-extender-plus

This is improved version of package [unity-toolbar-extender](https://github.com/marijnz/unity-toolbar-extender) by Marijn Zwemmer.

Extend the Unity Toolbar with your own UI code.
Add buttons to quickly access scenes, add sliders, toggles, anything.

## Improvements
- flexibility (add, remove, disable and sorting elements without code)
- convenient sorting elements
- contains ready toolbar styles like unity
- contains api for draw gui elements(button, dropdown, int field)
- supports light and dark editor theme
- every team member can set invisible locally some element, will be changed only editor prefs, not scriptable object

## Requirements ⚠️
- Unity 2021.1 and above

## Installation 💾
<details>
<summary>Add from GitHub in <kbd>Package Manager Editor Window</kbd></summary>

- open Package Manager
- click `+`
- select `Add from Git URL`
- paste `https://github.com/AlexZonov/unity-toolbar-extender-plus.git`
- click `Add`
</details>

<details>
<summary>Add from GitHub in <kbd>Packages/minifest.json</kbd></summary>

- open `Packages/minifest.json`
- add `"com.flexford.unity-package-info-receiver": "git+https://github.com/AlexZonov/unity-toolbar-extender-plus.git"` + version if need (`#v1.0.0`)
- save
</details>

## How to use 💡
1) Create settings scriptable object:</br>
   <kbd>Project Window -> RBM -> Create -> Tools -> Toolbar Extender+ -> Create settings</kbd>
2) Implement your elements inherited from <kbd>ToolbarElement</kbd> with available package API
3) Set these elements to your toolbar settings
4) Done, now you can use your elements

## Available API
### com.flexford.packages.toolbar.ToolbarStyles
Contains common styles for draw elements like other unity toolbar elements

### com.flexford.packages.toolbar.ToolbarLayout
Contains common methods for draw elements with callbacks on value change

### com.flexford.packages.toolbar.ToolbarUtilities
Contains useful methods, example create element scriptable object with dialog relative current project window dirrectory