using UnityEngine;

namespace com.flexford.packages.tooltip
{
	[ExecuteInEditMode] [DefaultExecutionOrder(20)]
	public class FlexibleTooltipRectWrapper : MonoBehaviour
	{
		[SerializeField]
		private FlexibleTooltip _tooltip;

		[SerializeField]
		private ContainerWrapMode _mode;

		[SerializeField] [HideInInspector]
		private RectTransform _transform;

		[SerializeField] [HideInInspector]
		private Rect _rect;

		[SerializeField]
		private bool _wrapAtUpdate = false;

		private RectTransform TooltipTransform => _tooltip?.transform as RectTransform;

		public ContainerWrapMode DefaultMode => _mode;
		public RectTransform DefaultTransform => _transform;
		public Rect DefaultRect => _rect;

		private void OnEnable()
		{
			Wrap();
		}

		private void Reset()
		{
			_tooltip = GetComponentInChildren<FlexibleTooltip>(true);
		}

		private void Update()
		{
			if (_wrapAtUpdate)
			{
				Wrap();
			}
		}

		public void SetCustomTransform(RectTransform rectTransform)
		{
			_transform = rectTransform;
		}

		public void SetCustomRect(Rect rect)
		{
			_rect = rect;
		}

		public void SetMode(ContainerWrapMode mode)
		{
			_mode = mode;
		}

		public void Wrap()
		{
			if (TooltipTransform == null)
			{
				return;
			}

			Rect containerRect = GetTargetRect();
			FlexibleTooltipUtils.WrapInRect(TooltipTransform, containerRect);
		}

		private Rect GetTargetRect()
		{
			Rect GetScreenRect() => new Rect(0, 0, Screen.width, Screen.height);

			if (_mode == ContainerWrapMode.Rect)
			{
				return _rect != default ? _rect : GetScreenRect();
			}
			else if (_mode == ContainerWrapMode.Transform)
			{
				return FlexibleTooltipUtils.GetWorldRect(_transform);
			}
			else if (_mode == ContainerWrapMode.Screen)
			{
				return GetScreenRect();
			}
			else
			{
				Debug.LogError($"Unsupported wrap mode: {_mode}, will return screen rect");
				return GetScreenRect();
			}
		}
	}
}