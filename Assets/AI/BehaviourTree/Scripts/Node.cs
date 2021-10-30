namespace RitsGameSeminar.AI.BehaviourTree {
    /// <summary>
    /// Nodeの基底クラス.
    /// </summary>
    public abstract class Node {
        protected readonly BehaviourTreeSystem btSystem;
        public int nodeID { get; }
        
        protected Node(BehaviourTreeSystem btSystem) {
            this.btSystem = btSystem;
            nodeID = btSystem.GetNextNodeID();
            btSystem.NodeStatusMap[nodeID] = ENodeStatus.Ready;
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