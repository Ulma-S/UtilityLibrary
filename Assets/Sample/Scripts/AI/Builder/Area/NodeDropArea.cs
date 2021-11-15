using System.Collections.Generic;
using RitsGameSeminar.AI.BehaviourTree;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RitsGameSeminar.Sample {
    public class NodeDropArea : MonoBehaviour, IDropHandler {
        [SerializeField] private NodeDropArea m_parentArea;
        public DraggableNode DraggableNode { get; private set; } = null;

        private AIBehaviourBuilder m_behaviourBuilder;
        public Node Node { get; private set; }
        
        private void Start() {
            m_behaviourBuilder = FindObjectOfType<AIBehaviourBuilder>();
        }

        public void OnDrop(PointerEventData eventData) {
            if (eventData.pointerDrag.TryGetComponent(out DraggableNode node)) {
                node.transform.position = transform.position;

                DraggableNode = node;

                if (m_parentArea != null) {
                    if (m_parentArea.DraggableNode != null) {
                        if (m_parentArea.DraggableNode.NodeType == EDraggableNodeType.Selector ||
                            m_parentArea.DraggableNode.NodeType == EDraggableNodeType.Sequence) {

                            switch (node.NodeType) {
                                case EDraggableNodeType.Sequence:
                                    Node = new SequenceNode(m_behaviourBuilder.BtSystem, new List<Node>());
                                    break;
                                
                                case EDraggableNodeType.Selector:
                                    Node = new SelectorNode(m_behaviourBuilder.BtSystem, new List<Node>());
                                    break;
                                
                                case EDraggableNodeType.Task:
                                    if (node.TryGetComponent(out IActionable actionable)) {
                                        Node = new TaskNode(m_behaviourBuilder.BtSystem, actionable.DoAction);
                                    }
                                    break;
                            }
                       
                            switch (m_parentArea.DraggableNode.NodeType) {
                                case EDraggableNodeType.Sequence:
                                {
                                    var parentNode = (SequenceNode) m_parentArea.Node;
                                    parentNode.AddSubNode(Node);
                                    parentNode.Reset();
                                }
                                    break;
                                
                                case EDraggableNodeType.Selector:
                                {
                                    var parentNode = (SelectorNode) m_parentArea.Node;
                                    parentNode.AddSubNode(Node);
                                    parentNode.Reset();
                                }
                                    break;
                            }
                        }
                        else {
                            node.gameObject.SetActive(false);
                            DraggableNode = null;
                            Node = null;
                        }
                    }
                    else {
                        node.gameObject.SetActive(false);
                        DraggableNode = null;
                        Node = null;
                    }
                }
                else {
                    if (node.NodeType == EDraggableNodeType.Sequence) {
                        Node = new SequenceNode(m_behaviourBuilder.BtSystem, new List<Node>());
                        m_behaviourBuilder.BtSystem.SetRoot(Node);
                    }
                    else if (node.NodeType == EDraggableNodeType.Selector) {
                        Node = new SelectorNode(m_behaviourBuilder.BtSystem, new List<Node>());
                        m_behaviourBuilder.BtSystem.SetRoot(Node);
                    }
                    else {
                        if (node.TryGetComponent(out IActionable actionable)) {
                            Node = new TaskNode(m_behaviourBuilder.BtSystem, actionable.DoAction);
                            m_behaviourBuilder.BtSystem.SetRoot(Node);
                        }
                    }
                }
            }
        }
    }
}
