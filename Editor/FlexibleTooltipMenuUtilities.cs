﻿using System.IO;
using System.Linq;
using UnityEditor;

#if UNITY_2021_1_OR_NEWER
	using UnityEditor.SceneManagement;
#else
	using UnityEditor.SceneManagement;
	using UnityEditor.Experimental.SceneManagement;
#endif

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.flexford.packages.tooltip.editor
{
	internal static class FlexibleTooltipMenuUtilities
	{
		public static string GetProjectWindowFolder()
		{
			string projectPath = new DirectoryInfo(Application.dataPath).Parent.FullName + "/";
			string objectProjectPath = AssetDatabase.GetAssetPath(Selection.activeObject);
			string objectAbsolutePath = string.IsNullOrEmpty(objectProjectPath) ? Application.dataPath : $"{projectPath}{objectProjectPath}";
			string objectCorrectAbsolutePath = objectAbsolutePath.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
			string folderAbsolutePath = File.Exists(objectCorrectAbsolutePath) ? Path.GetDirectoryName(objectCorrectAbsolutePath) : objectCorrectAbsolutePath;

			string GetRelativePath(string relativeTo, string path)
			{
				return path.Substring(relativeTo.Length);
			}

			return GetRelativePath(projectPath, folderAbsolutePath);
		}

		public static T FindScriptableObject<T>() where T : ScriptableObject
		{
			var guids = AssetDatabase.FindAssets($"t:{typeof(T)}");
			if (guids.Length == 0)
			{
				return null;
			}

			if (guids.Length > 1)
			{
				Debug.LogError("Found multiple configs(will use first):");
				for (int i = 0; i < guids.Length; i++)
				{
					string guid = guids[i];
					Debug.LogError($"\t{i + 1}) Path: '{AssetDatabase.GUIDToAssetPath(guid)}'");
				}
			}

			string path = AssetDatabase.GUIDToAssetPath(guids[0]);
			if (string.IsNullOrEmpty(path))
			{
				return null;
			}

			return AssetDatabase.LoadAssetAtPath<T>(path);
		}

		public static void PlaceUIElementRoot(GameObject element, MenuCommand menuCommand)
		{
			GameObject parent = menuCommand.context as GameObject;
			bool explicitParentChoice = true;
			if (parent == null)
			{
				parent = GetOrCreateCanvasGameObject();
				explicitParentChoice = false;

				// If in Prefab Mode, Canvas has to be part of Prefab contents,
				// otherwise use Prefab root instead.
				PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
				if (prefabStage != null && !prefabStage.IsPartOfPrefabContents(parent))
					parent = prefabStage.prefabContentsRoot;
			}

			if (parent.GetComponentsInParent<Canvas>(true).Length == 0)
			{
				// Create canvas under context GameObject,
				// and make that be the parent which UI element is added under.
				GameObject canvas = CreateNewUI();
				Undo.SetTransformParent(canvas.transform, parent.transform, "");
				parent = canvas;
			}

			GameObjectUtility.EnsureUniqueNameForSibling(element);
			GameObjectUtility.SetParentAndAlign(element, parent);

			if (!explicitParentChoice) // not a context click, so center in sceneview
				SetPositionVisibleinSceneView(parent.GetComponent<RectTransform>(), element.GetComponent<RectTransform>());

			// This call ensure any change made to created Objects after they where registered will be part of the Undo.
			Undo.RegisterFullObjectHierarchyUndo(parent == null ? element : parent, "");

			// We have to fix up the undo name since the name of the object was only known after reparenting it.
			Undo.SetCurrentGroupName("Create " + element.name);

			Selection.activeGameObject = element;
		}

		private static GameObject GetOrCreateCanvasGameObject()
		{
			GameObject selectedGo = Selection.activeGameObject;

			// Try to find a gameobject that is the selected GO or one if its parents.
			Canvas canvas = (selectedGo != null) ? selectedGo.GetComponentInParent<Canvas>() : null;
			if (IsValidCanvas(canvas))
				return canvas.gameObject;

			// No canvas in the scene at all? Then create a new one.
			return CreateNewUI();
		}

		private static bool IsValidCanvas(Canvas canvas)
		{
			if (canvas == null || !canvas.gameObject.activeInHierarchy)
				return false;

			// It's important that the non-editable canvas from a prefab scene won't be rejected,
			// but canvases not visible in the Hierarchy at all do. Don't check for HideAndDontSave.
			if (EditorUtility.IsPersistent(canvas) || (canvas.hideFlags & HideFlags.HideInHierarchy) != 0)
				return false;

			if (StageUtility.GetStageHandle(canvas.gameObject) != StageUtility.GetCurrentStageHandle())
				return false;

			return true;
		}

		private static GameObject CreateNewUI()
		{
			// Root for the UI
			var root = new GameObject("Canvas");
			root.layer = LayerMask.NameToLayer("UI");
			Canvas canvas = root.AddComponent<Canvas>();
			canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			root.AddComponent<CanvasScaler>();
			root.AddComponent<GraphicRaycaster>();

			// Works for all stages.
			StageUtility.PlaceGameObjectInCurrentStage(root);
			bool customScene = false;
			PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
			if (prefabStage != null)
			{
				root.transform.SetParent(prefabStage.prefabContentsRoot.transform, false);
				customScene = true;
			}

			Undo.RegisterCreatedObjectUndo(root, "Create " + root.name);

			// If there is no event system add one...
			// No need to place event system in custom scene as these are temporary anyway.
			// It can be argued for or against placing it in the user scenes,
			// but let's not modify scene user is not currently looking at.
			if (!customScene)
			{
				CreateEventSystem(false);
			}

			return root;
		}

		private static void CreateEventSystem(bool select)
		{
			CreateEventSystem(select, null);
		}

		private static void CreateEventSystem(bool select, GameObject parent)
		{
			var esys = Object.FindObjectOfType<EventSystem>();
			if (esys == null)
			{
				var eventSystem = new GameObject("EventSystem");
				GameObjectUtility.SetParentAndAlign(eventSystem, parent);
				esys = eventSystem.AddComponent<EventSystem>();
				eventSystem.AddComponent<StandaloneInputModule>();

				Undo.RegisterCreatedObjectUndo(eventSystem, "Create " + eventSystem.name);
			}

			if (select && esys != null)
			{
				Selection.activeGameObject = esys.gameObject;
			}
		}

		private static void SetPositionVisibleinSceneView(RectTransform canvasRTransform, RectTransform itemTransform)
		{
			// Find the best scene view
			SceneView sceneView = SceneView.lastActiveSceneView;

			if (sceneView == null && SceneView.sceneViews.Count > 0)
				sceneView = SceneView.sceneViews[0] as SceneView;

			// Couldn't find a SceneView. Don't set position.
			if (sceneView == null || sceneView.camera == null)
				return;

			// Create world space Plane from canvas position.
			Camera camera = sceneView.camera;
			Vector3 position = Vector3.zero;
			Vector2 localPlanePosition;

			if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRTransform, new Vector2(camera.pixelWidth / 2, camera.pixelHeight / 2), camera, out localPlanePosition))
			{
				// Adjust for canvas pivot
				localPlanePosition.x = localPlanePosition.x + canvasRTransform.sizeDelta.x * canvasRTransform.pivot.x;
				localPlanePosition.y = localPlanePosition.y + canvasRTransform.sizeDelta.y * canvasRTransform.pivot.y;

				localPlanePosition.x = Mathf.Clamp(localPlanePosition.x, 0, canvasRTransform.sizeDelta.x);
				localPlanePosition.y = Mathf.Clamp(localPlanePosition.y, 0, canvasRTransform.sizeDelta.y);

				// Adjust for anchoring
				position.x = localPlanePosition.x - canvasRTransform.sizeDelta.x * itemTransform.anchorMin.x;
				position.y = localPlanePosition.y - canvasRTransform.sizeDelta.y * itemTransform.anchorMin.y;

				Vector3 minLocalPosition;
				minLocalPosition.x = canvasRTransform.sizeDelta.x * (0 - canvasRTransform.pivot.x) + itemTransform.sizeDelta.x * itemTransform.pivot.x;
				minLocalPosition.y = canvasRTransform.sizeDelta.y * (0 - canvasRTransform.pivot.y) + itemTransform.sizeDelta.y * itemTransform.pivot.y;

				Vector3 maxLocalPosition;
				maxLocalPosition.x = canvasRTransform.sizeDelta.x * (1 - canvasRTransform.pivot.x) - itemTransform.sizeDelta.x * itemTransform.pivot.x;
				maxLocalPosition.y = canvasRTransform.sizeDelta.y * (1 - canvasRTransform.pivot.y) - itemTransform.sizeDelta.y * itemTransform.pivot.y;

				position.x = Mathf.Clamp(position.x, minLocalPosition.x, maxLocalPosition.x);
				position.y = Mathf.Clamp(position.y, minLocalPosition.y, maxLocalPosition.y);
			}

			itemTransform.anchoredPosition = position;
			itemTransform.localRotation = Quaternion.identity;
			itemTransform.localScale = Vector3.one;
		}
	}
}