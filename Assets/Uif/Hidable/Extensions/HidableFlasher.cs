using UnityEngine;
using System.Collections;

namespace Uif {
	[AddComponentMenu("Uif/Extension/Hidable Flasher")]
	[RequireComponent(typeof(EasedHidable))]
	public class HidableFlasher : MonoBehaviour {
		public EasedHidable Hidable;

		public float ShowWait = 0.5f;
		public float HideWait = 0.5f;

		public bool InverseOrder;

		void OnValidate() {
			Hidable = GetComponent<EasedHidable>();
		}
	
		void Start() {
			StartCoroutine(FlashHandler());
		}

		public void Stop() {
			StopAllCoroutines();
		}

		IEnumerator FlashHandler() {
			if (InverseOrder) while (true) {
					Hidable.Hide();
					yield return new WaitForSeconds(HideWait);

					Hidable.Show();
					yield return new WaitForSeconds(ShowWait);
				}
			
			while (true) {
				Hidable.Show();
				yield return new WaitForSeconds(ShowWait);

				Hidable.Hide();
				yield return new WaitForSeconds(HideWait);
			}
		}
	}
}