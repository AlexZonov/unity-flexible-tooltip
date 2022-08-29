using System;

namespace com.flexford.packages.tooltip
{
	[Flags]
	public enum RectWrapSide
	{
		Left = 1 << 0,
		Right = 1 << 1,
		Top = 1 << 2,
		Bottom = 1 << 3,
		All = Left | Right | Top | Bottom
	}

	public static class RectWrapSideUtils
	{
		public static bool HasFlagFast(this RectWrapSide side, RectWrapSide value)
		{
			return (side & value) == value;
		}
	}
}