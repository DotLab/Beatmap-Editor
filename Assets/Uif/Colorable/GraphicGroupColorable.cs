using UnityEngine;
using UnityEngine.UI;

namespace Uif {
	[AddComponentMenu("Uif/Colorable/Graphic Group Colorable")]
	public class GraphicGroupColorable : Colorable {
		public bool ExcludeSelf = true;
		public LayerMask ColorableLayers;

		[Space]
		public Color Color = Color.white;

		[Space]
		public Graphic[] graphics;


		public void OnValidate() {
			graphics = GetComponentsInChildren<Graphic>();

			SetColor(Color);
		}

		public override Color GetColor() {
			return Color;
		}

		public override void SetColor(Color newColor) {
			foreach (var graphic in graphics) {
				if (ExcludeSelf && graphic.gameObject == gameObject) continue;
				if (ColorableLayers == (ColorableLayers & (1 << graphic.gameObject.layer))) continue;

				graphic.color = newColor;
			}

			Color = newColor;
		}
	}
}