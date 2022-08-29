using UnityEngine;

namespace com.flexford.packages.tooltip
{
	public static class FlexibleTooltipUtils
	{
		public static void WrapInRect(RectTransform targetTansform, Rect containerRect, RectSide side = RectSide.All)
		{
			Rect targetRect = GetWorldRect(targetTansform);
			Rect wrappedRect = GetWrappedRect(targetRect, containerRect, side);
			Wrap(targetTansform, wrappedRect);
		}

		public static void Wrap(RectTransform targetTansform, Rect wrappedRect)
		{
			Transform parentTransform = targetTansform.parent;
			Vector2 worldPointWithPivot = GetPositionAtPivot(wrappedRect, targetTansform.pivot);
			Vector2 localPointWithPivot = parentTransform != null 
				                              ? (Vector2) parentTransform.InverseTransformPoint(worldPointWithPivot) 
				                              : worldPointWithPivot;

			if ((Vector2)targetTansform.localPosition != localPointWithPivot)
			{
				targetTansform.localPosition = localPointWithPivot;
			}
		}

		public static Rect GetWrappedRect(Rect targetRect, Rect containerRect, RectSide side = RectSide.All)
		{
			float x = Mathf.Clamp(targetRect.xMin, 
			                      side.HasFlagFast(RectSide.Left) ? containerRect.xMin : targetRect.xMin,
			                      side.HasFlagFast(RectSide.Right) ? (containerRect.xMax - targetRect.width) : targetRect.xMax);

			float y = Mathf.Clamp(targetRect.yMin, 
			                      side.HasFlagFast(RectSide.Bottom) ? containerRect.yMin : targetRect.yMin, 
			                      side.HasFlagFast(RectSide.Top) ? (containerRect.yMax - targetRect.height) : targetRect.yMax);

			return new Rect(x, y, targetRect.width, targetRect.height);
		}

		public static Rect GetWorldRect(RectTransform rectTransform)
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

		public static Vector2 GetPositionAtPivot(Rect rect, Vector2 pivot)
		{
			return GetPositionAtPivot(rect.min, rect.size, pivot);
		}

		public static Vector2 GetPositionAtPivot(Vector2 minPosition, Vector2 size, Vector2 pivot)
		{
			float x = minPosition.x + (size.x * pivot.x);
			float y = minPosition.y + (size.y * pivot.y);
			return new Vector2(x, y);
		}

		public static void DrawRectTransformGizmos(RectTransform rectTransform, Rect rect, Color color)
		{
			Color prevColor = Gizmos.color;
			Matrix4x4 prevMatrix = Gizmos.matrix;
			Gizmos.matrix = rectTransform.localToWorldMatrix;
			Gizmos.color = color;
			{
				Vector3 pos = rect.position;
				pos.x += rect.size.x / 2f;
				pos.y += rect.size.y / 2f;
				Gizmos.DrawWireCube(pos, rect.size);
			}
			Gizmos.color = prevColor;
			Gizmos.matrix = prevMatrix;
		}
	}
}