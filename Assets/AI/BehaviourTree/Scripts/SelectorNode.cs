using System.Collections.Generic;

namespace RitsGameSeminar.AI.BehaviourTree {
    /// <summary>
    /// SubNodeのいずれか一つを実行するnode.
    /// </summary>
    public class SelectorNode : Node {
        private readonly List<Node> m_subNodes;
        private Node m_currentSubNode;
        private int m_currentSubNodeIdx = 0;
        
        public SelectorNode(BehaviourTreeSystem btSystem, List<Node> subNodes) : base(btSystem) {
            m_subNodes = subNodes;
            m_currentSubNodeIdx = 0;
            if (subNodes.Count == 0) {
                m_currentSubNode = null;
            }
            else {
                m_currentSubNode = m_subNodes[m_currentSubNodeIdx];
            }
        }

        public override void Execute() {
            if (btSystem == null) {
                return;
            }

            if (m_currentSubNode == null) {
                return;
            }
            
            //Success nodeがあればそこで止める.
            if (btSystem.NodeStatusMap[m_currentSubNode.nodeID] == ENodeStatus.Success) {
                btSystem.NodeStatusMap[nodeID] = ENodeStatus.Success;
                return;
            }
            
            //現在のnodeがFailureなら
            if (btSystem.NodeStatusMap[m_currentSubNode.nodeID] == ENodeStatus.Failure) {
                //次のnodeが存在すればnodeを移行する.
                if (m_currentSubNodeIdx + 1 < m_subNodes.Count) {
                    m_currentSubNodeIdx++;
                    m_currentSubNode = m_subNodes[m_currentSubNodeIdx];
                }
                //現在のnodeが最後ならSelector nodeをFailureにする.
                else {
                    btSystem.NodeStatusMap[nodeID] = ENodeStatus.Failure;
                    return;
                }
            }
            m_currentSubNode.Execute();
            btSystem.NodeStatusMap[nodeID] = ENodeStatus.Running;
        }

        public void AddSubNode(Node node) {
            m_subNodes.Add(node);
        }

        public void RemoveSubNode(Node node) {
            m_subNodes.Remove(node);
        }

        public override void Reset() {
            //sub nodeをリセット.
            foreach (var node in m_subNodes) {
                node?.Reset();
            }
            
            //Selector nodeをリセット.
            m_currentSubNodeIdx = 0;
            if (m_subNodes.Count == 0) {
                m_currentSubNode = null;
            }
            else {
                m_currentSubNode = m_subNodes[0];
            }
        }
    }
}