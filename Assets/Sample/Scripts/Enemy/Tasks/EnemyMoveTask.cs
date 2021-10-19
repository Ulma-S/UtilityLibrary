using RitsGameSeminar.Extension;
using UnityEngine;

namespace RitsGameSeminar.Sample {
    public class EnemyMoveTask : EnemyTaskBase {
        public override EEnemyTaskType TaskType { get; } = EEnemyTaskType.Move;
        private int m_moveDir;

        public EnemyMoveTask(GameObject owner) : base(owner) { }

        public override void OnEnter() {
            m_moveDir = GetPlayerDir();
            rb.SetVelocityX(m_moveDir * enemyStatus.MoveSpeed);
        }

        public override bool OnUpdate() {
            return false;
        }

        public override void OnExit() {
            rb.SetVelocityX(0f);
        }

        private int GetPlayerDir() {
            var dir = player.transform.position - owner.transform.position;

            if (dir.x > 0f) {
                return 1;
            }

            if (dir.x < 0f) {
                return -1;
            }

            return 0;
        }
    }
}