using UnityEngine;

namespace Uif {
	public abstract class Colorable : MonoBehaviour {
		public abstract Color GetColor();
	
		public abstract void SetColor(Color newColor);
	}
}
