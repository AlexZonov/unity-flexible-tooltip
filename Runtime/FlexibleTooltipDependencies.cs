using UnityEngine;
using UnityEngine.UI;

namespace com.flexford.packages.tooltip
{
	public class FlexibleTooltipDependencies : MonoBehaviour
	{
		[field: SerializeField]
		public Canvas Canvas { get; private set; }

		[field: SerializeField]
		public HorizontalLayoutGroup HorizontalGroup { get; private set; }

		[field: SerializeField]
		public RectTransform HorizontalGroupTransform { get; private set; }

		[field: SerializeField]
		public VerticalLayoutGroup VerticalGroup { get; private set; }

		[field: SerializeField]
		public RectTransform VerticalGroupTransform { get; private set; }

		[field: SerializeField]
		public Image BgImage { get; private set; }

		[field: SerializeField]
		public Image FrameImage { get; private set; }

		[field: SerializeField]
		public Text Text { get; private set; }

		[field: SerializeField]
		public RectTransform ContentTransform { get; private set; }
	}
}