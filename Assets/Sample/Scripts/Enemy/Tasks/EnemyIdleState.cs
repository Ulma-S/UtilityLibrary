using RitsGameSeminar.Extension;
using UnityEngine;

namespace RitsGameSeminar.Sample {
    public class EnemyIdleState : EnemyTaskBase {
        public EnemyIdleState(GameObject owner) : base(owner) { }

        public override EEnemyTaskType TaskType { get; } = EEnemyTaskType.Idle;

        public override void OnEnter() {
            rb.SetVelocityX(0f);
        }

        public override bool OnUpdate() {
            return false;
        }

        public override void OnExit() {
        }
    }
}