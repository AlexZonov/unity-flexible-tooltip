using System;
using UnityEngine;
using UnityEngine.UI;

namespace com.flexford.packages.tooltip
{
	[ExecuteInEditMode]
	public class FlexibleTooltip : MonoBehaviour
	{
		[SerializeField]
		private FlexibleTooltipViewType _viewType;

		[SerializeField]
		private FlexibleTooltipAlignment _alignment;

		[SerializeField]
		private FlexibleTooltipStyle _style;

		[SerializeField]
		private float _width = 200;

		[SerializeField]
		private FlexibleTooltipDependencies _dependencies;

		private RectTransform _rectTransform;

		[SerializeField]
		private bool _renderAtUpdate = true;

		private float PixelPerUnit { get; set; }

		private void OnEnable()
		{
			_rectTransform = transform as RectTransform;
			UpdateView();
		}

#if UNITY_EDITOR
		private void Update()
		{
			if (_renderAtUpdate)
			{
				UpdateView();
			}
		}
#endif

		private void UpdateView()
		{
			Sprite bgSprite = _style?.GetBgSprite(_viewType);
			Vector2 styleScale = _style?.GetScale(_viewType, _alignment) ?? Vector2.one;
			Vector2 baseSize = bgSprite != null ? bgSprite.rect.size : _rectTransform.sizeDelta;

			UpdateSize();

			if (_style != null && _dependencies?.BgImage != null)
			{
				_dependencies.BgImage.sprite = bgSprite;
				_dependencies.BgImage.color = _style.GetBgColor(_viewType);
				_dependencies.BgImage.pixelsPerUnitMultiplier = PixelPerUnit;
			}

			if (_style != null && _dependencies?.FrameImage != null)
			{
				_dependencies.FrameImage.sprite = _style.GetFrameSprite(_viewType);
				_dependencies.FrameImage.color = _style.GetFrameColor(_viewType);
				_dependencies.FrameImage.pixelsPerUnitMultiplier = PixelPerUnit;
			}

			if (_style != null && _dependencies != null)
			{
				_dependencies.VerticalGroupTransform.localScale = styleScale;
				_dependencies.ContentTransform.localScale = styleScale;
			}

			UpdatePivot(styleScale, baseSize);
		}

		private void UpdateSize()
		{
			if (_dependencies != null)
			{
				_dependencies.HorizontalGroupTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _width);
				_dependencies.VerticalGroup.childScaleHeight = _viewType == FlexibleTooltipViewType.Horizontal;
				_dependencies.HorizontalGroup.childScaleHeight = _viewType == FlexibleTooltipViewType.Horizontal;
			}

			bool isSizeUpdating = true;
			int iterations = 0;
			do
			{
				RebuildLayout();
				RecalculatePixelPerUnit();
				float previousPixelPerUnit = PixelPerUnit;
				_dependencies.VerticalGroup.padding = GetPaddingWithPixelPerUnit();
				RebuildLayout();
				RecalculatePixelPerUnit();
				isSizeUpdating = Mathf.Abs(previousPixelPerUnit - PixelPerUnit) > 0.001f;
				iterations++;
			} while (isSizeUpdating);
		}

		private void RecalculatePixelPerUnit()
		{
			Sprite bgSprite = _style?.GetBgSprite(_viewType);
			if (bgSprite != null)
			{
				Vector2 baseSize = bgSprite.rect.size;
				Vector2 currentSize = _rectTransform.sizeDelta;
				PixelPerUnit = CalculatePixelPerUnit(baseSize, currentSize);
			}
			else
			{
				PixelPerUnit = 1f;
			}
		}

		private float CalculatePixelPerUnit(Vector2 baseSize, Vector2 currentSize)
		{
			float Calc(float baseSize, float currentSize)
			{
				if (currentSize == 0)
				{
					return 1f;
				}

				return baseSize > currentSize ? baseSize / currentSize : 1f;
			}

			if (_viewType == FlexibleTooltipViewType.Rect)
			{
				return 1f;
			}
			else
			{
				return _viewType == FlexibleTooltipViewType.Vertical
					       ? Calc(baseSize.x, currentSize.x)
					       : Calc(baseSize.y, currentSize.y);
			}
		}

		private void UpdatePivot(Vector2 styleScale, Vector2 baseSize)
		{
			Sprite bgSprite = _style?.GetBgSprite(_viewType);

			if (_style == null || bgSprite == null)
			{
				return;
			}

			float GetRelativeOffset(float baseSize, float currentSize, float pivot)
			{
				return (baseSize * (float) pivot) / (float) currentSize;
			}

			Vector2 currentSize = _rectTransform.sizeDelta;
			Vector2 spritePivot = _style.GetSpritePivot(_viewType, _alignment);

			// add relative offset + pixels per unit offset
			if (_viewType == FlexibleTooltipViewType.Horizontal)
			{
				spritePivot.y = 1f - GetRelativeOffset(baseSize.y, currentSize.y, spritePivot.y) / PixelPerUnit;
			}
			else if (_viewType == FlexibleTooltipViewType.Vertical)
			{
				spritePivot.x = 1f - GetRelativeOffset(baseSize.x, currentSize.x, spritePivot.x) / PixelPerUnit;
			}

			// invert if need
			spritePivot.x = Mathf.Sign(styleScale.x) < 0 ? 1f - spritePivot.x : spritePivot.x;
			spritePivot.y = Mathf.Sign(styleScale.y) < 0 ? 1f - spritePivot.y : spritePivot.y;

			// clump 01
			spritePivot.y = Mathf.Clamp01(spritePivot.y);
			spritePivot.x = Mathf.Clamp01(spritePivot.x);

			_rectTransform.pivot = spritePivot;

			RebuildLayout();
		}

		private RectOffset GetPaddingWithPixelPerUnit()
		{
			if (_style != null && _dependencies?.VerticalGroup != null)
			{
				int GetRelativePadding(int padding)
				{
					return (int)(padding / PixelPerUnit);
				}

				RectOffset result = _style.GetPadding(_viewType);
				result.bottom = GetRelativePadding(result.bottom);
				result.top = GetRelativePadding(result.top);
				result.left = GetRelativePadding(result.left);
				result.right = GetRelativePadding(result.right);
				return result;
			}

			return new RectOffset();
		}

		private void RebuildLayout()
		{
			LayoutRebuilder.ForceRebuildLayoutImmediate(_dependencies.ContentTransform);
			LayoutRebuilder.ForceRebuildLayoutImmediate(_dependencies.VerticalGroupTransform);
			LayoutRebuilder.ForceRebuildLayoutImmediate(_dependencies.HorizontalGroupTransform);
			LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform);
		}
	}
}