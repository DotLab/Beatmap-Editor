using UnityEngine;

namespace Uif {
	[AddComponentMenu("Uif/Hidable/Rect Slide Hidable")]
	public class RectSlideHidable : EasedHidable {
		[Space]
		public bool LockX;
		public float ShowX;
		public float HideX;

		[Space]
		public bool LockY;
		public float ShowY;
		public float HideY;

		public RectTransform rect;


		public void OnValidate() {
			rect = GetComponent<RectTransform>();
		}

		public override bool Shown() {
			return rect.anchoredPosition == new Vector2(ShowX, ShowY);
		}

		public override bool Hided() {
			return rect.anchoredPosition == new Vector2(HideX, HideY);
		}

		public override void ForceShow() {
			rect.anchoredPosition = new Vector2(
				LockX ? rect.anchoredPosition.x : ShowX,
				LockY ? rect.anchoredPosition.y : ShowY);
		}

		public override void ForceHide() {
			rect.anchoredPosition = new Vector2(
				LockX ? rect.anchoredPosition.x : HideX, 
				LockY ? rect.anchoredPosition.y : HideY);
		}

		Vector2 startPosition, endPosition;

		public override void PrepareShow() {
			startPosition = rect.anchoredPosition;
			endPosition = new Vector2(
				LockX ? rect.anchoredPosition.x : ShowX,
				LockY ? rect.anchoredPosition.y : ShowY);
		}

		public override void PrepareHide() {
			startPosition = rect.anchoredPosition;
			endPosition = new Vector2(
				LockX ? rect.anchoredPosition.x : HideX, 
				LockY ? rect.anchoredPosition.y : HideY);
		}

		public override void ApplyTransition(float step) {
			rect.anchoredPosition = Vector2.Lerp(startPosition, endPosition, step);
		}
	}
}