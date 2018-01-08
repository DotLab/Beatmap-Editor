using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Uif {
	public class ScrollbarDimer : MonoBehaviour, IBeginDragHandler, IEndDragHandler {
		public float HideDelay = 2;
		
		public ScrollRect uiScrollRect;
		public Hidable scrollbarHidable;
		
		public void OnValidate() {
			uiScrollRect = GetComponent<ScrollRect>();
		}
		
		public void OnBeginDrag(PointerEventData eventData) {
			StopAllCoroutines();
			scrollbarHidable.Show();
		}
		
		public void OnEndDrag(PointerEventData eventData) {
			StartCoroutine(DelayHide());
		}
		
		public IEnumerator DelayHide() {
			yield return new WaitForSeconds(HideDelay);
			
			scrollbarHidable.Hide();
		}
	}
}