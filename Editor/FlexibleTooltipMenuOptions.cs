using UnityEditor;
using UnityEngine;

namespace com.flexford.packages.tooltip.editor
{
	public static class FlexibleTooltipMenuOptions
	{
		[MenuItem("GameObject/UI/Flexible Tooltip/Create v1", false)]
		static public void Create_v1(MenuCommand menuCommand)
		{
			GameObject prefab = Resources.Load<GameObject>("FlexibleTooltip/Prefabs/tooltip");
			GameObject instance = GameObject.Instantiate(prefab);
			FlexibleTooltipMenuUtilities.PlaceUIElementRoot(instance, menuCommand);
		}

		[MenuItem("GameObject/UI/Flexible Tooltip/Create v2", false)]
		static public void Create_v2(MenuCommand menuCommand)
		{
			GameObject prefab = Resources.Load<GameObject>("FlexibleTooltip/Prefabs/tooltip_v2");
			GameObject instance = GameObject.Instantiate(prefab);
			FlexibleTooltipMenuUtilities.PlaceUIElementRoot(instance, menuCommand);
		}
	}
}