using UnityEngine;

namespace com.flexford.packages.tooltip.editor
{
	[CreateAssetMenu(fileName = "FlexibleTooltipConfig", menuName = "Flexible Tooltip/Create config")]
	public class FlexibleTooltipConfig : ScriptableObject
	{
		public static FlexibleTooltipConfig Instance => _instance == null ? _instance = FlexibleTooltipMenuUtilities.FindScriptableObject<FlexibleTooltipConfig>() : _instance;
		private static FlexibleTooltipConfig _instance;

		[field: SerializeField]
		public GameObject DefaultPrefab { get; private set; }

		[field: SerializeField]
		public FlexibleTooltipStyle DefaultStyleShort { get; private set; }

		[field: SerializeField]
		public FlexibleTooltipStyle DefaultStyleMedium { get; private set; }

		[field: SerializeField]
		public FlexibleTooltipStyle DefaultStyleLong { get; private set; }
	}
}