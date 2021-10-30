namespace RitsGameSeminar.AI.BehaviourTree {
    /// <summary>
    /// SubNodeを順に実行するnode.
    /// </summary>
    public class SequenceNode : Node {
        private readonly Node[] m_nodes;
        private Node m_currentSubNode;
        private int m_currentSubNodeIdx;
        
        public SequenceNode(BehaviourTreeSystem btSystem, Node[] nodes) : base(btSystem) {
            m_nodes = nodes;
            m_currentSubNodeIdx = 0;
            m_currentSubNode = m_nodes[m_currentSubNodeIdx];
        }

        public override void Execute() {
            if (btSystem == null) {
                return;
            }
            
            //現在のnodeがSuccessなら
            if (btSystem.NodeStatusMap[m_currentSubNode.nodeID] == ENodeStatus.Success) {
                //次のnodeが存在していればnodeを移行する.
                if (m_currentSubNodeIdx + 1 < m_nodes.Length) {
                    m_currentSubNodeIdx++;
                    m_currentSubNode = m_nodes[m_currentSubNodeIdx];
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

        public override void Reset() {
            //sub nodeをリセット.
            foreach (var node in m_nodes) {
                node.Reset();
            }
            
            //Sequence nodeをリセット.
            m_currentSubNodeIdx = 0;
            m_currentSubNode = m_nodes[m_currentSubNodeIdx];
        }
    }
}
