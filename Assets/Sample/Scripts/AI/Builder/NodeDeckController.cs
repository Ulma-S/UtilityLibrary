using UnityEngine;

namespace RitsGameSeminar.Sample {
    public class NodeDeckController : MonoBehaviour {
        [SerializeField] private GameObject m_nodesArea;
        
        private void OnTriggerExit2D(Collider2D col) { 
            if (col.TryGetComponent(out DraggableNode node)) {
                if (node.HasUsed) {
                    return;
                }
                var obj = Instantiate(node.gameObject, transform.position, Quaternion.identity);
                obj.transform.parent = m_nodesArea.transform;
                obj.transform.position = node.OriginPosition;
                node.HasUsed = true;
            }
        }
    }
}