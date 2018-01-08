using UnityEngine;
using UnityEngine.UI;

namespace Uif {
	public class FpsCounter : MonoBehaviour {
		public Text uiText;

		public void OnValidate() {
			if (uiText == null) uiText = GetComponent<Text>();
		}

		public void Update() {
			uiText.text = (1.0f / Time.smoothDeltaTime).ToString("N1");
		}
	}
}