using UnityEngine;

namespace Uif {
	[AddComponentMenu("Uif/Hidable/AudioSource Hidable")]
	[RequireComponent(typeof(AudioSource))]
	public class AudioSourceHidable : EasedHidable {
		[Range(0, 1)]
		public float ShowVolume = 1;
		[Range(0, 1)]
		public float HideVolume;

		[Space]
		public AudioSource source;


		public void OnValidate() {
			source = GetComponent<AudioSource>();
		}

		public override bool Shown() {
			return source.volume == ShowVolume;
		}

		public override bool Hided() {
			return source.volume == HideVolume;
		}

		public override void ForceShow() {
			source.volume = ShowVolume;
		}

		public override void ForceHide() {
			source.volume = HideVolume;
		}

		float srcVolume, dstVolume;

		public override void PrepareShow() {
			srcVolume = source.volume;
			dstVolume = ShowVolume;
		}

		public override void PrepareHide() {
			srcVolume = source.volume;
			dstVolume = HideVolume;
		}

		public override void ApplyTransition(float step) {
			source.volume = srcVolume + (dstVolume - srcVolume) * step;
		}
	}
}