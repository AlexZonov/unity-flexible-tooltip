using UnityEngine;

namespace com.flexford.packages.tooltip
{
	[ExecuteInEditMode][DefaultExecutionOrder(10)]
	public class FlexibleTooltipTarget : MonoBehaviour
	{
		[SerializeField]
		private RectTransform _transform;

		[SerializeField]
		private Vector2 _offset;

		[SerializeField]
		private bool _workAtUpdate = true;

		private Vector2 _lastPosition;

		public RectTransform TargetRectTransform => _transform;
		public RectTransform RectTransform => transform as RectTransform;

		private void OnEnable()
		{
			UpdatePosition();
		}

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
			UpdatePosition();
		}

		public void SetOffset(Vector2 offset)
		{
			_offset = offset;
			UpdatePosition();
		}

		public void UpdatePosition()
		{
			if (TargetRectTransform == null || RectTransform == null)
			{
				return;
			}

			if (TargetRectTransform.position.x == _lastPosition.x &&
			    TargetRectTransform.position.y == _lastPosition.y)
			{
				return;
			}

			if (_offset != Vector2.zero)
			{
				Rect targetTransformWorldRect = FlexibleTooltipUtils.GetWorldRect(TargetRectTransform);
				targetTransformWorldRect.xMin += _offset.x;
				targetTransformWorldRect.yMin += _offset.y;

				Transform parentTransform = RectTransform.parent;
				Vector2 worldPointWithPivot = FlexibleTooltipUtils.GetPositionAtPivot(targetTransformWorldRect, TargetRectTransform.pivot);
				Vector2 localPointWithPivot = parentTransform != null 
					                              ? (Vector2) parentTransform.InverseTransformPoint(worldPointWithPivot) 
					                              : worldPointWithPivot;
					
				RectTransform.localPosition = localPointWithPivot;
			}
			else
			{
				RectTransform.position = TargetRectTransform.position;
			}

			_lastPosition = RectTransform.position;
		}
	}
}