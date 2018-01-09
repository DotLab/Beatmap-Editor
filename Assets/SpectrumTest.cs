using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Midif.File.Wave;

public class SpectrumTest : MonoBehaviour {
	const string filePath = @"C:\Users\Kailang\Desktop\MMmjs6HzBTKi.wav";

	public Gradient gradient;

	public float bpm;
	public UnityEngine.UI.RawImage uiRawImage;
	public AudioSource source;
	public RectTransform progressLine;
	public BeatmapEditorPanel beatmapEditor;

	public string GameState = "start";

	public float songLength;
	void Start() {
		AudioClip clip;

		float[] data;
		double[] data2;
		double sampleRate;
		int channels;
		using (var stream = System.IO.File.OpenRead(filePath)) {
			var waveFile = new WaveFile(stream);
			clip = AudioClip.Create("test", waveFile.Samples.Length / waveFile.Channels, channels = waveFile.Channels, (int)waveFile.SamplePerSec, false);
			songLength = (float)waveFile.Samples.Length / waveFile.SamplePerSec / waveFile.Channels;
			data = new float[waveFile.Samples.Length];
			data2 = new double[waveFile.Samples.Length / waveFile.Channels];
			sampleRate = waveFile.SamplePerSec;
			for (int i = 0; i < data.Length; i++) data[i] = (float)waveFile.Samples[i];
			for (int i = 0; i < data2.Length; i++) data2[i] = waveFile.Samples[i * waveFile.Channels];
			clip.SetData(data, 0);
		}
	
		const int length = 2048;
		const int skip = 512;
		const int width = length >> 3;
		var com = new Complex[length];
	
		var texture = new Texture2D(width, data.Length / channels / skip, TextureFormat.RGB24, false);
		texture.filterMode = FilterMode.Point;
		Debug.Log(texture.height);
		Debug.Log(texture.width);
	
		for (int i = 0; i < data.Length / channels / skip; i++) {
			if (i * skip * channels + length * channels >= data.Length) break;

			for (int j = 0; j < length; j++) {
				com[j].real = data[i * skip * channels + j * channels];
				com[j].img = 0;
			}

			com = FFT.CalculateFFT(com, false);

			for (int j = 0; j < width; j++) {
				texture.SetPixel(width - j - 1, i, gradient.Evaluate(com[j].fMagnitude * 5));
			}
		}

		texture.Apply();

		uiRawImage.texture = texture;
//		uiRawImage.SetNativeSize();

		beatmapEditor.Init(clip.length, 2, 0);
	
		System.IO.File.WriteAllBytes(filePath + ".png", texture.EncodeToPNG());

		source.clip = clip;
	}

	//	double startDspTime;
	//	void Update() {
	//		double time = AudioSettings.dspTime - startDspTime;
	//		float progresss = (float)time / songLength;
	//		float progresss = source.time / source.clip.length;
	//		float height = uiRawImage.rectTransform.rect.height;
	//		progressLine.anchoredPosition = new Vector2(0, height * progresss);
	//	}
}
