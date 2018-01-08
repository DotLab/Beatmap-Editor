using UnityEngine.UI;

namespace Uif {
	public class EmptyGraphic : Graphic {
		protected override void OnPopulateMesh(VertexHelper vh) {
			vh.Clear();
		}
	}
}
