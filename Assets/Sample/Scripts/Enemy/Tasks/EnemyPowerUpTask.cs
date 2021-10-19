using UnityEngine;

namespace RitsGameSeminar.Sample {
    public class EnemyPowerUpTask : EnemyTaskBase {
        public EnemyPowerUpTask(GameObject owner) : base(owner) { }

        public override EEnemyTaskType TaskType { get; } = EEnemyTaskType.PowerUp;

        public override void OnEnter() {
        }

        public override bool OnUpdate() {
            return false;
        }

        public override void OnExit() {
        }
    }
}