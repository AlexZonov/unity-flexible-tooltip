using System;

namespace com.flexford.packages.tooltip
{
	[Flags]
	public enum RectSide
	{
		Left = 1 << 0,
		Right = 1 << 1,
		Top = 1 << 2,
		Bottom = 1 << 3,
		All = Left | Right | Top | Bottom
	}

	public static class RectWrapSideUtils
	{
		public static bool HasFlagFast(this RectSide side, RectSide value)
		{
			return (side & value) == value;
		}
	}
}