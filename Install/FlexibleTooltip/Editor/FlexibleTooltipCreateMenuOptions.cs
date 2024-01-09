using System;
using UnityEditor;
using UnityEngine;

namespace com.flexford.packages.tooltip.editor
{
	public static class FlexibleTooltipCreateMenuOptions
	{
		public const string CONTEXT_MENU_PATH = "Flexible Tooltip/";
		public const string GAME_OBJECT_CONTEXT_MENU_PATH = "GameObject/" + CONTEXT_MENU_PATH;
		public const string ASSETS_CONTEXT_MENU_PATH = "Assets/Create/" + CONTEXT_MENU_PATH;

		[MenuItem(GAME_OBJECT_CONTEXT_MENU_PATH + "Create default", false)]
		private static void CreateElement(MenuCommand menuCommand)
		{
			CreatePrefabCommand(menuCommand);
		}

		[MenuItem(ASSETS_CONTEXT_MENU_PATH + "Create prefab variant", false)]
		private static void CreatePrefabVariant(MenuCommand menuCommand)
		{
			var prefab = GetDefaultPrefabObject();
			if (prefab != null)
			{
				GameObject objSource = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
				string assetsPath = $"{FlexibleTooltipMenuUtilities.GetProjectWindowFolder()}/{objSource.name} (Variant).prefab";
				GameObject prefabVariant = PrefabUtility.SaveAsPrefabAsset(objSource, assetsPath);
				GameObject.DestroyImmediate(objSource);
			}
		}

		[MenuItem(ASSETS_CONTEXT_MENU_PATH + "Create style/Default(short) variant", false)]
		private static void CreateDefaultShortStyleVariant(MenuCommand menuCommand)
		{
			CreateStyleCommand(config => config.DefaultStyleShort, menuCommand);
		}

		[MenuItem(ASSETS_CONTEXT_MENU_PATH + "Create style/Default(medium) variant", false)]
		private static void CreateDefaultMediumStyleVariant(MenuCommand menuCommand)
		{
			CreateStyleCommand(config => config.DefaultStyleMedium, menuCommand);
		}

		[MenuItem(ASSETS_CONTEXT_MENU_PATH + "Create style/Default(long) variant", false)]
		private static void CreateDefaultLongStyleVariant(MenuCommand menuCommand)
		{
			CreateStyleCommand(config => config.DefaultStyleLong, menuCommand);
		}

		public static void CreatePrefabCommand(MenuCommand menuCommand)
		{
			var prefab = GetDefaultPrefabObject();
			if (prefab != null)
			{
				GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
				FlexibleTooltipMenuUtilities.PlaceUIElementRoot(instance, menuCommand);
			}
		}

		private static GameObject GetDefaultPrefabObject()
		{
			FlexibleTooltipConfig config = FlexibleTooltipConfig.Instance;
			if (config == null)
			{
				Debug.LogError($"Not found package config '{nameof(FlexibleTooltipConfig)}'");
				return null;
			}

			if (config.DefaultPrefab == null)
			{
				Debug.LogError($"Not found default prefab in config '{AssetDatabase.GetAssetPath(config)}'");
				return null;
			}

			return config.DefaultPrefab;
		}

		public static void CreateStyleCommand(Func<FlexibleTooltipConfig, FlexibleTooltipStyle> getStyle, MenuCommand menuCommand)
		{
			FlexibleTooltipConfig config = FlexibleTooltipConfig.Instance;
			if (config == null)
			{
				Debug.LogError($"Not found package config '{nameof(FlexibleTooltipConfig)}'");
				return;
			}

			if (getStyle == null)
			{
				Debug.LogError($"Style fet method is null");
				return;
			}

			var style = getStyle.Invoke(config);
			if (style == null)
			{
				Debug.LogError($"Not found default style in config '{AssetDatabase.GetAssetPath(config)}'");
				return;
			}

			FlexibleTooltipStyle newStyleInstance = ScriptableObject.Instantiate(style);
			string assetsPath = $"{FlexibleTooltipMenuUtilities.GetProjectWindowFolder()}/{style.name} (Variant).asset";
			AssetDatabase.CreateAsset(newStyleInstance, assetsPath);
		}
	}
}