using UnityEngine;
using UnityEngine.UI;

namespace Uif {
	[AddComponentMenu("Uif/Colorable/Graphic List Colorable")]
	public class GraphicListColorable : Colorable {
		public Color Color = Color.white;
	
		[Space]
		public Graphic[] Graphics;


		public void OnValidate() {
			SetColor(Color);
		}

		public override Color GetColor() {
			return Color;
		}

		public override void SetColor(Color newColor) {
			foreach (var graphic in Graphics) {
				graphic.color = newColor;
			}

			Color = newColor;
		}
	}
}