using RitsGameSeminar.AI.BehaviourTree;

namespace RitsGameSeminar.Sample {
    public interface IActionable {
        public ENodeStatus DoAction();
    }
}