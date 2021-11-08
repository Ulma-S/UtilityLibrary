using UnityEngine;
using UnityEngine.EventSystems;

namespace RitsGameSeminar.Sample {
    public class DraggableNode : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
        [SerializeField] private EDraggableNodeType m_nodeType = EDraggableNodeType.None;
        public EDraggableNodeType NodeType => m_nodeType;
        
        public Vector3 OriginPosition { get; private set; }
        public bool HasUsed { get; set; } = false;

        private void Start() {
            OriginPosition = transform.position;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }

        public virtual void OnBeginDrag(PointerEventData eventData) {
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        public virtual void OnDrag(PointerEventData eventData) {
            transform.position = eventData.position;
        }

        public virtual void OnEndDrag(PointerEventData eventData) {
            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
    }
}