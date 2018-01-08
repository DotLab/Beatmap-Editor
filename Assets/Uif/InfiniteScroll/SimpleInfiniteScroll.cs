namespace Uif {
	public class SimpleInfiniteScroll : InfiniteScroll<InfiniteScrollItem> {
		public bool Dirty;
		
		public void Start() {
			uiScroll.onValueChanged.AddListener(value => Dirty = true);
			
			RebuildItems();
			RebuildContent();
		}
		
		public void Update() {
			if (Dirty) {
				Dirty = false;
				UpdateScroll();
			}
		}
		
		public override void InitItem(InfiniteScrollItem item, int index) {
			base.InitItem(item, index);
			
			item.GetComponentInChildren<UnityEngine.UI.Text>().text = index.ToString();
		}
	}
}