namespace Uif {
	public class EasedCompositeHidable : EasedHidable {
		public EasedHidable MainHidable {
			get { return Hidables == null || Hidables.Length == 0 ? null : Hidables[0]; }
		}

		public bool Enforce = true;
		public bool Recursive = true;

		public EasedHidable[] Hidables;

		void OnValidate() {
			var hidables = Recursive ? GetComponentsInChildren<EasedHidable>() : GetComponents<EasedHidable>();

			Hidables = new EasedHidable[hidables.Length - 1];
			var i = 0;
			foreach (var hidable in hidables) if (hidable != this) Hidables[i++] = hidable;

			if (Enforce) {
				foreach (var hidable in Hidables) {
					hidable.TransitionEasingType = TransitionEasingType;
					hidable.TransitionDuration = TransitionDuration;
					hidable.StartState = StartState;
					hidable.StartAction = StartAction;
				}
			}
		}
			
		public override bool Shown() {
			return MainHidable == null || MainHidable.Shown();
		}

		public override bool Hided() {
			return MainHidable == null || MainHidable.Hided();
		}
			
		public override void ForceShow() {
			foreach (var hidable in Hidables) hidable.ForceShow();
		}

		public override void ForceHide() {
			foreach (var hidable in Hidables) hidable.ForceHide();
		}

		public override void PrepareShow() {
			foreach (var hidable in Hidables) hidable.PrepareShow();
		}

		public override void PrepareHide() {
			foreach (var hidable in Hidables) hidable.PrepareHide();
		}

		public override void ApplyTransition(float step) {
			foreach (var hidable in Hidables) hidable.ApplyTransition(step);
		}
	}
}

