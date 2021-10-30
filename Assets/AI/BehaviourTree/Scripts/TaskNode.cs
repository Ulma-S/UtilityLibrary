using System;

namespace RitsGameSeminar.AI.BehaviourTree {
    /// <summary>
    /// 基本となるnode.
    /// </summary>
    public class TaskNode : Node {
        private readonly Func<ENodeStatus> m_taskMethod;
        
        public TaskNode(BehaviourTreeSystem btSystem, Func<ENodeStatus> taskMethod) : base(btSystem){
            m_taskMethod = taskMethod;
        }

        public override void Execute() {
            if (btSystem == null) {
                return;
            }
            
            //methodがnullなら何もしない.
            if (m_taskMethod == null) {
                btSystem.NodeStatusMap[nodeID] = ENodeStatus.Failure;
                return;
            }

            //methodの実行.
            var result = m_taskMethod.Invoke();
            
            //nodeの状態を反映.
            btSystem.NodeStatusMap[nodeID] = result;
        }

        public override void Reset() { }
    }
}