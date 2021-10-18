using System;
using System.Collections.Generic;
using UnityEngine;

namespace RitsGameSeminar.VFX {
    public class VisualEffectManager : SingletonMonoBehaviour<VisualEffectManager> {
        [Serializable]
        public class VisualEffectData {
            public EVisualEffectID ID;
            public GameObject Source;
        }

        [SerializeField] private int m_maxEffectCount = 10;
        [SerializeField] private List<VisualEffectData> m_visualEffectDatas = new List<VisualEffectData>();

        /// <summary>
        /// 生成済みエフェクトを格納するリスト.
        /// </summary>
        private readonly List<GameObject> m_instantiatedEffects = new List<GameObject>();

        /// <summary>
        /// 生成済みエフェクトのIndxを格納するキュー.
        /// </summary>
        private readonly Queue<int> m_instantiatedEffectIdxQueue = new Queue<int>();
        
        private void Start() {
            //領域確保.
            for (int i = 0; i < m_maxEffectCount; i++) {
                m_instantiatedEffects.Add(null);
            }
        }

        /// <summary>
        /// エフェクトを再生するメソッド.
        /// </summary>
        /// <param name="visualEffectID"></param>
        /// <param name="position"></param>
        public void Play(EVisualEffectID visualEffectID, Vector3 position) {
            var data = m_visualEffectDatas.Find(value => value.ID == visualEffectID);
            if (data == null) {
                Debug.LogError(visualEffectID + "に対応するVFXデータが存在しません.");
                return;
            }

            var effect = Instantiate(data.Source, position, Quaternion.identity);

            //現在のエフェクト数が最大数と同値以上の場合は順次上書きする.
            if (GetCurrentInstantiatedEffectCount() >= m_maxEffectCount) {
                var idx = m_instantiatedEffectIdxQueue.Dequeue();
                
                m_instantiatedEffects[idx] = effect;
            }
            else {
                var idx = GetEmptyIdx();
                m_instantiatedEffectIdxQueue.Enqueue(idx);
                m_instantiatedEffects[idx] = effect;
            }
        }
        
        /// <summary>
        /// 現在シーン上に存在するエフェクト数を返すメソッド.
        /// </summary>
        /// <returns></returns>
        private int GetCurrentInstantiatedEffectCount() {
            var count = 0;
            foreach (var effect in m_instantiatedEffects) {
                if (effect != null) {
                    count++;
                }
            }
            return count;
        }
        

        /// <summary>
        /// 生成済みエフェクトリストから空いているインデックスを検索して返すメソッド.
        /// </summary>
        /// <returns></returns>
        private int GetEmptyIdx() {
            var idx = -1;
            for (int i = 0; i < m_maxEffectCount; i++) {
                if (m_instantiatedEffects[i] == null) {
                    idx = i;
                    break;
                }
            }
            return idx;
        }
    }
}