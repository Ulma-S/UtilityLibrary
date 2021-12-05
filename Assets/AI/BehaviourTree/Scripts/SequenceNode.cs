using System.Collections.Generic;

namespace RitsGameSeminar.AI.BehaviourTree {
    /// <summary>
    /// SubNodeを順に実行するnode.
    /// </summary>
    public class SequenceNode : Node {
        private readonly List<Node> m_subNodes;
        
        /// <summary>
        /// 現在実行されているsub node.
        /// </summary>
        private Node m_currentSubNode;
        
        /// <summary>
        /// 現在実行されているsub nodeのindex.
        /// </summary>
        private int m_currentSubNodeIdx;
        
        public SequenceNode(BehaviourTreeSystem btSystem, List<Node> subNodes) : base(btSystem) {
            m_subNodes = subNodes;
            m_currentSubNodeIdx = 0;
            if (m_subNodes.Count == 0) {
                m_currentSubNode = null;
            }
            else {
                m_currentSubNode = m_subNodes[0];
            }
        }

        public override void Execute() {
            if (btSystem == null) {
                return;
            }

            if (m_currentSubNode == null) {
                return;
            }
            
            //現在のnodeがSuccessなら
            if (btSystem.NodeStatusMap[m_currentSubNode.nodeID] == ENodeStatus.Success) {
                //次のnodeが存在していればnodeを移行する.
                if (m_currentSubNodeIdx + 1 < m_subNodes.Count) {
                    m_currentSubNodeIdx++;
                    m_currentSubNode = m_subNodes[m_currentSubNodeIdx];
                }
                //現在のnodeが最後ならSequence nodeをSuccessにする.
                else {
                    btSystem.NodeStatusMap[nodeID] = ENodeStatus.Success;
                    return;
                }
            }

            //現在のnodeがFailureならそこで止める.
            if (btSystem.NodeStatusMap[m_currentSubNode.nodeID] == ENodeStatus.Failure) {
                btSystem.NodeStatusMap[nodeID] = ENodeStatus.Failure;
                return;
            }
            
            //現在のsub nodeを実行する.
            m_currentSubNode.Execute();
            
            btSystem.NodeStatusMap[nodeID] = ENodeStatus.Running;
        }

        public void AddSubNode(Node node) {
            m_subNodes.Add(node);
        }

        public void RemoveSubNode(Node node) {
            m_subNodes.Remove(node);
        }

        public void RemoveAllNodes() {
            m_subNodes.Clear();
        }
        
        public override void Reset() {
            //sub nodeをリセット.
            foreach (var node in m_subNodes) {
                node?.Reset();
            }
            
            //Sequence nodeをリセット.
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
