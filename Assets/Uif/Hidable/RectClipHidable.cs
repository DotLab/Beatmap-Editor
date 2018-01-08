using UnityEngine;

namespace Uif {
	[AddComponentMenu("Uif/Hidable/Rect Clip Hidable")]
	public class RectClipHidable : EasedHidable {
		public bool LockWidth;
		public float ShowWidth;
		public float HideWidth;

		[Space]
		public bool LockHeight;
		public float ShowHeight;
		public float HideHeight;

		[Space]
		public RectTransform rect;


		public void OnValidate() {
			rect = GetComponent<RectTransform>();
		}

		public override bool Shown() {
			return rect.sizeDelta == new Vector2(ShowWidth, ShowHeight);
		}

		public override bool Hided() {
			return rect.sizeDelta == new Vector2(HideHeight, HideWidth);
		}

		public override void ForceShow() {
			rect.sizeDelta = new Vector2(
				LockWidth ? rect.sizeDelta.x : ShowWidth,
				LockHeight ? rect.sizeDelta.y : ShowHeight);
		}

		public override void ForceHide() {
			rect.sizeDelta = new Vector2(
				LockWidth ? rect.sizeDelta.x : HideWidth, 
				LockHeight ? rect.sizeDelta.y : HideHeight);
		}

		Vector2 startSize, endSize;

		public override void PrepareShow() {
			startSize = rect.sizeDelta;
			endSize = new Vector2(
				LockWidth ? rect.sizeDelta.x : ShowWidth,
				LockHeight ? rect.sizeDelta.y : ShowHeight);
		}

		public override void PrepareHide() {
			startSize = rect.sizeDelta;
			endSize = new Vector2(
				LockWidth ? rect.sizeDelta.x : HideWidth, 
				LockHeight ? rect.sizeDelta.y : HideHeight);
		}

		public override void ApplyTransition(float step) {
			rect.sizeDelta = Vector2.Lerp(startSize, endSize, step);
		}
	}
}