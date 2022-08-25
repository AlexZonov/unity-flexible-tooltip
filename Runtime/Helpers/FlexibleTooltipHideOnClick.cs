using UnityEngine;
using UnityEngine.EventSystems;

namespace com.flexford.packages.tooltip
{
	public class FlexibleTooltipHideOnClick : MonoBehaviour, IPointerClickHandler
	{
		[SerializeField]
		private FlexibleTooltip _tooltip;

		[SerializeField]
		private FlexibleTooltipAnimator _animator;

		public void OnPointerClick(PointerEventData eventData)
		{
			if (_animator != null)
			{
				_animator.Hide();
			}
			else if (_tooltip != null)
			{
				_tooltip.gameObject.SetActive(false);
			}
		}
	}
}