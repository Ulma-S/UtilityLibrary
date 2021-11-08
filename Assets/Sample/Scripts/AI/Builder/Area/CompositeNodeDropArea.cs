using UnityEngine;
using UnityEngine.EventSystems;

namespace RitsGameSeminar.Sample {
    public class CompositeNodeDropArea : MonoBehaviour, IDropHandler {
        public void OnDrop(PointerEventData eventData) {
            if (eventData.pointerDrag.TryGetComponent(out DraggableCompositeNode compositeNode)) {
                Debug.Log(compositeNode.name);
                compositeNode.transform.position = transform.position;
            }
        }
    }
}