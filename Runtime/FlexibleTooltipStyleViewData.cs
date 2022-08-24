using System;
using UnityEngine;

namespace com.flexford.packages.tooltip
{
	[Serializable]
	public class FlexibleTooltipStyleViewData
	{
		[field: SerializeField]
		public Sprite BgSprite { get; private set; }

		[field: SerializeField]
		public Sprite FrameSprite { get; private set; }

		[field: SerializeField]
		public Color BgColor { get; private set; }

		[field: SerializeField]
		public Color FrameColor { get; private set; }

		[field: SerializeField]
		public RectOffset Padding { get; private set; } = new RectOffset();

		[field: SerializeField]
		public ScaleData Scale { get; private set; } = new ScaleData();

		public Vector2 GetScale(FlexibleTooltipAlignment alignment)
		{
			switch (alignment)
			{
				case FlexibleTooltipAlignment.BottomLeft:
					return Scale.BottomLeft;
				case FlexibleTooltipAlignment.BottomRight:
					return Scale.BottomRight;
				case FlexibleTooltipAlignment.TopLeft:
					return Scale.TopLeft;
				case FlexibleTooltipAlignment.TopRight:
					return Scale.TopRight;
			}

			return Vector2.one;
		}

		public Vector2 GetSpritePivot()
		{
			return BgSprite != null ? (BgSprite.pivot / BgSprite.rect.size) : (Vector2.one / 2f);
		}

		[Serializable]
		public class ScaleData
		{
			[field: SerializeField]
			public Vector2 TopLeft { get; private set; } = Vector2.one;

			[field: SerializeField]
			public Vector2 TopRight { get; private set; } = Vector2.one;

			[field: SerializeField]
			public Vector2 BottomLeft { get; private set; } = Vector2.one;

			[field: SerializeField]
			public Vector2 BottomRight { get; private set; } = Vector2.one;
		}
	}
}