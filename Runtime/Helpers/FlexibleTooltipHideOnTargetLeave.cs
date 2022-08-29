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
		private RectSide _triggerSide = RectSide.All;

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
			if (!enabled || _areaTransform == null)
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

			bool hasLeftTrigger = _triggerSide.HasFlagFast(RectSide.Left);
			bool hasRightTrigger = _triggerSide.HasFlagFast(RectSide.Right);
			bool hasTopTrigger = _triggerSide.HasFlagFast(RectSide.Top);
			bool hasBottomTrigger = _triggerSide.HasFlagFast(RectSide.Bottom);

			return hasLeftTrigger && targetPosAtPivot.x < areaRect.xMin || 
			       hasRightTrigger && targetPosAtPivot.x > areaRect.xMax ||
			       hasBottomTrigger && targetPosAtPivot.y < areaRect.yMin || 
			       hasTopTrigger && targetPosAtPivot.y > areaRect.yMax;
		}
	}
}