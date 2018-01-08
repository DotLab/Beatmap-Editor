using UnityEngine;

namespace Uif {
	public enum HidableState {
		None,
		Shown,
		Hided
	}

	public enum HidableAction {
		None,
		Show,
		Hide
	}

	public abstract class Hidable : MonoBehaviour {
		public const string ShowMessage = "Show";
		public const string HideMessage = "Hide";

		public HidableState StartState;
		public HidableAction StartAction;

		public virtual void Awake() {
			switch (StartState) {
				case HidableState.Shown:
					ForceShow();
					break;
				case HidableState.Hided:
					ForceHide();
					break;
			}
		
//			switch (StartAction) {
//			case HidableAction.Show:
//				Show();
//				break;
//			case HidableAction.Hide:
//				Hide();
//				break;
//			}
		}

		public virtual void Start() {
			switch (StartAction) {
				case HidableAction.Show:
					Show();
					break;
				case HidableAction.Hide:
					Hide();
					break;
			}
		}

		public abstract bool Shown();

		public abstract bool Hided();

		public abstract void Show();

		public abstract void Hide();

		public abstract void ForceShow();

		public abstract void ForceHide();

		public virtual void Set(bool show) {
			if (show) Show();
			else Hide();
		}

		public virtual void ForceSet(bool show) {
			if (show) ForceShow();
			else ForceHide();
		}
	}
}

