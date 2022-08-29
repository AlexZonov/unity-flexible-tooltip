using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace com.flexford.packages.tooltip
{
	[ExecuteInEditMode]
	public class FlexibleTooltip : MonoBehaviour
	{
		private static readonly YieldInstruction WAIT_END_OF_FRAME = new WaitForEndOfFrame();

		[SerializeField]
		private FlexibleTooltipViewType _viewType;

		[SerializeField]
		private FlexibleTooltipAlignment _alignment;

		[SerializeField]
		private FlexibleTooltipStyle _style;

		[SerializeField]
		private float _width = 200;

		[SerializeField][HideInInspector]
		private FlexibleTooltipDependencies _dependencies;

		[SerializeField]
		private bool _renderAtUpdate = false;

		private int _lastUpdateViewFrameCount;
		private Coroutine _updateEndOfFrameCoroutine;

		private RectTransform RectTransform => transform as RectTransform;
		private float PixelPerUnit { get; set; }

		public FlexibleTooltipAlignment Alignment => _alignment;

		private void Reset()
		{
			_dependencies = GetComponentInChildren<FlexibleTooltipDependencies>(true);
		}

		private void OnEnable()
		{
			UpdateView();
		}

		private void OnRectTransformDimensionsChange()
		{
			if (_renderAtUpdate)
			{
				return;
			}

			if (_updateEndOfFrameCoroutine != null)
			{
				StopCoroutine(_updateEndOfFrameCoroutine);
			}

			if (gameObject.activeInHierarchy)
			{
				_updateEndOfFrameCoroutine = StartCoroutine(UpdateEndOfFrame());
			}
		}

		private IEnumerator UpdateEndOfFrame()
		{
			if (Application.isPlaying)
			{
				yield return WAIT_END_OF_FRAME;
			}
			else
			{
				yield return null;
			}

			ForceUpdateView();
		}

		private void Update()
		{
			if (_renderAtUpdate)
			{
				UpdateView();
			}
		}

		public void SetAlignment(FlexibleTooltipAlignment alignment)
		{
			_alignment = alignment;
			UpdateView();
		}

		public void ForceUpdateView()
		{
			_lastUpdateViewFrameCount = -1;
			UpdateView();
		}

		public void UpdateView()
		{
			if (_lastUpdateViewFrameCount == Time.frameCount)
			{
				return;
			}

			if (_dependencies == null || _style == null)
			{
				return;
			}

			_lastUpdateViewFrameCount = Time.frameCount;

			Sprite bgSprite = _style.GetBgSprite(_viewType);
			Vector2 styleScale = _style.GetScale(_viewType, _alignment);
			Vector2 baseSize = bgSprite != null ? bgSprite.rect.size : RectTransform.sizeDelta;

			UpdateSize(baseSize);

			UpdateBackgound();
			UpdateFrame();

			// apply reflection
			_dependencies.VerticalGroupTransform.localScale = styleScale;
			_dependencies.ContentTransform.localScale = styleScale;

			UpdatePivot(styleScale, baseSize);
		}

		private void UpdateSize(Vector2 baseSize)
		{
			if (_dependencies != null)
			{
				_dependencies.HorizontalGroupTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _width);
			}

			bool isSizeUpdating = true;
			int iterations = 0;
			do
			{
				RebuildLayout();
				RecalculatePixelPerUnit(baseSize);
				float previousPixelPerUnit = PixelPerUnit;
				_dependencies.VerticalGroup.padding = GetPaddingWithPixelPerUnit();
				RebuildLayout();
				RecalculatePixelPerUnit(baseSize);
				isSizeUpdating = Mathf.Abs(previousPixelPerUnit - PixelPerUnit) > 0.01f;
				iterations++;
			} while (isSizeUpdating);
		}

		private void UpdateBackgound()
		{
			Image bgImage = _dependencies.BgImage;
			if (bgImage != null)
			{
				bgImage.sprite = _style.GetBgSprite(_viewType);
				bgImage.color = _style.GetBgColor(_viewType);
				bgImage.pixelsPerUnitMultiplier = PixelPerUnit;
			}
		}

		private void UpdateFrame()
		{
			Image frameImage = _dependencies.FrameImage;
			if (frameImage != null)
			{
				frameImage.sprite = _style.GetFrameSprite(_viewType);
				frameImage.color = _style.GetFrameColor(_viewType);
				frameImage.pixelsPerUnitMultiplier = PixelPerUnit;
			}
		}

		private void RecalculatePixelPerUnit(Vector2 baseSize)
		{
			Vector2 currentSize = RectTransform.sizeDelta;
			PixelPerUnit = CalculatePixelPerUnit(baseSize, currentSize);
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

			Vector2 currentSize = RectTransform.sizeDelta;
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

			RectTransform.pivot = spritePivot;

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
			LayoutRebuilder.ForceRebuildLayoutImmediate(RectTransform);
		}
	}
}