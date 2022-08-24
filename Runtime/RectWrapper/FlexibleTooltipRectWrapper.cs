using UnityEngine;

namespace com.flexford.packages.tooltip
{
	[ExecuteInEditMode]
	public class FlexibleTooltipRectWrapper : MonoBehaviour
	{
		[SerializeField]
		private FlexibleTooltip _tooltip;

		[SerializeField]
		private ContainerWrapMode _mode;

		// [SerializeField]
		// private bool _adaptiveAlignmentEnabled;

		[SerializeField][HideInInspector]
		private RectTransform _transform;

		[SerializeField][HideInInspector]
		private Rect _rect;

		[SerializeField]
		private bool _wrapAtUpdate = false;

		// [SerializeField]
		// private bool _debugFollowCursor;

		private FlexibleTooltipAlignment DefaultAlignment { get; set; }
		private RectTransform TooltipTransform => _tooltip?.transform as RectTransform;

		public ContainerWrapMode DefaultMode => _mode;
		public RectTransform DefaultTransform => _transform;
		public Rect DefaultRect => _rect;

		private void OnEnable()
		{
			DefaultAlignment = _tooltip.Alignment;
			Wrap();

			// SceneView.duringSceneGui -= OnSceneGui;
			// SceneView.duringSceneGui += OnSceneGui;
		}

		// private void OnSceneGui(SceneView obj)
		// {
		// 	if (Event.current != null && _debugFollowCursor)
		// 	{
		// 		Rect tooltipRect = GetWorldRect(TooltipTransform);
		// 		var world = HandleUtility.GUIPointToScreenPixelCoordinate(Event.current.mousePosition);
		// 		tooltipRect.x = world.x;
		// 		tooltipRect.y = world.y;
		// 		Transform parentTransform = TooltipTransform.parent;
		// 		Vector2 worldPointWithPivot = GetPositionAtPivot(tooltipRect, TooltipTransform.pivot);
		// 		Vector2 localPointWithPivot = parentTransform != null 
		// 			                              ? (Vector2) parentTransform.InverseTransformPoint(worldPointWithPivot) 
		// 			                              : worldPointWithPivot;
		//
		// 		TooltipTransform.localPosition = localPointWithPivot;
		// 	}
		// }

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

