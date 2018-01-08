using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Midif.File.Wave;

public class MainScheduler : MonoBehaviour {
	const string infile = @"C:\Users\Kailang\Desktop\uHFrZrOJpz5d.wav";
	const string outfile = @"C:\Users\Kailang\Downloads\96140__zgump__zg-hat-01.wav";

	public Gradient gradient;

	public UnityEngine.UI.RawImage uiRawImage, uiRawImage1, uiRawImage2;

	void Start() {
		AudioClip clip;
//	
		float[] data;
		int channels;
		using (var stream = System.IO.File.OpenRead(infile)) {
			var waveFile = new WaveFile(stream);
			clip = AudioClip.Create("test", waveFile.Samples.Length, channels = waveFile.Channels, (int)waveFile.SamplePerSec, false);
			data = new float[waveFile.Samples.Length];
			for (int i = 0; i < data.Length; i++) data[i] = (float)waveFile.Samples[i];
			clip.SetData(data, 0);
		}
	
		const int length = 1 << 10;
		const int skip = 1 << 9;
		const int width = 1 << 7;
		var com = new B83.MathHelpers.Complex[length];
	
		var texture = new Texture2D(width, data.Length / channels / skip, TextureFormat.RGB24, false);
		texture.filterMode = FilterMode.Point;
		Debug.Log(texture.height);
		Debug.Log(texture.width);
	
		for (int i = 0; i < data.Length / channels / skip; i++) {

//		for (int i = 0; i < 100; i++) {
			if (i * skip * channels + length * channels >= data.Length) break;

			for (int j = 0; j < length; j++) {
				com[j].real = data[i * skip * channels + j * channels];
				com[j].img = 0;
			}
	
			com = B83.MathHelpers.FFT.CalculateFFT(com, false);
	
			for (int j = 0; j < width; j++) {
				texture.SetPixel(width - j - 1, i, gradient.Evaluate(com[j].fMagnitude * 4));
			}
		}
	
		texture.Apply();
		uiRawImage.texture = texture;
	
		System.IO.File.WriteAllBytes(infile + ".png", texture.EncodeToPNG());

		AudioSource.PlayClipAtPoint(clip, Vector2.zero);
//	
//		StartCoroutine(StartHandler());

//		texture1 = new Texture2D(1024, 1);
//		texture1.filterMode = FilterMode.Point;
//		uiRawImage1.texture = texture1;
//	
//		texture2 = new Texture2D(1024, 1);
//		texture2.filterMode = FilterMode.Point;
//		uiRawImage2.texture = texture2;
	}
	//	Texture2D texture1, texture2;
	//	float[] spec = new float[2048];
	//	B83.MathHelpers.Complex[] com = new B83.MathHelpers.Complex[2048];
	//	public float scale = 10;
	//	void Update() {
	//		AudioListener.GetSpectrumData(spec, 0, FFTWindow.Rectangular);
	//		for (int i = 0; i < 1024; i++) {
	//			texture1.SetPixel(i, 0, gradient.Evaluate(spec[i] * scale));
	//		}
	//		texture1.Apply();
	//
	//		AudioListener.GetOutputData(spec, 0);
	//		for (int i = 0; i < 2048; i++) {
	//			com[i] = new B83.MathHelpers.Complex(spec[i], 0);
	//		}
	//		B83.MathHelpers.FFT.CalculateFFT(com, false);
	//		for (int i = 0; i < 1024; i++) {
	//			texture2.SetPixel(i, 0, gradient.Evaluate(scale * com[i].fMagnitude));
	//		}
	//		texture2.Apply();
	//	}
}
