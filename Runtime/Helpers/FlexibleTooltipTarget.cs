using UnityEngine;

namespace com.flexford.packages.tooltip
{
	[ExecuteInEditMode][DefaultExecutionOrder(10)]
	public class FlexibleTooltipTarget : MonoBehaviour
	{
		[SerializeField]
		private RectTransform _transform;

		[SerializeField]
		private bool _workAtUpdate = true;

		private void Update()
		{
			if (_workAtUpdate)
			{
				UpdatePosition();
			}
		}

		public void UpdatePosition()
		{
			if (_transform != null)
			{
				transform.position = _transform.position;
			}
		}
	}
}