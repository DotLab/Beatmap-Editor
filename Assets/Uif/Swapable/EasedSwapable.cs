using UnityEngine;

namespace Uif {
	public abstract class EasedSwapable <T> : Swapable<T> {
		public EasingType TransitionEasingType = EasingType.Cubic;
		public EasingPhase TransitionEasingPhase = EasingPhase.InOut;
		public float TransitionDuration = 0.5f;

		public override void Swap(T newItem) {
			if (!NeedTransition(newItem)) return;

			StopAllCoroutines();

			PrepareTransition(newItem);
			StartCoroutine(TransitionHandler());
		}

		public void Swap(T newItem, bool forced) {
			if (forced) ForceSwap(newItem);
			else Swap(newItem);
		}

		protected virtual System.Collections.IEnumerator TransitionHandler() {
			float time = 0;

			while (time < TransitionDuration) {
				var easedStep = Easing.Ease(TransitionEasingType, TransitionEasingPhase, time, TransitionDuration);

				ApplyTransition(easedStep);

				time += Time.deltaTime;
				yield return null;
			}

			FinishTransition();
		}

		protected abstract bool NeedTransition(T newItem);
		protected abstract void PrepareTransition(T newItem);
		protected abstract void ApplyTransition(float t);
		protected abstract void FinishTransition();
	}

	public abstract class EasedColorSwapable : EasedSwapable<Color> {

	}

	public abstract class EasedSpriteSwapable : EasedSwapable<Sprite> {

	}

	public abstract class EasedStringSwapable : EasedSwapable<string> {
		
	}
}