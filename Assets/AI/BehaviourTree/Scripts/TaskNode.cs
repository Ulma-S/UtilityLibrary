using System;

namespace RitsGameSeminar.AI.BehaviourTree {
    /// <summary>
    /// 基本となるnode.
    /// </summary>
    public class TaskNode : Node {
        private readonly Func<ENodeStatus> m_actionMethod;
        
        public TaskNode(BehaviourTreeMachine btMachine, Func<ENodeStatus> actionMethod) : base(btMachine){
            m_actionMethod = actionMethod;
        }

        public override void Execute() {
            if (m_actionMethod == null) {
                btMachine.NodeStatusMap[nodeID] = ENodeStatus.Failure;
                return;
            }

            var result = m_actionMethod.Invoke();
            btMachine.NodeStatusMap[nodeID] = result;
        }

        public override void Reset() {
        }
    }
}