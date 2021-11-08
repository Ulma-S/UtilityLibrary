using UnityEngine;
using UnityEngine.EventSystems;

namespace RitsGameSeminar.Sample {
    public class NodeTrashArea : MonoBehaviour, IDropHandler {
        public void OnDrop(PointerEventData eventData) {
            if (eventData.pointerDrag.TryGetComponent(out DraggableNode node)) {
                node.gameObject.SetActive(false);
            }
        }
    }
}