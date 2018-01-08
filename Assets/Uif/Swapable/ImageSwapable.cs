using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Uif {
	[AddComponentMenu("Uif/Swapable/Image Swapable")]
	[RequireComponent(typeof(Image))]
	public class ImageSwapable : EasedSpriteSwapable {
		public Image MainImage;
		public Image TransitionImage;

	
		void OnValidate() {
			if (MainImage == null) MainImage = GetComponent<Image>();
			if (TransitionImage == null) TransitionImage = transform.Find("Transition").GetComponent<Image>();
		}

		protected override bool NeedTransition(Sprite newItem) {
			return !((MainImage.sprite == newItem && TransitionImage.sprite == null) || (newItem != null && TransitionImage.sprite == newItem));
		}

		public override void ForceSwap(Sprite newItem) {
			if (!NeedTransition(newItem)) return;

			MainImage.sprite = newItem;
		}

		Sprite srcSprite, dstSprite;
		float srcAlpha;

		protected override void PrepareTransition(Sprite newItem) {
			if (TransitionImage.sprite != null) {
				srcSprite = TransitionImage.sprite;
				srcAlpha = TransitionImage.color.a;
			} else {
				srcSprite = MainImage.sprite;
				srcAlpha = MainImage.color.a;
			}

			dstSprite = newItem;

			MainImage.sprite = srcSprite;
			MainImage.color = SetAlpha(MainImage.color, srcAlpha);
			TransitionImage.sprite = dstSprite;
			TransitionImage.color = SetAlpha(MainImage.color, 0);
		}

		protected override void ApplyTransition(float t) {
			MainImage.color = SetAlpha(MainImage.color, srcAlpha - srcAlpha * t);
			TransitionImage.color = SetAlpha(MainImage.color, t);
		}

		protected override void FinishTransition() {
			MainImage.sprite = dstSprite;
			MainImage.color = SetAlpha(MainImage.color, 1);
			TransitionImage.sprite = null;
			TransitionImage.color = Color.clear;
		}

		static Color SetAlpha(Color color, float alpha) {
			color.a = alpha;
			return color;
		}
	}
}