using UnityEngine;
using UnityEngine.UI;

namespace Uif {
	public class InfiniteScroll <T> : MonoBehaviour where T : InfiniteScrollItem {
		public float ScrollTop, ScrollBottom = -200;

		public GameObject ItemPrototype;
		public int ItemCount = 24;
		public float ItemWidth = 200, ItemHeight = 60, ItemSpacing = 30;

		public ScrollRect uiScroll;
		public RectTransform rectTrans, contentRectTrans;

		public T[] items;
		public int visibleItemCount;
		public float itemSkip;

		public bool notInfinite;

		public void OnValidate() {
			rectTrans = GetComponent<RectTransform>();
			uiScroll = GetComponent<ScrollRect>();
			contentRectTrans = uiScroll.content;

			itemSkip = ItemHeight + ItemSpacing;
		}

		public void RebuildItems() {
			visibleItemCount = Mathf.CeilToInt((ScrollTop - ScrollBottom + ItemSpacing) / itemSkip) + 1;
			var newItems = new T[visibleItemCount];
			System.Array.Copy(items, newItems, items.Length);

			for (int i = items.Length; i < visibleItemCount; i++) {
				var item = Instantiate(ItemPrototype, contentRectTrans).GetComponent<T>();
				item.GetComponent<Button>().onClick.AddListener(() => OnItemClick(item));
				item.Size = new Vector2(ItemWidth, ItemHeight);

				newItems[i] = item;
			}

			items = newItems;
		}

		public virtual void RebuildContent() {
			contentRectTrans.anchoredPosition = new Vector2(contentRectTrans.anchoredPosition.x, 0);
			contentRectTrans.sizeDelta = new Vector2(contentRectTrans.sizeDelta.x, ItemCount * itemSkip - ItemSpacing);

			if (ItemCount <= visibleItemCount) {  // Infinite scroll is not needed;
				for (int i = 0; i < ItemCount; i++) {
					if (notInfinite) items[i].gameObject.SetActive(true);
					InitItem(items[i], i);
				}

				for (int i = ItemCount; i < visibleItemCount; i++) {
					items[i].gameObject.SetActive(false);
				}

				notInfinite = true;
			} else {
				for (int i = 0; i < visibleItemCount; i++) {
					if (notInfinite) items[i].gameObject.SetActive(true);
					InitItem(items[i], i);
				}

				notInfinite = false;
			}
		}

		public float contentTop;
		public int firstVisibleIndex, lastVisibleIndex;
		public int minIndex, maxIndex;

		public void UpdateScroll() {
			contentTop = contentRectTrans.anchoredPosition.y + contentRectTrans.rect.yMax;

			if (notInfinite) return;

			firstVisibleIndex = Mathf.FloorToInt((contentTop - ScrollTop + ItemSpacing) / itemSkip);
			lastVisibleIndex = firstVisibleIndex + visibleItemCount - 1;

			if (firstVisibleIndex < 0 || lastVisibleIndex >= ItemCount) return;

			minIndex = items[0].Index;
			maxIndex = items[0].Index;
			foreach (var item in items) {
				if (item.Index < minIndex) minIndex = item.Index;
				else if (item.Index > maxIndex) maxIndex = item.Index;
			}

			if (minIndex == firstVisibleIndex) return;

			if (Mathf.Abs(firstVisibleIndex - minIndex) < 4) {
				foreach (var item in items) {
					if (item.Y + item.Bottom + contentTop > ScrollTop && (-itemSkip * (maxIndex + 1)) + item.Top + contentTop >= ScrollBottom) {
						maxIndex += 1;
						//					Debug.Log("Move item " + item.Index + " to bottom " + maxIndex);
						InitItem(item, maxIndex);
					} else if (item.Y + item.Top + contentTop < ScrollBottom && (-itemSkip * (minIndex - 1)) + item.Bottom + contentTop <= ScrollTop) {
						minIndex -= 1;
						//					Debug.Log("Move item " + item.Index + " to top " + minIndex);
						InitItem(item, minIndex);
					}
				}
			} else {
				Debug.Log("Flush");
				for (int i = 0; i < visibleItemCount; i++) {
					InitItem(items[i], firstVisibleIndex + i);
				}
			}
		}

		public virtual void InitItem(T item, int index) {
			item.Index = index;
			item.Y = -itemSkip * index;
		}

		public virtual void OnItemClick(T item) {
			//		Debug.Log(item.Index);
		}
	}
}
