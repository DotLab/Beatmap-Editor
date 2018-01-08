using UnityEngine;
using System.Collections;

namespace Uif {
	[AddComponentMenu("Uif/Swapable/Colorable Swapable")]
	[RequireComponent(typeof(Colorable))]
	public class ColorableSwapable : EasedColorSwapable {
		public Colorable colorable;

		Color lastColor;

		public void OnValidate() {
			colorable = GetComponent<Colorable>();
		}

		protected override bool NeedTransition(Color newItem) {
			return colorable.GetColor() != newItem || lastColor != newItem;
		}

		public override void ForceSwap(Color newItem) {
			colorable.SetColor(newItem);
		}

		Color srcColor, dstColor;

		protected override void PrepareTransition(Color newItem) {
			lastColor = newItem;

			srcColor = colorable.GetColor();
			dstColor = newItem;
		}

		protected override void ApplyTransition(float t) {
			colorable.SetColor(Color.Lerp(srcColor, dstColor, t));
		}

		protected override void FinishTransition() {
			colorable.SetColor(dstColor);
		}
	}
}