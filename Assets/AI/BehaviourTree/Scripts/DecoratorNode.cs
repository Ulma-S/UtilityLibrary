using System;

namespace RitsGameSeminar.AI.BehaviourTree {
    /// <summary>
    /// 実行条件を付与するnode.
    /// </summary>
    public class DecoratorNode : Node {
        private readonly Node m_subNode;
        private readonly Func<bool> m_judgeMethod;
        
        public DecoratorNode(BehaviourTreeMachine btMachine, Node subNode, Func<bool> judgeMethod) : base(btMachine) {
            m_subNode = subNode;
            m_judgeMethod = judgeMethod;
        }

        public override void Execute() {
            if (m_judgeMethod == null) {
                btMachine.NodeStatusMap[nodeID] = ENodeStatus.Failure;
                return;
            }

            var judge = m_judgeMethod.Invoke();

            if (judge) {
                m_subNode.Execute();
                return;
            }

            btMachine.NodeStatusMap[nodeID] = ENodeStatus.Failure;
        }

        public override void Reset() {
        }
    }
}
