using UnityEditor;
using UnityEngine;

namespace com.flexford.packages.tooltip.editor
{
	public static class FlexibleTooltipMenuOptions
	{
		public const string CONTEXT_MENU_PATH = "UI/Flexible Tooltip/";
		public const string GAME_OBJECT_CONTEXT_MENU_PATH = "GameObject/" + CONTEXT_MENU_PATH;
		public const string ASSETS_CONTEXT_MENU_PATH = "Assets/Create/" + CONTEXT_MENU_PATH;

		public const string DEFAULT_PREFAB_PATH = "FlexibleTooltip/Prefabs/tooltip";
		public const string DEFAULT_STYLE_SHORT_PREFAB_PATH = "FlexibleTooltip/Styles/DefaultTooltipStyle(short)";
		public const string DEFAULT_STYLE_MEDIUM_PREFAB_PATH = "FlexibleTooltip/Styles/DefaultTooltipStyle(medium)";
		public const string DEFAULT_STYLE_LONG_PREFAB_PATH = "FlexibleTooltip/Styles/DefaultTooltipStyle(long)";

		[MenuItem(GAME_OBJECT_CONTEXT_MENU_PATH + "Create default", false)]
		private static void CreateElement(MenuCommand menuCommand)
		{
			CreatePrefabCommand(DEFAULT_PREFAB_PATH, menuCommand);
		}

		[MenuItem(ASSETS_CONTEXT_MENU_PATH + "Create prefab variant", false)]
		private static void CreatePrefabVariant(MenuCommand menuCommand)
		{
			Object originalPrefab = Resources.Load<GameObject>(DEFAULT_PREFAB_PATH);
			GameObject objSource = PrefabUtility.InstantiatePrefab(originalPrefab) as GameObject;
			string assetsPath = $"{FlexibleTooltipMenuUtilities.GetProjectWindowFolder()}/{objSource.name} (Variant).prefab";
			GameObject prefabVariant = PrefabUtility.SaveAsPrefabAsset(objSource, assetsPath);
			GameObject.DestroyImmediate(objSource);
		}

		[MenuItem(ASSETS_CONTEXT_MENU_PATH + "Create style/Default(short) variant", false)]
		private static void CreateDefaultShortStyleVariant(MenuCommand menuCommand)
		{
			CreateStyleCommand(DEFAULT_STYLE_SHORT_PREFAB_PATH, menuCommand);
		}

		[MenuItem(ASSETS_CONTEXT_MENU_PATH + "Create style/Default(medium) variant", false)]
		private static void CreateDefaultMediumStyleVariant(MenuCommand menuCommand)
		{
			CreateStyleCommand(DEFAULT_STYLE_MEDIUM_PREFAB_PATH, menuCommand);
		}

		[MenuItem(ASSETS_CONTEXT_MENU_PATH + "Create style/Default(long) variant", false)]
		private static void CreateDefaultLongStyleVariant(MenuCommand menuCommand)
		{
			CreateStyleCommand(DEFAULT_STYLE_LONG_PREFAB_PATH, menuCommand);
		}

		public static void CreatePrefabCommand(string prefabPath, MenuCommand menuCommand)
		{
			GameObject prefab = Resources.Load<GameObject>(prefabPath);
			GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
			FlexibleTooltipMenuUtilities.PlaceUIElementRoot(instance, menuCommand);
		}

		public static void CreateStyleCommand(string stylePath, MenuCommand menuCommand)
		{
			FlexibleTooltipStyle defaultStyle = Resources.Load<FlexibleTooltipStyle>(stylePath);
			if (defaultStyle != null)
			{
				FlexibleTooltipStyle newStyleInstance = ScriptableObject.Instantiate(defaultStyle);
				string assetsPath = $"{FlexibleTooltipMenuUtilities.GetProjectWindowFolder()}/{defaultStyle.name} (Variant).asset";
				AssetDatabase.CreateAsset(newStyleInstance, assetsPath);
			}
		}
	}
}