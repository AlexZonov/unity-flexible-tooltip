using UnityEngine;
using UnityEngine.EventSystems;

namespace com.flexford.packages.tooltip
{
	public class FlexibleTooltipHideOnClick : MonoBehaviour, IPointerClickHandler
	{
		[SerializeField]
		private FlexibleTooltipAnimator _animator;

		private void Reset()
		{
			_animator = GetComponent<FlexibleTooltipAnimator>();
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			if (_animator != null)
			{
				_animator.Hide();
			}
			else
			{
				gameObject.SetActive(false);
			}
		}
	}
}