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

		[MenuItem(GAME_OBJECT_CONTEXT_MENU_PATH + "Create default", false)]
		public static void CreateElement(MenuCommand menuCommand)
		{
			CreateCommand(DEFAULT_PREFAB_PATH, menuCommand);
		}

		[MenuItem(ASSETS_CONTEXT_MENU_PATH + "Create prefab variant", false)]
		public static void CreatePrefabVariant(MenuCommand menuCommand)
		{
			Object originalPrefab = Resources.Load<GameObject>(DEFAULT_PREFAB_PATH);
			GameObject objSource = PrefabUtility.InstantiatePrefab(originalPrefab) as GameObject;
			string assetsPath = $"{FlexibleTooltipMenuUtilities.GetProjectWindowFolder()}/{objSource.name} (Variant).prefab";
			GameObject prefabVariant = PrefabUtility.SaveAsPrefabAsset(objSource, assetsPath);
			GameObject.DestroyImmediate(objSource);
		}

		public static void CreateCommand(string prefabPath, MenuCommand menuCommand)
		{
			GameObject prefab = Resources.Load<GameObject>(prefabPath);
			GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
			FlexibleTooltipMenuUtilities.PlaceUIElementRoot(instance, menuCommand);
		}
	}
}