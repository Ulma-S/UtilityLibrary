namespace RitsGameSeminar.AI.BehaviourTree {
    /// <summary>
    /// Nodeの基底クラス.
    /// </summary>
    public abstract class Node {
        protected readonly BehaviourTreeMachine btMachine;
        public int nodeID { get; }
        
        protected Node(BehaviourTreeMachine btMachine) {
            this.btMachine = btMachine;
            nodeID = btMachine.GetNextNodeID();
            btMachine.NodeStatusMap[nodeID] = ENodeStatus.Ready;
        }
        
        /// <summary>
        /// nodeを実行するメソッド.
        /// </summary>
        public abstract void Execute();

        /// <summary>
        /// nodeをリセットするメソッド.
        /// </summary>
        public abstract void Reset();
    }
}