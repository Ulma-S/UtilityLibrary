using System.Collections.Generic;
using UnityEngine;

namespace RitsGameSeminar.Sample {
    public class EnemyBulletPool : MonoBehaviour {
        [SerializeField] private EnemyBulletMover m_enemyBullet;
        private readonly List<EnemyBulletMover> m_bullets = new List<EnemyBulletMover>();
        private int m_currentIdx = 0;

        private void Start() {
            var count = ServiceLocator.Resolve<IResourceProvider>()
                .LoadResource<EnemyStatus>(EResourceID.EnemyStatus).MaxBulletCount;

            //予め弾を生成.
            for (int i = 0; i < count; i++) {
                var obj = Instantiate(m_enemyBullet, transform.position, Quaternion.identity);
                obj.transform.parent = transform;
                m_bullets.Add(obj);
            }
        }

        /// <summary>
        /// 次に発射できる弾を取得するメソッド.
        /// </summary>
        /// <returns></returns>
        public EnemyBulletMover GetNextBullet() {
            var result = m_bullets[m_currentIdx];
            m_currentIdx++;
            if (m_currentIdx >= m_bullets.Count) {
                m_currentIdx = 0;
            }
            return result;
        }
    }
}