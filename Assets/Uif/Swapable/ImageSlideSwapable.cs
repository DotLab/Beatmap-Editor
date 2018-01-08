using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Uif {
	[AddComponentMenu("Uif/Swapable/Image Slide Swapable")]
	[RequireComponent(typeof(Image))]
	public class ImageSlideSwapable : EasedSpriteSwapable {
		public Image MainImage;
		public Image TransitionImage;

		public bool LockX;
		public float StartX;
		public float EndX;

		public bool LockY;
		public float StartY;
		public float EndY;
	
		public RectTransform trans;

		void OnValidate() {
			trans = TransitionImage.GetComponent<RectTransform>();
		
			if (MainImage == null) MainImage = GetComponent<Image>();
			if (TransitionImage == null) TransitionImage = transform.Find("Transition").GetComponent<Image>();
		}

		protected override bool NeedTransition(Sprite newItem) {
			return !((MainImage.sprite == newItem && TransitionImage.sprite == null) || TransitionImage.sprite == newItem);
		}

		public override void ForceSwap(Sprite newItem) {
			if (!NeedTransition(newItem)) return;

			MainImage.sprite = newItem;
		}

		Sprite srcSprite, dstSprite;
		float srcAlpha;

		protected override void PrepareTransition(Sprite newItem) {
			srcSprite = MainImage.sprite;
			srcAlpha = MainImage.color.a;
			dstSprite = newItem;

			MainImage.sprite = srcSprite;
			MainImage.color = SetAlpha(MainImage.color, srcAlpha);
			TransitionImage.sprite = dstSprite;
			TransitionImage.color = SetAlpha(MainImage.color, 0);
		}

		protected override void ApplyTransition(float t) {
			MainImage.color = SetAlpha(MainImage.color, srcAlpha - srcAlpha * t);
			TransitionImage.color = SetAlpha(MainImage.color, t);
			trans.anchoredPosition = new Vector2(
				LockX ? trans.anchoredPosition.x : Mathf.Lerp(StartX, EndX, t),
				LockY ? trans.anchoredPosition.y : Mathf.Lerp(StartY, EndY, t));
		}

		protected override void FinishTransition() {
			MainImage.sprite = dstSprite;
			MainImage.color = SetAlpha(MainImage.color, 1);
			TransitionImage.sprite = null;
			TransitionImage.color = Color.clear;
			trans.anchoredPosition = new Vector2(
				LockX ? trans.anchoredPosition.x : StartY,
				LockY ? trans.anchoredPosition.y : StartY);
		}

		static Color SetAlpha(Color baseColor, float alpha) {
			baseColor.a = alpha;
			return baseColor;
		}
	}
}