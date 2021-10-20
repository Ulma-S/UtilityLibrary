using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RitsGameSeminar.Sample {
    public class ShockWaveController : MonoBehaviour {
        private class EffectData {
            public GameObject Source;
            public Vector3 MoveDir;
        }
        
        [SerializeField] private GameObject m_effectElement;
        [SerializeField] private int m_effectCount = 40;
        [SerializeField] private float m_effectSpeed = 15f;
        [SerializeField] private float m_effectDuration = 5f;
        private readonly List<EffectData> m_effectElements = new List<EffectData>();
        
        private void Start() {
            for (int i = 0; i < m_effectCount; i++) {
                var obj = Instantiate(m_effectElement, Vector3.zero, Quaternion.identity);
                obj.transform.parent = transform;
                obj.SetActive(false);
                var angle = (360f / m_effectCount) * i * Mathf.Deg2Rad;
                m_effectElements.Add(new EffectData {
                    Source = obj, 
                    MoveDir = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)),
                });
            }
        }

        /// <summary>
        /// 攻撃を発火するメソッド.
        /// </summary>
        /// <param name="startPosition"></param>
        public void Fire(Vector3 startPosition) {
            StartCoroutine(MoveCoroutine(startPosition));
        }

        private IEnumerator MoveCoroutine(Vector3 startPosition) {
            foreach (var element in m_effectElements) {
                element.Source.transform.position = startPosition;
                element.Source.SetActive(true);
            }

            var startTime = Time.time;
            while (true) {
                foreach (var element in m_effectElements) {
                    element.Source.transform.Translate(element.MoveDir * m_effectSpeed * Time.deltaTime);
                }
                yield return null;

                if (Time.time - startTime >= m_effectDuration) {
                    break;
                }
            }

            foreach (var element in m_effectElements) {
                element.Source.SetActive(false);
            }
        }
    }
}