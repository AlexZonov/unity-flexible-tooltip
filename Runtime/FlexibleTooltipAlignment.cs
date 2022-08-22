using System.ComponentModel;

namespace com.flexford.packages.tooltip
{
	public enum FlexibleTooltipAlignment
	{
		// [Description("Pivot is at the center of the graphic rectangle.")]
		// Center,

		[Description("Pivot is at the top left corner of the graphic rectangle.")]
		TopLeft,

		// [Description("Pivot is at the center of the top edge of the graphic rectangle.")]
		// TopCenter,

		[Description("Pivot is at the top right corner of the graphic rectangle.")]
		TopRight,

		// [Description("Pivot is at the center of the left edge of the graphic rectangle.")]
		// LeftCenter,

		// [Description("Pivot is at the center of the right edge of the graphic rectangle.")]
		// RightCenter,

		[Description("Pivot is at the bottom left corner of the graphic rectangle.")]
		BottomLeft,

		// [Description("Pivot is at the center of the bottom edge of the graphic rectangle.")]
		// BottomCenter,

		[Description("Pivot is at the bottom right corner of the graphic rectangle.")]
		BottomRight
	}
}