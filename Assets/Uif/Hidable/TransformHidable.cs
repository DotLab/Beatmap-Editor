using UnityEngine;

namespace Uif {
	[AddComponentMenu("Uif/Hidable/Transform Hidable")]
	public class TransformHidable : EasedHidable {
		[System.Serializable]
		public class Translation {
			public Vector3 Position;
			public Vector3 Rotation;
			public Vector3 Scale;

			public Quaternion RotationQ {
				get { return Quaternion.Euler(Rotation); }
			}
		}

		public bool TransPosition;
		public bool TransRotation;
		public bool TransScale;

		[Space]
		public Translation ShowTranslation;
		public Translation HideTranslation;

		[Space]
		public Transform trans;


		public void OnValidate() {
			trans = GetComponent<Transform>();
		}

		[ContextMenu("Record Show Translation")]
		public void RecordShowTranslation() {
			ShowTranslation.Position = trans.localPosition;
			ShowTranslation.Rotation = trans.localRotation.eulerAngles;
			ShowTranslation.Scale = trans.localScale;
		}

		[ContextMenu("Record Hide Translation")]
		public void RecordHideTranslation() {
			HideTranslation.Position = trans.localPosition;
			HideTranslation.Rotation = trans.localRotation.eulerAngles;
			HideTranslation.Scale = trans.localScale;
		}

		public override bool Shown() {
			return 	(TransPosition && trans.localPosition == ShowTranslation.Position)
			|| (TransRotation && trans.localRotation == ShowTranslation.RotationQ)
			|| (TransScale && trans.localScale == ShowTranslation.Scale);
		}

		public override bool Hided() {
			return 	(TransPosition && trans.localPosition == HideTranslation.Position)
			|| (TransRotation && trans.localRotation == HideTranslation.RotationQ)
			|| (TransScale && trans.localScale == HideTranslation.Scale);
		}

		public override void ForceShow() {
			if (TransPosition) trans.localPosition = ShowTranslation.Position;
			if (TransRotation) trans.localRotation = ShowTranslation.RotationQ;
			if (TransScale) trans.localScale = ShowTranslation.Scale;
		}

		public override void ForceHide() {
			if (TransPosition) trans.localPosition = HideTranslation.Position;
			if (TransRotation) trans.localRotation = HideTranslation.RotationQ;
			if (TransScale) trans.localScale = HideTranslation.Scale;
		}

		Vector3 startPosition, endPosition, startScale, endScale;
		Quaternion startRotation, endRotation;

		public override void PrepareShow() {
			startPosition = trans.localPosition;
			startRotation = trans.localRotation;
			startScale = trans.localScale;

			endPosition = ShowTranslation.Position;
			endRotation = ShowTranslation.RotationQ;
			endScale = ShowTranslation.Scale;
		}

		public override void PrepareHide() {
			startPosition = trans.localPosition;
			startRotation = trans.localRotation;
			startScale = trans.localScale;

			endPosition = HideTranslation.Position;
			endRotation = HideTranslation.RotationQ;
			endScale = HideTranslation.Scale;
		}

		public override void ApplyTransition(float step) {
			if (TransPosition) trans.localPosition = Vector3.Lerp(startPosition, endPosition, step);
			if (TransRotation) trans.localRotation = Quaternion.Lerp(startRotation, endRotation, step);
			if (TransScale) trans.localScale = Vector3.Lerp(startScale, endScale, step);
		}
	}
}