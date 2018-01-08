using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Uif {
	public static class EditorExtension {
#if UNITY_EDITOR
		[MenuItem("Kailang/Normalize UI Scale")]
		public static void NormalizeUiScale() {
			NormalizeUiScale(Selection.activeTransform.GetComponent<RectTransform>());
		}

		[MenuItem("Kailang/Normalize UI Scale Recursive")]
		public static void NormalizeUiScaleRecursive() {
			var tran = Selection.activeTransform;
			foreach (var rectTrans in tran.GetComponentsInChildren<RectTransform>()) {
				NormalizeUiScale(rectTrans);
			}
		}

		[MenuItem("Kailang/Unset Raycast Target")]
		public static void UnsetRaycastTarget() {
			foreach (var graphic in Selection.activeTransform.GetComponentsInChildren<UnityEngine.UI.Graphic>()) {
				graphic.raycastTarget = false;
			}
		}

		[MenuItem("Kailang/Set Raycast Target")]
		public static void SetRaycastTarget() {
			foreach (var graphic in Selection.activeTransform.GetComponentsInChildren<UnityEngine.UI.Graphic>()) {
				graphic.raycastTarget = true;
			}
		}
#endif

		public const float SnapStep = 0.5f;

		public static void NormalizeUiScale(RectTransform rectTrans) {
			var position = rectTrans.anchoredPosition;
			position.x = Mathf.Round(position.x / SnapStep) * SnapStep;
			position.y = Mathf.Round(position.y / SnapStep) * SnapStep;
			rectTrans.anchoredPosition = Vector2.zero;
			rectTrans.anchoredPosition = Vector2.one;
			rectTrans.anchoredPosition = position;

			var size = rectTrans.sizeDelta;
			size.x = Mathf.Round(size.x / SnapStep) * SnapStep;
			size.y = Mathf.Round(size.y / SnapStep) * SnapStep;
			rectTrans.sizeDelta = Vector2.zero;
			rectTrans.sizeDelta = Vector2.one;
			rectTrans.sizeDelta = size;

			rectTrans.localScale = Vector2.zero;
			rectTrans.localScale = Vector2.one;
		}

		public static void DeleteAllChildre(Transform transform) {
			var count = transform.childCount;
			var children = new GameObject[count];
			for (int i = 0; i < count; i++) {
				children[i] = transform.GetChild(i).gameObject;
			}
			transform.DetachChildren();

#if UNITY_EDITOR
			EditorApplication.delayCall += () => {
				for (int i = 0; i < count; i++) {
					Object.DestroyImmediate(children[i]);
				}
			};
#else
		for (int i = 0; i < count; i++) {
		Object.DestroyImmediate(children[i]);
		}
#endif
		}
	}
}