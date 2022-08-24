#if F_TOOLTIP_DOTWEEN && UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace com.flexford.packages.tooltip
{
	public partial class FlexibleTooltipAnimator
	{
		[MenuItem("CONTEXT/FlexibleTooltipShower/Show(3 sec)")]
		private static void Debug_Show_3f(MenuCommand command)
		{
			FlexibleTooltipAnimator component = (FlexibleTooltipAnimator)command.context;
			component.Show(3f);
		}

		[MenuItem("CONTEXT/FlexibleTooltipShower/Show(6 sec)")]
		private static void Debug_Show_6f(MenuCommand command)
		{
			FlexibleTooltipAnimator component = (FlexibleTooltipAnimator)command.context;
			component.Show(6f);
		}

		[MenuItem("CONTEXT/FlexibleTooltipShower/Hide(3 sec)")]
		private static void Debug_Hide_3f(MenuCommand command)
		{
			FlexibleTooltipAnimator component = (FlexibleTooltipAnimator)command.context;
			component.Hide(3f);
		}

		[MenuItem("CONTEXT/FlexibleTooltipShower/Hide(6 sec)")]
		private static void Debug_Hide_6f(MenuCommand command)
		{
			FlexibleTooltipAnimator component = (FlexibleTooltipAnimator)command.context;
			component.Hide(6f);
		}

		[MenuItem("CONTEXT/FlexibleTooltipShower/Show and Hide(9 sec)")]
		private static void Debug_ShowAndHide_5_5f(MenuCommand command)
		{
			FlexibleTooltipAnimator component = (FlexibleTooltipAnimator)command.context;
			component.ShowAndHide(3f, 3f, 3f);
		}

		[MenuItem("CONTEXT/FlexibleTooltipShower/Show Force")]
		private static void Debug_ShowForce(MenuCommand command)
		{
			FlexibleTooltipAnimator component = (FlexibleTooltipAnimator)command.context;
			component.ShowForce();
		}

		[MenuItem("CONTEXT/FlexibleTooltipShower/Hide Force")]
		private static void Debug_HideForce(MenuCommand command)
		{
			FlexibleTooltipAnimator component = (FlexibleTooltipAnimator)command.context;
			component.HideForce();
		}
	}
}
#endif