using UnityEngine.UI;

namespace Uif {
	public class TextSwapable : EasedStringSwapable {
		public Text text;

		public void OnValidate() {
			text = GetComponent<Text>();
		}

		StringLerp stringLerp;

		[UnityEngine.ContextMenu("SwapTest")]
		protected void SwapTest() {
			Swap("Test " + UnityEngine.Time.time.ToString("N2"));
		}

		protected override bool NeedTransition(string newItem) {
			return stringLerp == null || stringLerp.Target != newItem;
		}

		public override void ForceSwap(string newItem) {
			text.text = newItem;
			stringLerp = null;
		}

		protected override void PrepareTransition(string newItem) {
			stringLerp = new StringLerp(text.text, newItem);
		}

		protected override void ApplyTransition(float t) {
			text.text = stringLerp.Lerp(t);
		}

		protected override void FinishTransition() {
			text.text = stringLerp.Target;
		}
	}
}