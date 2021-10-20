using System.Collections;
using UnityEngine;

namespace RitsGameSeminar.Sample {
    public class EnemyShootAttackState : EnemyTaskBase {
        public override EEnemyTaskType TaskType { get; } = EEnemyTaskType.ShootAttack;
        private readonly EnemyGunController m_gunController;
        private bool m_isFinished = false;

        public EnemyShootAttackState(GameObject owner) : base(owner) {
            m_gunController = Object.FindObjectOfType<EnemyGunController>();
        }

        public override void OnEnter() {
            m_isFinished = false;
            CoroutineManager.Instance.StartDelegatedCoroutine(ShootCoroutine());
        }

        public override bool OnUpdate() {
            return m_isFinished;
        }

        public override void OnExit() {
        }

        private IEnumerator ShootCoroutine() {
            m_gunController.ShootToPlayer();
            yield return new WaitForSeconds(enemyStatus.BulletLife + enemyStatus.BulletInterval * enemyStatus.MaxBulletCount);
            m_isFinished = true;
        }
    }
}