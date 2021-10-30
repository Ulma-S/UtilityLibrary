using System;

namespace RitsGameSeminar.AI.BehaviourTree {
    /// <summary>
    /// 実行条件を付与するnode.
    /// </summary>
    public class DecoratorNode : Node {
        private readonly Node m_subNode;
        private readonly Func<bool> m_judgeMethod;
        private bool m_judge = false;
        private bool m_hasJudged = false;
        
        public DecoratorNode(BehaviourTreeMachine btMachine, Node subNode, Func<bool> judgeMethod) : base(btMachine) {
            m_subNode = subNode;
            m_judgeMethod = judgeMethod;
        }

        public override void Execute() {
            if (m_judgeMethod == null) {
                btMachine.NodeStatusMap[nodeID] = ENodeStatus.Failure;
                return;
            }

            if (!m_hasJudged) {
                m_judge = m_judgeMethod.Invoke();
                m_hasJudged = true;
            }

            if (m_judge) {
                m_subNode.Execute();
                btMachine.NodeStatusMap[nodeID] = btMachine.NodeStatusMap[m_subNode.nodeID];
                return;
            }

            btMachine.NodeStatusMap[nodeID] = ENodeStatus.Failure;
        }

        public override void Reset() {
            m_judge = false;
            m_hasJudged = false;
        }
    }
}
