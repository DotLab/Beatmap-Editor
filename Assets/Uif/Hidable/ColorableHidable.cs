using UnityEngine;

namespace Uif {
	[AddComponentMenu("Uif/Hidable/Colorable Hidable")]
	[RequireComponent(typeof(Colorable))]
	public class ColorableHidable : EasedHidable {
		[Range(0, 1)]
		public float ShowAlpha = 1;
		[Range(0, 1)]
		public float HideAlpha;

		[Space]
		public Colorable colorable;


		void OnValidate() {
			colorable = GetComponent<Colorable>();
		}

		public override bool Shown() {
			return colorable.GetColor().a == ShowAlpha;
		}

		public override bool Hided() {
			return colorable.GetColor().a == HideAlpha;
		}

		public override void ForceShow() {
			srcColor = colorable.GetColor();
			colorable.SetColor(new Color(srcColor.r, srcColor.g, srcColor.b, ShowAlpha));
		}

		public override void ForceHide() {
			srcColor = colorable.GetColor();
			colorable.SetColor(new Color(srcColor.r, srcColor.g, srcColor.b, HideAlpha));
		}

		Color srcColor, dstColor;

		public override void PrepareShow() {
			srcColor = colorable.GetColor();
			dstColor = new Color(srcColor.r, srcColor.g, srcColor.b, ShowAlpha);
		}

		public override void PrepareHide() {
			srcColor = colorable.GetColor();
			dstColor = new Color(srcColor.r, srcColor.g, srcColor.b, HideAlpha);
		}

		public override void ApplyTransition(float step) {
			colorable.SetColor(Color.Lerp(srcColor, dstColor, step));
		}
	}
}