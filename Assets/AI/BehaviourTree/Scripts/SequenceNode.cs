namespace RitsGameSeminar.AI.BehaviourTree {
    /// <summary>
    /// SubNodeを順に実行するnode.
    /// </summary>
    public class SequenceNode : Node {
        private readonly Node[] m_nodes;
        private Node m_currentNode;
        private int m_currentNodeIdx;
        
        public SequenceNode(BehaviourTreeMachine btMachine, Node[] nodes) : base(btMachine) {
            m_nodes = nodes;
            m_currentNodeIdx = 0;
            m_currentNode = m_nodes[m_currentNodeIdx];
        }

        public override void Execute() {
            //現在のnodeがSuccessなら
            if (btMachine.NodeStatusMap[m_currentNode.nodeID] == ENodeStatus.Success) {
                //次のnodeが存在していればnodeを移行する.
                if (m_currentNodeIdx + 1 < m_nodes.Length) {
                    m_currentNodeIdx++;
                    m_currentNode = m_nodes[m_currentNodeIdx];
                }
                //現在のnodeが最後ならSequence nodeをSuccessにする.
                else {
                    btMachine.NodeStatusMap[nodeID] = ENodeStatus.Success;
                    return;
                }
            }

            //現在のnodeがFailureならそこで止める.
            if (btMachine.NodeStatusMap[m_currentNode.nodeID] == ENodeStatus.Failure) {
                btMachine.NodeStatusMap[nodeID] = ENodeStatus.Failure;
                return;
            }
            m_currentNode.Execute();
        }

        public override void Reset() {
            foreach (var node in m_nodes) {
                node.Reset();
            }
            m_currentNodeIdx = 0;
            m_currentNode = m_nodes[m_currentNodeIdx];
        }
    }
}
