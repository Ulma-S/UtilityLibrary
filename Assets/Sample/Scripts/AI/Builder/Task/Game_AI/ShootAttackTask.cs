using System.Collections;
using RitsGameSeminar.AI.BehaviourTree;
using UnityEngine;

namespace RitsGameSeminar.Sample {
    public class ShootAttackTask : MonoBehaviour, IActionable {
        private EnemyGunController m_gunController;
        private EnemyStatus m_enemyStatus;
        private bool m_isStarted = false;
        private bool m_isFinished = false;

        private void Start() {
            m_gunController = FindObjectOfType<EnemyGunController>();
            m_enemyStatus = ServiceLocator.Resolve<IResourceProvider>().LoadResource<EnemyStatus>(EResourceID.EnemyStatus);
        }

        public ENodeStatus DoAction() {
            if (!m_isStarted) {
                CoroutineManager.Instance.StartDelegatedCoroutine(ShootCoroutine());
                m_isStarted = true;
                m_isFinished = false;
            }

            if (m_isFinished) {
                m_isStarted = false;
                return ENodeStatus.Success;
            }
            return ENodeStatus.Ready;
        }
        
        private IEnumerator ShootCoroutine() {
            m_gunController.ShootToPlayer();
            yield return new WaitForSeconds(m_enemyStatus.BulletLife + m_enemyStatus.BulletInterval * m_enemyStatus.MaxBulletCount);
            m_isFinished = true;
        }
    }
}