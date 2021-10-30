namespace RitsGameSeminar.AI.BehaviourTree {
    /// <summary>
    /// SubNodeのいずれか一つを実行するnode.
    /// </summary>
    public class SelectorNode : Node {
        private readonly Node[] m_nodes;
        private Node m_currentSubNode;
        private int m_currentSubNodeIdx = 0;
        
        public SelectorNode(BehaviourTreeSystem btSystem, Node[] nodes) : base(btSystem) {
            m_nodes = nodes;
            m_currentSubNode = m_nodes[m_currentSubNodeIdx];
        }

        public override void Execute() {
            if (btSystem == null) {
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
                if (m_currentSubNodeIdx + 1 < m_nodes.Length) {
                    m_currentSubNodeIdx++;
                    m_currentSubNode = m_nodes[m_currentSubNodeIdx];
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

        public override void Reset() {
            //sub nodeをリセット.
            foreach (var node in m_nodes) {
                node.Reset();
            }
            
            //Selector nodeをリセット.
            m_currentSubNodeIdx = 0;
            m_currentSubNode = m_nodes[m_currentSubNodeIdx];
        }
    }
}