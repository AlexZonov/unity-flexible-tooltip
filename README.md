# com.flexford.unity-flexible-tooltip

A simple, flexible and user-friendly UI element that can be used for both tooltips and dialogs.

## Features 🏆
- supported horizontal 4-sides layout
- supported vertical 4-sides layout
- supported simple rect 4-sides layout
- has simple components for extend tooltip behavior
- easy style creation and replacement

## Requirements ⚠️
- Unity 2020.3 and above
- DOTween (with asmdef: <kbd>DOTween.Modules</kbd>)

## Installation 💾

### Add package in project
<details>
<summary>Add from GitHub in <kbd>Package Manager Editor Window</kbd></summary>

- open Package Manager
- click `+`
- select `Add from Git URL`
- paste `https://github.com/AlexZonov/unity-flexible-tooltip.git`
- click `Add`
</details>

<details>
<summary>Add from GitHub in <kbd>Packages/minifest.json</kbd></summary>

- open `Packages/minifest.json`
- add `"com.flexford.unity-flexible-tooltip": "git+https://github.com/AlexZonov/unity-flexible-tooltip.git"` + version if need (`#v1.0.0`)
- save
</details>

### Install additional assets
1) Execute `Tools -> Flexible Tooltip -> Install`
2) In project will be add `FlexibleTooltip` folder with configs and assets
3) You can move this folder and rename

> 📌 You can change all default assets, but it's not recommended

## How to use 💡

### Check package config
Config was added after "Install" action and has name `FlexibleTooltipConfig.asset`  
In config need set default assets(prefab and styles).

### Look at samples
1) Open `Package Manager Window`  
2) Select `Flexible Tooltip` package
3) Choose sample, like example `Overview`, click `Import` or `Reimport`  
4) Open scene from sample by path `Assets/Samples/Flexible Tooltip/[PACKAGE_VERSION]/[SAMPLE_NAME]`  
5) Start play mode

### Use default prefab with default styles
1) Execute in scene hierarhy window `Mouse right button -> Flexible Tooltip -> Create`  
2) Prepare object in inspector
3) Done

### Ways for change default tooltip prefab ways
1) Change base `tooltip.prefab`, this file was created after `Install` action(not recommended)
2) Create prefab variant, prepare and set in `FlexibleTooltipConfig.asset` as default (recommended)

### Create your element variant
Variant will created relative default prefab from config `FlexibleTooltipConfig.asset`   
You can create new prefab variants for all need cases with different components set:
1) Open preffered folder for prefab variants
2) Execute in project window `Mouse right button -> Create -> Flexible Tooltip -> Create Prefab Variant`  
3) Prepare variant
4) Done

### Create your own unique styles
1) Open preffered folder for prefab variants
2) Execute in project window `Mouse right button -> Create -> Flexible Tooltip -> Create Style`
   - `New` - create new empty style
   - `Default(short) variant` - create variant of default short style
   - `Default(medium) variant` - create variant of default medium style
   - `Default(large) variant` - create variant of default large style
3) Look at default styles settings and sprites import settings
4) Make custom sprites like were in default styles
5) Prepare your styles settings
6) (optional) set as default to config `FlexibleTooltipConfig.asset`
7) Done, you can use style in `FlexibleTooltip` component

## Components

### FlexibleTooltip
Main component with general settings and logic

### FlexibleTooltipHideOnClick
Add hide on click ability for tooltip

### FlexibleTooltipAnimator
Add ability for show\hide tooltip.  
Component has debug actions in context menu.  
⚠️ DOTween required.

### FlexibleTooltipRectWrapper
Add ability for restrict tooltip moving in rect

### FlexibleTooltipTarget
Add ability for auto-move tooltip to target object

### FlexibleTooltipHideOnTargetLeave
Add ability for hide tooltip if target leave from area.  
You can set required paddings and enable\disable sides for this logic.
