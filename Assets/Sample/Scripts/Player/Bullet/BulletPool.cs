using System.Collections.Generic;
using UnityEngine;

namespace RitsGameSeminar.Sample {
    public class BulletPool : MonoBehaviour {
        [SerializeField] private BulletMover m_bullet;
        private readonly List<BulletMover> m_bullets = new List<BulletMover>();
        private int m_currentIdx = 0;

        private void Start() {
            var count = ServiceLocator.Resolve<IResourceProvider>()
                .LoadResource<PlayerStatus>(EResourceID.PlayerStatus).MaxBulletCount;

            for (int i = 0; i < count; i++) {
                var obj = Instantiate(m_bullet, transform.position, Quaternion.identity);
                obj.transform.parent = transform;
                m_bullets.Add(obj);
            }
        }

        public BulletMover GetNextBullet() {
            var result = m_bullets[m_currentIdx];
            m_currentIdx++;
            if (m_currentIdx >= m_bullets.Count) {
                m_currentIdx = 0;
            }
            return result;
        }
    }
}