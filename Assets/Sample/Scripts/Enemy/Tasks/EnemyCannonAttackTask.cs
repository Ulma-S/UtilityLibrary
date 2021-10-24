using UnityEngine;

namespace RitsGameSeminar.Sample {
    public class EnemyCannonAttackTask : EnemyTaskBase {
        public override EEnemyTaskType TaskType { get; } = EEnemyTaskType.CannonAttack;
        private readonly EnemyCannonController m_cannonController;
        private bool m_isFinised = false;

        public EnemyCannonAttackTask(GameObject owner) : base(owner) {
            m_cannonController = Object.FindObjectOfType<EnemyCannonController>();
        }

        public override void OnEnter() {
            m_isFinised = false;
            m_cannonController.PrepareBarrel(() => {
                m_isFinised = true;
            });
        }

        public override bool OnUpdate() {
            return m_isFinised;
        }

        public override void OnExit() {
        }
    }
}