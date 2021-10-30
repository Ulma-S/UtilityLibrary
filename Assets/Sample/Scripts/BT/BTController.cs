using RitsGameSeminar.AI.BehaviourTree;
using UnityEngine;

namespace RitsGameSeminar.Sample {
    public class BTController : MonoBehaviour {
        private BehaviourTreeMachine m_btMachine;

        private void Start() {
            m_btMachine = new BehaviourTreeMachine();
            var selectorNode = new SelectorNode(m_btMachine, new Node[] {
                new TaskNode(m_btMachine, ActionMethod4),
                new TaskNode(m_btMachine, ActionMethod5),
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
                Debug.Log("No.1 success.");
                return ENodeStatus.Success;
            }

            if (Input.GetKeyDown(KeyCode.Return)) {
                Debug.Log("No.1 failure.");
                return ENodeStatus.Failure;
            }

            Debug.Log("No.1 executing.");
            return ENodeStatus.Running;
        }

        private ENodeStatus ActionMethod2() {
            if (Input.GetKeyDown(KeyCode.Space)) {
                Debug.Log("No.2 success.");
                return ENodeStatus.Success;
            }
            
            if (Input.GetKeyDown(KeyCode.Return)) {
                Debug.Log("No.2 failure.");
                return ENodeStatus.Failure;
            }
            
            Debug.Log("No.2 executing.");
            return ENodeStatus.Running;
        }
        
        private ENodeStatus ActionMethod3() {
            if (Input.GetKeyDown(KeyCode.Space)) {
                Debug.Log("No.3 success.");
                return ENodeStatus.Success;
            }
            
            if (Input.GetKeyDown(KeyCode.Return)) {
                Debug.Log("No.3 failure.");
                return ENodeStatus.Failure;
            }
            
            Debug.Log("No.3 executing.");
            return ENodeStatus.Running;
        }
        
        private ENodeStatus ActionMethod4() {
            if (Input.GetKeyDown(KeyCode.Space)) {
                Debug.Log("No.4 success.");
                return ENodeStatus.Success;
            }
            
            if (Input.GetKeyDown(KeyCode.Return)) {
                Debug.Log("No.4 failure.");
                return ENodeStatus.Failure;
            }
            
            Debug.Log("No.4 executing.");
            return ENodeStatus.Running;
        }
        
        private ENodeStatus ActionMethod5() {
            if (Input.GetKeyDown(KeyCode.Space)) {
                Debug.Log("No.5 success.");
                return ENodeStatus.Success;
            }
            
            if (Input.GetKeyDown(KeyCode.Return)) {
                Debug.Log("No.5 failure.");
                return ENodeStatus.Failure;
            }
            
            Debug.Log("No.5 executing.");
            return ENodeStatus.Running;
        }
    }
}