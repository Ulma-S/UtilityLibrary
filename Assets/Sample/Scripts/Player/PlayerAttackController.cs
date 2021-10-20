using System.Collections;
using UnityEngine;

namespace RitsGameSeminar.Sample {
    public class PlayerAttackController : MonoBehaviour {
        private PlayerBulletPool m_playerBulletPool;
        private float m_shootInterval;
        private GameObject m_enemy;
        private IInputProvider m_inputProvider;

        private void Start() {
            m_playerBulletPool = FindObjectOfType<PlayerBulletPool>();
            m_shootInterval = ServiceLocator.Resolve<IResourceProvider>()
                .LoadResource<PlayerStatus>(EResourceID.PlayerStatus).ShootInterval;
            
            m_enemy = GameObject.FindWithTag("Enemy");
            m_inputProvider = ServiceLocator.Resolve<IInputProvider>();
        }

        private void Update() {
            if (m_inputProvider.IsShootButtonDown) {
                Shoot();
            }
        }

        private void Shoot() {
            StartCoroutine(ShootCoroutine());
        }

        private IEnumerator ShootCoroutine() {
            while (m_inputProvider.IsShootButton) {
                var dir = m_enemy.transform.position - transform.position;
                m_playerBulletPool.GetNextBullet().Activate(transform.position, dir.normalized);
                yield return new WaitForSeconds(m_shootInterval);
            }
        }
    }
}