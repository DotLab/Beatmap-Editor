using UnityEngine;
using System.Collections;

namespace Uif {
	public abstract class EasedHidable : Hidable {
		public EasingType TransitionEasingType = EasingType.Cubic;
		public EasingPhase TransitionEasingPhase = EasingPhase.InOut;
		public float TransitionDuration = 0.5f;

		public System.Action Callback;
	
		public override void Show() {
			if (!Shown()) {
				StopAllCoroutines();

				PrepareShow();
				StartCoroutine(TransitionHandler());
			}
		}

		public override void Hide() {
			if (!Hided()) {
				StopAllCoroutines();

				PrepareHide();
				StartCoroutine(TransitionHandler());
			}
		}

		protected virtual IEnumerator TransitionHandler() {
			float time = 0;

			while (time < TransitionDuration) {
				var easedStep = Easing.Ease(TransitionEasingType, TransitionEasingPhase, time, TransitionDuration);

				ApplyTransition(easedStep);

				time += Time.deltaTime;
				yield return null;
			}

			ApplyTransition(1);

			FinishTransition();
		}

		public abstract void PrepareShow();

		public abstract void PrepareHide();

		public abstract void ApplyTransition(float step);

		public void FinishTransition() {
			if (Callback != null) {
				Callback();
				Callback = null;
			}
		}
	}
}