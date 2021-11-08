using UnityEngine;
using UnityEngine.UI;

namespace RitsGameSeminar.Sample {
    public class ResetButton : MonoBehaviour {
        private void Start() {
            var dropAreas = FindObjectsOfType<NodeDropArea>();
            
            GetComponent<Button>().onClick.AddListener(() => {
                FindObjectOfType<AIBehaviourBuilder>().BtSystem.SetRoot(null);
                foreach (var dropArea in dropAreas) {
                    if (dropArea.DraggableNode == null) {
                        continue;
                    }
                    dropArea.DraggableNode.gameObject.SetActive(false);
                }
            });
        }
    }
}
