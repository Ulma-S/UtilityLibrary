using System.Collections.Generic;
using UnityEngine;

namespace RitsGameSeminar.Sample {
    public class EnemyShellPool : MonoBehaviour {
        [SerializeField] private EnemyShellMover m_enemyShell;
        private readonly List<EnemyShellMover> m_shells = new List<EnemyShellMover>();
        private int m_currentIdx = 0;

        private void Start() {
            var count = ServiceLocator.Resolve<IResourceProvider>()
                .LoadResource<EnemyStatus>(EResourceID.EnemyStatus).MaxShellCount;

            //予め弾を生成.
            for (int i = 0; i < count; i++) {
                var obj = Instantiate(m_enemyShell, transform.position, Quaternion.identity);
                obj.transform.parent = transform;
                m_shells.Add(obj);
            }
        }

        /// <summary>
        /// 次に発射できる弾を取得するメソッド.
        /// </summary>
        /// <returns></returns>
        public EnemyShellMover GetNextShell() {
            var result = m_shells[m_currentIdx];
            m_currentIdx++;
            if (m_currentIdx >= m_shells.Count) {
                m_currentIdx = 0;
            }
            return result;
        }
    }
}