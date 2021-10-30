namespace RitsGameSeminar.AI.BehaviourTree {
    /// <summary>
    /// SubNodeのいずれか一つを実行するnode.
    /// </summary>
    public class SelectorNode : Node {
        private readonly Node[] m_nodes;
        private Node m_currentNode;
        private int m_currentNodeIdx = 0;
        
        public SelectorNode(BehaviourTreeMachine btMachine, Node[] nodes) : base(btMachine) {
            m_nodes = nodes;
            m_currentNode = m_nodes[m_currentNodeIdx];
        }

        public override void Execute() {
            //Success nodeがあればそこで止める.
            if (btMachine.NodeStatusMap[m_currentNode.nodeID] == ENodeStatus.Success) {
                btMachine.NodeStatusMap[nodeID] = ENodeStatus.Success;
                return;
            }
            
            //現在のnodeがFailureなら
            if (btMachine.NodeStatusMap[m_currentNode.nodeID] == ENodeStatus.Failure) {
                //次のnodeが存在すればnodeを移行する.
                if (m_currentNodeIdx + 1 < m_nodes.Length) {
                    m_currentNodeIdx++;
                    m_currentNode = m_nodes[m_currentNodeIdx];
                }
                //現在のnodeが最後ならSelector nodeをFailureにする.
                else {
                    btMachine.NodeStatusMap[nodeID] = ENodeStatus.Failure;
                    return;
                }
            }
            m_currentNode.Execute();
        }

        public override void Reset() {
            foreach (var node in m_nodes) {
                node.Reset();
            }
            //Selector nodeをリセット.
            m_currentNodeIdx = 0;
            m_currentNode = m_nodes[m_currentNodeIdx];
        }
    }
}