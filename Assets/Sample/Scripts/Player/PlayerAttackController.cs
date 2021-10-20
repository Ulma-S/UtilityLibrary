using System.Collections;
using UnityEngine;

namespace RitsGameSeminar.Sample {
    public class PlayerAttackController : MonoBehaviour {
        private BulletPool m_bulletPool;
        private float m_shootInterval;
        private GameObject m_enemy;
        private IInputProvider m_inputProvider;

        private void Start() {
            m_bulletPool = FindObjectOfType<BulletPool>();
            m_shootInterval = ServiceLocator.Resolve<IResourceProvider>()
                .LoadResource<PlayerStatus>(EResourceID.PlayerStatus).ShootInterval;
            
            m_enemy = GameObject.FindWithTag("Enemy");
            m_inputProvider = ServiceLocator.Resolve<IInputProvider>();
        }

        private void Update() {
            if (m_inputProvider.IsShootDown) {
                Shoot();
            }
        }

        private void Shoot() {
            StartCoroutine(ShootCoroutine());
        }

        private IEnumerator ShootCoroutine() {
            while (m_inputProvider.IsShoot) {
                var dir = m_enemy.transform.position - transform.position;
                m_bulletPool.GetNextBullet().Activate(transform.position, dir.normalized);
                yield return new WaitForSeconds(m_shootInterval);
            }
        }
    }
}