		private void Wrap()
		{
			if (TooltipTransform == null)
			{
				return;
			}

			Rect tooltipRect = GetWorldRect(TooltipTransform);
			Rect containerRect = GetTargetRect();
			Vector2 tooltipPivot = TooltipTransform.pivot;

			// if (_adaptiveAlignmentEnabled)
			// {
			// 	TryAdaptAlignment(tooltipRect, tooltipPivot, containerRect);
			// }

			WrapInRect(TooltipTransform, tooltipRect, TooltipTransform.pivot, containerRect);
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
				return GetWorldRect(_transform);
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

		// private bool TryAdaptAlignment(Rect tooltipRect, Vector2 tooltipPivot, Rect containerRect)
		// {
		// 	FlexibleTooltipAlignment alignment = GetCorrectAlignment(tooltipRect, tooltipPivot, containerRect);
		// 	if (_tooltip.Alignment != alignment)
		// 	{
		// 		_tooltip.SetAlignment(alignment);
		// 		return true;
		// 	}
		//
		// 	return false;
		// }

		// private FlexibleTooltipAlignment GetCorrectAlignment(Rect tooltipRect, Vector2 tooltipPivot, Rect containerRect)
		// {
		// 	Vector2 tooltipPositionAtPivot = GetPositionAtPivot(tooltipRect, tooltipPivot);
		// 	bool isLeft = tooltipPositionAtPivot.x <= containerRect.xMin + (tooltipRect.width * tooltipPivot.x) + 1f;
		// 	bool isRight = tooltipPositionAtPivot.x >= containerRect.xMax - (tooltipRect.width * (1f - tooltipPivot.x)) - 1f;
		// 	bool isBottom = tooltipPositionAtPivot.y <= containerRect.yMin + (tooltipRect.height * tooltipPivot.y) + 1f;
		// 	bool isTop = tooltipPositionAtPivot.y >= containerRect.yMax - (tooltipRect.height * (1f - tooltipPivot.y)) -1f;
		//
		// 	FlexibleTooltipAlignment result = _tooltip.Alignment;
		// 	bool isAlignmentTop() => result == FlexibleTooltipAlignment.TopLeft || result == FlexibleTooltipAlignment.TopRight;
		// 	bool isAlignmentLeft() => result == FlexibleTooltipAlignment.TopLeft || result == FlexibleTooltipAlignment.BottomLeft;
		//
		// 	if (isLeft)
		// 	{
		// 		result = isAlignmentTop() ? FlexibleTooltipAlignment.TopLeft : FlexibleTooltipAlignment.BottomLeft;
		// 	}
		// 	else if(isRight)
		// 	{
		// 		result = isAlignmentTop() ? FlexibleTooltipAlignment.TopRight : FlexibleTooltipAlignment.BottomRight;
		// 	}
		//
		// 	if(isTop)
		// 	{
		// 		result = isAlignmentLeft() ? FlexibleTooltipAlignment.TopLeft : FlexibleTooltipAlignment.TopRight;
		// 	}
		// 	else if(isBottom)
		// 	{
		// 		result = isAlignmentLeft() ? FlexibleTooltipAlignment.BottomLeft : FlexibleTooltipAlignment.BottomRight;
		// 	}
		//
		// 	return result;
		// }

		private void WrapInRect(RectTransform tooltipTansform, Rect tooltipRect, Vector2 tooltipPivot, Rect containerRect)
		{
			Rect wrappedRect = GetWrappedRect(tooltipRect, containerRect);
			WrapImpl(tooltipTansform, tooltipPivot, wrappedRect);
		}

		private void WrapImpl(RectTransform tooltipTansform, Vector2 tooltipPivot, Rect wrappedRect)
		{
			Transform parentTransform = tooltipTansform.parent;
			Vector2 worldPointWithPivot = GetPositionAtPivot(wrappedRect, tooltipPivot);
			Vector2 localPointWithPivot = parentTransform != null 
				                              ? (Vector2) parentTransform.InverseTransformPoint(worldPointWithPivot) 
				                              : worldPointWithPivot;

			if ((Vector2)tooltipTansform.localPosition != localPointWithPivot)
			{
				tooltipTansform.localPosition = localPointWithPivot;
			}
		}

		private Rect GetWrappedRect(Rect targetRect, Rect containerRect)
		{
			float x = Mathf.Clamp(targetRect.xMin, containerRect.xMin, containerRect.xMax - targetRect.width);
			float y = Mathf.Clamp(targetRect.yMin, containerRect.yMin, containerRect.yMax - targetRect.height);
			return new Rect(x, y, targetRect.width, targetRect.height);
		}

		private Rect GetWorldRect(RectTransform rectTransform)
		{
			Vector3[] corners = new Vector3[4];
			rectTransform.GetWorldCorners(corners);

			Vector3 botLeftCorner = corners[0];
			Vector3 scale = rectTransform.lossyScale;

			float width = rectTransform.rect.size.x * scale.x;
			float height = rectTransform.rect.size.y * scale.y;
			Vector2 scaledSize = new Vector2(width, height);
 
			return new Rect(botLeftCorner, scaledSize);
		}

		private Vector2 GetPositionAtPivot(Rect rect, Vector2 pivot)
		{
			return GetPositionAtPivot(rect.min, rect.size, pivot);
		}

		private Vector2 GetPositionAtPivot(Vector2 minPosition, Vector2 size, Vector2 pivot)
		{
			float x = minPosition.x + (size.x * pivot.x);
			float y = minPosition.y + (size.y * pivot.y);
			return new Vector2(x, y);
		}
	}
}