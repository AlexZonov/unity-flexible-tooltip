using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace com.flexford.packages.tooltip
{
	[DefaultExecutionOrder(30)]
	public class FlexibleTooltipHideOnTargetLeave : MonoBehaviour
	{
		[SerializeField]
		private FlexibleTooltipAnimator _animator;

		[SerializeField]
		private FlexibleTooltipTarget _target;

		[SerializeField]
		private RectTransform _areaTransform;

		[SerializeField]
		private RectOffset _padding;

		private RectTransform _transform;

		public bool IsLeft => IsLeftImpl();

		private void Awake()
		{
			_transform = transform as RectTransform;
		}

		private void Reset()
		{
			_animator = GetComponent<FlexibleTooltipAnimator>();
			_target = GetComponent<FlexibleTooltipTarget>();
		}

		private void Update()
		{
			if (_target == null || _target.TargetTransform == null || !gameObject.activeSelf)
			{
				return;
			}

			if (_animator != null && (!_animator.IsVisible || _animator.IsHidding))
			{
				return;
			}

			if (IsLeft)
			{
				HideImpl();
			}
		}

		private void OnDrawGizmos()
		{
			if (!enabled)
			{
				return;
			}

			Rect areaRect = _areaTransform.rect;
			FlexibleTooltipUtils.DrawRectTransformGizmos(_areaTransform, areaRect, Color.white);

			Rect areaRectWithPadding = areaRect;
			areaRectWithPadding.xMin += _padding.left;
			areaRectWithPadding.xMax -= _padding.right;
			areaRectWithPadding.yMin += _padding.bottom;
			areaRectWithPadding.yMax -= _padding.top;
			FlexibleTooltipUtils.DrawRectTransformGizmos(_areaTransform, areaRectWithPadding, Color.green);
		}

		private void HideImpl()
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

		private bool IsLeftImpl()
		{
			if (_target == null || _target.TargetTransform == null)
			{
				return false;
			}

			Rect targetRect = FlexibleTooltipUtils.GetWorldRect(_target.TargetTransform);
			Rect areaRect = FlexibleTooltipUtils.GetWorldRect(_areaTransform);
			Vector2 targetPosAtPivot = FlexibleTooltipUtils.GetPositionAtPivot(targetRect, _target.TargetTransform.pivot);
			areaRect.xMin += _padding.left;
			areaRect.xMax -= _padding.right;
			areaRect.yMin += _padding.bottom;
			areaRect.yMax -= _padding.top;

			return targetPosAtPivot.x < areaRect.xMin || targetPosAtPivot.x > areaRect.xMax ||
			       targetPosAtPivot.y < areaRect.yMin || targetPosAtPivot.y > areaRect.yMax;
		}
	}
}