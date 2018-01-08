using UnityEngine;

namespace Uif {
	public class InfiniteScrollItem : MonoBehaviour {
		public int Index;
		
		public float Y {
			get { return rectTrans.anchoredPosition.y; }
			set { rectTrans.anchoredPosition = new Vector2(0, value); }
		}
		
		public Vector2 Size {
			get { return rectTrans.sizeDelta; }
			set { rectTrans.sizeDelta = value; }
		}
		
		public float Top { get { return rectTrans.rect.yMax; } }
		public float Bottom { get { return rectTrans.rect.yMin; } }
		
		public RectTransform rectTrans;
		
		void OnValidate() {
			rectTrans = GetComponent<RectTransform>();
		}
	}
}