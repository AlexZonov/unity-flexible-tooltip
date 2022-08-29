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

		public RectTransform TargetTransform => _transform;

		private void Update()
		{
			if (_workAtUpdate)
			{
				UpdatePosition();
			}
		}

		public void SetTarget(Transform targetTransform)
		{
			SetTarget(targetTransform as RectTransform);
		}

		public void SetTarget(RectTransform targetTransform)
		{
			_transform = targetTransform;
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