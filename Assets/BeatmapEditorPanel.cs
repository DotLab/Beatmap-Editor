using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BeatmapEditorPanel : MonoBehaviour, IScrollHandler {
	public GameObject BarPrototype, SubPrototype, NotePrototype;
	public float ScaleSensitivity = 0.2f, ScrollSensitivity = 4;

	public List<RectTransform> Bars, Subs, Notes;

	public AudioSource source;

	public RawImage uiRawImage;
	public ScrollRect uiScrollRect;
	public RectTransform rectTrans, contentRectTrans, realContentRectTrans, progressLine;

	public float beatsPerMinute = 120, beatDuration;
	public int subdivide = 2;

	public float songLength, visibleDuration, startDelay;

	public bool isPlaying, isLocked;

	public void OnValidate() {
		rectTrans = GetComponent<RectTransform>();
		uiScrollRect = GetComponent<ScrollRect>();
		contentRectTrans = uiScrollRect.content;
	}

	[ContextMenu("Test")]
	public void Test() {
		Init(songLength, visibleDuration, startDelay);
		SetBpm(beatsPerMinute);
		SetSubdivide(subdivide);
	}

	public void SetStartDelay(float startDelay) {
		this.startDelay = startDelay;

		contentRectTrans.sizeDelta = new Vector2(0, rectTrans.rect.height / visibleDuration * (songLength + startDelay));
		realContentRectTrans.offsetMin = new Vector2(0, rectTrans.rect.height / visibleDuration * startDelay);
	}

	public void Init(float songLength, float visibleDuration, float startDelay = 0) {
		this.songLength = songLength;
		this.startDelay = startDelay;
		SetVisibleDuration(visibleDuration);
	}

	public void SetVisibleDuration(float visibleDuration) {
		this.visibleDuration = visibleDuration;

		contentRectTrans.sizeDelta = new Vector2(0, rectTrans.rect.height / visibleDuration * (songLength + startDelay));
		realContentRectTrans.offsetMin = new Vector2(0, rectTrans.rect.height / visibleDuration * startDelay);
		SetBpm(beatsPerMinute);
	}

	public void SetBpm(float bpm) {
		beatsPerMinute = bpm;
		beatDuration = 1f / (bpm / 60f);

		int barCount = Mathf.CeilToInt(startDelay + songLength / beatDuration);
	
		while (Bars.Count < barCount) Bars.Add(Instantiate(BarPrototype, contentRectTrans).GetComponent<RectTransform>());
		while (Bars.Count > barCount) {
			Destroy(Bars[0].gameObject);
			Bars.RemoveAt(0);
		}

		for (int i = 0; i < barCount; i++) {
			Bars[i].anchoredPosition = new Vector2(0, rectTrans.rect.height / visibleDuration * (i * beatDuration));
		}

		SetSubdivide(subdivide);
	}

	public void SetSubdivide(int subdivide) {
		this.subdivide = subdivide;

		int subCount = Bars.Count * (subdivide - 1);

		while (Subs.Count < subCount) Subs.Add(Instantiate(SubPrototype, contentRectTrans).GetComponent<RectTransform>());
		while (Subs.Count > subCount) {
			Destroy(Subs[0].gameObject);
			Subs.RemoveAt(0);
		}

		for (int i = 0; i < Bars.Count; i++) {
			for (int j = 0; j < subdivide - 1; j++) {
				Subs[i * (subdivide - 1) + j].anchoredPosition = new Vector2(0, rectTrans.rect.height / visibleDuration * (i * beatDuration + (j + 1) * (beatDuration / subdivide)));
			}
		}
	}

	float startDspTime, time;
	public void Update() {

		if (Input.GetKeyDown(KeyCode.Space)) {
			isPlaying = !isPlaying;
			if (isPlaying) {
				source.Play();
				SetSourceTime(ScrollPosition2Time(uiScrollRect.verticalNormalizedPosition));
			} else {
				source.Stop();
			}
		}

		if (Input.GetKeyDown(KeyCode.LeftArrow)) {
			SetStartDelay(startDelay - 0.01f);
		} else if (Input.GetKeyDown(KeyCode.RightArrow)) {
			SetStartDelay(startDelay + 0.01f);
		}

		if (Input.GetKeyDown(KeyCode.G)) {
			isLocked = !isLocked;
		}

		if (!isPlaying) {
			if (Input.GetKeyDown(KeyCode.DownArrow)) {
				uiScrollRect.verticalNormalizedPosition = Time2ScrollPosition(GetSubdivideSnapedTime(ScrollPosition2Time(uiScrollRect.verticalNormalizedPosition) - beatDuration / subdivide));
			} else if (Input.GetKeyDown(KeyCode.UpArrow)) {
				uiScrollRect.verticalNormalizedPosition = Time2ScrollPosition(GetSubdivideSnapedTime(ScrollPosition2Time(uiScrollRect.verticalNormalizedPosition) + beatDuration / subdivide));
			}
		}

		if (!isPlaying) return;

		if (!source.isPlaying) {
			isPlaying = false;
			return;
		}
			
		time = source.time; 

		if (Input.GetKeyDown(KeyCode.DownArrow)) {
			SetSourceTime(GetBeatSnapedTime(source.time - beatDuration));
		} else if (Input.GetKeyDown(KeyCode.UpArrow)) {
			SetSourceTime(source.time + beatDuration);
		}

		if (isLocked) {
			uiScrollRect.verticalNormalizedPosition = Time2ScrollPosition(time);
		}

		float progresss = time / source.clip.length;
		progressLine.anchoredPosition = new Vector2(0, uiRawImage.rectTransform.rect.height * progresss);
	}

	public float Time2ScrollPosition(float time) {
		return time / (songLength + startDelay) + (startDelay / (songLength + startDelay));
	}

	public float ScrollPosition2Time(float scrollPosition) {
		return (scrollPosition - (startDelay / (songLength + startDelay))) * (songLength + startDelay);
	}

	public void SetSourceTime(float time) {
		source.time = Mathf.Clamp(time, 0, source.clip.length);
	}

	public float GetBeatSnapedTime(float time) {
		return Mathf.RoundToInt((time + startDelay) / beatDuration) * beatDuration - startDelay;
	}

	public float GetSubdivideSnapedTime(float time) {
		var duration = beatDuration / subdivide;
		return Mathf.RoundToInt((time + startDelay) / duration) * duration - startDelay;
	}



#region IScrollHandler implementation

	public void OnScroll(PointerEventData eventData) {
//		Debug.Log(eventData.scrollDelta);

		if (Input.GetKey(KeyCode.LeftControl)) {
			float verticalPosition = uiScrollRect.verticalNormalizedPosition;
			SetVisibleDuration(Mathf.Clamp(visibleDuration + eventData.scrollDelta.y * ScaleSensitivity, 1, 10));
			uiScrollRect.verticalNormalizedPosition = verticalPosition;
		} else {
			isLocked = false;
			if (eventData.scrollDelta.y < 0) {
				uiScrollRect.verticalNormalizedPosition = Time2ScrollPosition(GetBeatSnapedTime(ScrollPosition2Time(uiScrollRect.verticalNormalizedPosition) - beatDuration));
			} else if (eventData.scrollDelta.y > 0) {
				uiScrollRect.verticalNormalizedPosition = Time2ScrollPosition(GetBeatSnapedTime(ScrollPosition2Time(uiScrollRect.verticalNormalizedPosition) + beatDuration));
			}
//			uiScrollRect.verticalNormalizedPosition += eventData.scrollDelta.y / rectTrans.rect.height * ScrollSensitivity;
		}
	}

#endregion
}
