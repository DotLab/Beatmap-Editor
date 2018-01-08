using System.Collections;
using UnityEngine;

namespace Uif {
	public class SequencialHidable : Hidable {
		[System.Serializable]
		public class HidableKey {
			public Hidable Hidable;
			public float Delay;

			public HidableKey(Hidable hidable, float delay) {
				Hidable = hidable;
				Delay = delay;
			}
		}

		public bool Enforce = true;
		public bool Refresh;

		public HidableKey[] HidableKeys;

		public void OnValidate() {
			var hidables = GetComponentsInChildren<Hidable>();
			if (HidableKeys.Length != hidables.Length - 1 || Refresh) {
				Refresh = false;

				var keys = new HidableKey[hidables.Length - 1];
				int i = 0;
				foreach (var hidable in hidables) if (hidable != this) {
						int j;
						for (j = 0; j < HidableKeys.Length; j++) if (hidable == HidableKeys[j].Hidable) break;
						
						keys[i++] = j < HidableKeys.Length ? HidableKeys[j] : new HidableKey(hidable, 0);
					}
				HidableKeys = keys;
			}

			if (Enforce) foreach (var key in HidableKeys) {
					key.Hidable.StartState = StartState;
					key.Hidable.StartAction = StartAction;
				}
		}

		public override bool Shown() {
			return HidableKeys[HidableKeys.Length - 1].Hidable.Shown();
		}

		public override bool Hided() {
			return HidableKeys[HidableKeys.Length - 1].Hidable.Hided();
		}

		public override void Show() {
			StartCoroutine(SetHandler(true));
		}

		public override void Hide() {
			StartCoroutine(SetHandler(false));
		}

		IEnumerator SetHandler(bool show) {
			foreach (var key in HidableKeys) {
				if (key.Delay > 0) yield return new WaitForSeconds(key.Delay);

				key.Hidable.Set(show);
			}
		}

		public override void ForceShow() {
			foreach (var key in HidableKeys) key.Hidable.ForceShow();
		}

		public override void ForceHide() {
			foreach (var key in HidableKeys) key.Hidable.ForceHide();
		}
	}
}

