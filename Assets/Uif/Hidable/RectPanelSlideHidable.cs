using UnityEngine;

namespace Uif {
	[AddComponentMenu("Uif/Hidable/Rect Panel Slide Hidable")]
	public class RectPanelSlideHidable : EasedHidable {
		[Header("Lower Left : Upper Right")]
		public Vector2 ShowMin;
		public Vector2 ShowMax;

		[Header("Lower Left : Upper Right")]
		public Vector2 HideMin;
		public Vector2 HideMax;

		public RectTransform rect;


		public void OnValidate() {
			rect = GetComponent<RectTransform>();
		}

		[ContextMenu("RecordShowOffset")]
		public void RecordShowOffset() {
			if (rect == null) rect = GetComponent<RectTransform>();
			ShowMin = rect.offsetMin;
			ShowMax = rect.offsetMax;
		}

		[ContextMenu("RecordHideOffset")]
		public void RecordHideOffset() {
			if (rect == null) rect = GetComponent<RectTransform>();
			HideMin = rect.offsetMin;
			HideMax = rect.offsetMax;
		}

		public override bool Shown() {
			return rect.offsetMin == ShowMin && rect.offsetMax != ShowMax;
		}

		public override bool Hided() {
			return rect.offsetMin == HideMin && rect.offsetMax != HideMax;
		}

		public override void ForceShow() {
			rect.offsetMin = ShowMin;
			rect.offsetMax = ShowMax;
		}

		public override void ForceHide() {
			rect.offsetMin = HideMin;
			rect.offsetMax = HideMax;
		}

		Vector2 startMin, startMax, endMin, endMax;

		public override void PrepareShow() {
			startMin = rect.offsetMin;
			startMax = rect.offsetMax;
			endMin = ShowMin;
			endMax = ShowMax;
		}

		public override void PrepareHide() {
			startMin = rect.offsetMin;
			startMax = rect.offsetMax;
			endMin = HideMin;
			endMax = HideMax;
		}

		public override void ApplyTransition(float step) {
			rect.offsetMin = Vector2.Lerp(startMin, endMin, step);
			rect.offsetMax = Vector2.Lerp(startMax, endMax, step);
		}
	}
}