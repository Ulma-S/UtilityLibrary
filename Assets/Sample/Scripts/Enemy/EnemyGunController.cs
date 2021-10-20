using System.Collections;
using UnityEngine;

namespace RitsGameSeminar.Sample {
    public class EnemyGunController : MonoBehaviour {
        private GameObject m_player;
        private EnemyBulletPool m_bulletPool;
        private int m_maxBulletCount;
        private float m_shootInterval;
        
        private void Start() {
            m_player = GameObject.FindWithTag("Player");
            m_bulletPool = FindObjectOfType<EnemyBulletPool>();
            m_maxBulletCount = ServiceLocator.Resolve<IResourceProvider>()
                .LoadResource<EnemyStatus>(EResourceID.EnemyStatus).MaxBulletCount;
       
            m_shootInterval = ServiceLocator.Resolve<IResourceProvider>()
                .LoadResource<EnemyStatus>(EResourceID.EnemyStatus).BulletInterval;
        }

        public void ShootToPlayer() {
            StartCoroutine(ShootCoroutine());
        }

        private IEnumerator ShootCoroutine() {
            var shootCount = 0;
            while (shootCount < m_maxBulletCount) {
                var startTime = Time.time;
                while (true) {
                    var aim = (m_player.transform.position - transform.position).normalized;
                    var rot = Quaternion.LookRotation(aim);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rot, 0.04f);

                    yield return null;

                    if (Time.time - startTime >= m_shootInterval) {
                        m_bulletPool.GetNextBullet().Activate(transform.position, aim);
                        break;
                    }
                }
                shootCount++;
            }
        }
    }
}
