using RitsGameSeminar.AI.BehaviourTree;

namespace RitsGameSeminar.Sample {
    /// <summary>
    /// Task nodeのアクションを提供するインターフェース.
    /// </summary>
    public interface IActionable {
        public ENodeStatus DoAction();
    }
}