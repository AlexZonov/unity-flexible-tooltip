using System;
using UnityEditor;

namespace com.flexford.packages.tooltip
{
	[CustomEditor(typeof(FlexibleTooltipRectWrapper))]
	public class FlexibleTooltipRectWrapperEditor : Editor
	{
		private FlexibleTooltipRectWrapper _wrapper;
		private SerializedProperty _transformProperty;
		private SerializedProperty _rectProperty;

		private void OnEnable()
		{
			_wrapper = serializedObject.targetObject as FlexibleTooltipRectWrapper;
			_transformProperty = serializedObject.FindProperty("_transform");
			_rectProperty = serializedObject.FindProperty("_rect");
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			serializedObject.Update();

			if (_wrapper.DefaultMode == ContainerWrapMode.Rect)
			{
				_rectProperty.rectValue = EditorGUILayout.RectField(_rectProperty.rectValue);
			}
			else if(_wrapper.DefaultMode == ContainerWrapMode.Transform)
			{
				EditorGUILayout.ObjectField(_transformProperty);
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}