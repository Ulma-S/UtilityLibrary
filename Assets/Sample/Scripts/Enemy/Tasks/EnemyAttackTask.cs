using UnityEngine;

namespace RitsGameSeminar.Sample {
    public class EnemyAttackTask : EnemyTaskBase {
        public override EEnemyTaskType TaskType { get; } = EEnemyTaskType.Attack;
        
        public EnemyAttackTask(GameObject owner) : base(owner) {
        }
        
        public override void OnEnter() {
        }

        public override bool OnUpdate() {
            return false;
        }

        public override void OnExit() {
        }
    }
}