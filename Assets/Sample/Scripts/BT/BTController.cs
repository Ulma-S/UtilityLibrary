using RitsGameSeminar.AI.BehaviourTree;
using UnityEngine;

namespace RitsGameSeminar.Sample {
    public class BTController : MonoBehaviour {
        private BehaviourTreeMachine m_btMachine;

        private void Start() {
            m_btMachine = new BehaviourTreeMachine();
            var selectorNode = new SelectorNode(m_btMachine, new Node[] {
                new TaskNode(m_btMachine, ActionMethod4),
                new DecoratorNode(m_btMachine, new TaskNode(m_btMachine, ActionMethod5), () => {
                    var rand = Random.Range(0, 10);
                    if (rand % 2 == 0) {
                        return true;
                    }
                    return false;
                }),
            });

            var rootNode = new SequenceNode(m_btMachine, new Node[] {
                new TaskNode(m_btMachine, ActionMethod),
                new TaskNode(m_btMachine, ActionMethod2),
                selectorNode,
                new TaskNode(m_btMachine, ActionMethod3),
            });
            
            //rootを設定.
            m_btMachine.SetRoot(rootNode);
        }

        private void Update() {
            m_btMachine.Execute();
        }

        private ENodeStatus ActionMethod() {
            if (Input.GetKeyDown(KeyCode.Space)) {
                return ENodeStatus.Success;
            }

            if (Input.GetKeyDown(KeyCode.Return)) {
                return ENodeStatus.Failure;
            }

            Debug.Log("No.1 executing.");
            return ENodeStatus.Running;
        }

        private ENodeStatus ActionMethod2() {
            if (Input.GetKeyDown(KeyCode.Space)) {
                return ENodeStatus.Success;
            }
            
            if (Input.GetKeyDown(KeyCode.Return)) {
                return ENodeStatus.Failure;
            }
            
            Debug.Log("No.2 executing.");
            return ENodeStatus.Running;
        }
        
        private ENodeStatus ActionMethod3() {
            if (Input.GetKeyDown(KeyCode.Space)) {
                return ENodeStatus.Success;
            }
            
            if (Input.GetKeyDown(KeyCode.Return)) {
                return ENodeStatus.Failure;
            }
            
            Debug.Log("No.3 executing.");
            return ENodeStatus.Running;
        }
        
        private ENodeStatus ActionMethod4() {
            if (Input.GetKeyDown(KeyCode.Space)) {
                return ENodeStatus.Success;
            }
            
            if (Input.GetKeyDown(KeyCode.Return)) {
                return ENodeStatus.Failure;
            }
            
            Debug.Log("No.4 executing.");
            return ENodeStatus.Running;
        }
        
        private ENodeStatus ActionMethod5() {
            if (Input.GetKeyDown(KeyCode.Space)) {
                return ENodeStatus.Success;
            }
            
            if (Input.GetKeyDown(KeyCode.Return)) {
                return ENodeStatus.Failure;
            }
            
            Debug.Log("No.5 executing.");
            return ENodeStatus.Running;
        }
    }
}