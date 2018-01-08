using UnityEngine;

namespace Uif {
	[AddComponentMenu("Uif/Colorable/Camera Colorable")]
	[RequireComponent(typeof(Camera))]
	public class CameraColorable : Colorable {
		public Camera cam;


		public void OnValidate() {
			cam = GetComponent<Camera>();
		}

		public override Color GetColor() {
			return cam.backgroundColor;
		}

		public override void SetColor(Color newColor) {
			cam.backgroundColor = newColor;
		}
	}
}