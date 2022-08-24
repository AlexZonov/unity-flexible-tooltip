#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace com.flexford.packages.tooltip
{
	public partial class FlexibleTooltipAnimator
	{
		private const string DEBUG_ACTIONS_PATH = "CONTEXT/FlexibleTooltipAnimator/";

		[MenuItem(DEBUG_ACTIONS_PATH + "Show(3 sec)")]
		private static void Debug_Show_3f(MenuCommand command)
		{
			FlexibleTooltipAnimator component = (FlexibleTooltipAnimator)command.context;
			component.Show(3f);
		}

		[MenuItem(DEBUG_ACTIONS_PATH + "Show(6 sec)")]
		private static void Debug_Show_6f(MenuCommand command)
		{
			FlexibleTooltipAnimator component = (FlexibleTooltipAnimator)command.context;
			component.Show(6f);
		}

		[MenuItem(DEBUG_ACTIONS_PATH + "Hide(3 sec)")]
		private static void Debug_Hide_3f(MenuCommand command)
		{
			FlexibleTooltipAnimator component = (FlexibleTooltipAnimator)command.context;
			component.Hide(3f);
		}

		[MenuItem(DEBUG_ACTIONS_PATH + "Hide(6 sec)")]
		private static void Debug_Hide_6f(MenuCommand command)
		{
			FlexibleTooltipAnimator component = (FlexibleTooltipAnimator)command.context;
			component.Hide(6f);
		}

		[MenuItem(DEBUG_ACTIONS_PATH + "Show and Hide(9 sec)")]
		private static void Debug_ShowAndHide_5_5f(MenuCommand command)
		{
			FlexibleTooltipAnimator component = (FlexibleTooltipAnimator)command.context;
			component.ShowAndHide(3f, 3f, 3f);
		}

		[MenuItem(DEBUG_ACTIONS_PATH + "Show Force")]
		private static void Debug_ShowForce(MenuCommand command)
		{
			FlexibleTooltipAnimator component = (FlexibleTooltipAnimator)command.context;
			component.ShowForce();
		}

		[MenuItem(DEBUG_ACTIONS_PATH + "Hide Force")]
		private static void Debug_HideForce(MenuCommand command)
		{
			FlexibleTooltipAnimator component = (FlexibleTooltipAnimator)command.context;
			component.HideForce();
		}
	}
}
#endif