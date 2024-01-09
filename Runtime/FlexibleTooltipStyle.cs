using UnityEngine;

namespace com.flexford.packages.tooltip
{
	[CreateAssetMenu(fileName = "TooltipStyle", menuName = "Flexible Tooltip/Create style/New")]
	public class FlexibleTooltipStyle : ScriptableObject
	{
		[field: SerializeField]
		public FlexibleTooltipStyleViewData Rect { get; private set; }

		[field: SerializeField]
		public FlexibleTooltipStyleViewData Horizontal { get; private set; }

		[field: SerializeField]
		public FlexibleTooltipStyleViewData Vertical { get; private set; }

		public Sprite GetBgSprite(FlexibleTooltipViewType viewType)
		{
			return GetData(viewType)?.BgSprite;
		}

		public Sprite GetFrameSprite(FlexibleTooltipViewType viewType)
		{
			return GetData(viewType)?.FrameSprite;
		}

		public Color GetBgColor(FlexibleTooltipViewType viewType)
		{
			return GetData(viewType)?.BgColor ?? Color.black;
		}

		public Color GetFrameColor(FlexibleTooltipViewType viewType)
		{
			return GetData(viewType)?.FrameColor ?? Color.white;
		}

		public RectOffset GetPadding(FlexibleTooltipViewType viewType)
		{
			var padding = GetData(viewType)?.Padding;
			return new RectOffset(padding.left, padding.right, padding.top, padding.bottom);
		}

		public Vector2 GetSpritePivot(FlexibleTooltipViewType viewType, FlexibleTooltipAlignment alignment)
		{
			FlexibleTooltipStyleViewData styleData = GetData(viewType);
			if (viewType == FlexibleTooltipViewType.Rect || styleData == null)
			{
				return alignment switch
				{
					FlexibleTooltipAlignment.BottomLeft => new Vector2(0, 0),
					FlexibleTooltipAlignment.BottomRight => new Vector2(1, 0),
					FlexibleTooltipAlignment.TopLeft => new Vector2(0, 1),
					FlexibleTooltipAlignment.TopRight => new Vector2(1, 1),
				};
			}
			else
			{
				return styleData.GetSpritePivot();
			}
		}

		public Vector2 GetScale(FlexibleTooltipViewType viewType, FlexibleTooltipAlignment alignment)
		{
			return GetData(viewType)?.GetScale(alignment) ?? Vector2.one;
		}

		private FlexibleTooltipStyleViewData GetData(FlexibleTooltipViewType viewType)
		{
			switch (viewType)
			{
				case FlexibleTooltipViewType.Rect:
					return Rect;
				case FlexibleTooltipViewType.Vertical:
					return Vertical;
				case FlexibleTooltipViewType.Horizontal:
					return Horizontal;
			}

			return null;
		}
	}
}