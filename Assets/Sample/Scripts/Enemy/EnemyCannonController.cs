using System;
using System.Collections;
using UnityEngine;

namespace RitsGameSeminar.Sample {
    public class EnemyCannonController : MonoBehaviour {
        [SerializeField] private GameObject m_enemy;
        [SerializeField] private GameObject m_barrel;
        [SerializeField] private Vector3 m_cannonPosition = new Vector3(0f, 0f, 0.4f);
        [SerializeField] private Vector3 m_barrelPosition = new Vector3(0f, 0.9f, 0f);
        [SerializeField] private float m_prepareDuration = 1f;
        private GameObject m_player;
        private EnemyShellPool m_enemyShellPool;
        private bool m_hasPrepared = false;

        private void Start() {
            m_player = GameObject.FindWithTag("Player");
            m_enemyShellPool = FindObjectOfType<EnemyShellPool>();
        }

        public void PrepareBarrel(Action onFinishedCallback) {
            if (!m_hasPrepared) {
                StartCoroutine(PrepareBarrelCoroutine(onFinishedCallback));
                m_hasPrepared = true;
            }
            else { 
                ShootShell(onFinishedCallback);
            }
        }

        private void ShootShell(Action onFinishedCallback) {
            StartCoroutine(ShootShellCoroutine(onFinishedCallback));
        }

        private IEnumerator PrepareBarrelCoroutine(Action onFinishedCallback) {
            transform.localPosition = Vector3.zero;
            m_barrel.transform.localPosition = Vector3.zero;
            var cannonDis = m_cannonPosition - transform.localPosition;
            var elapsedTime = 0f;
            while (true) {
                transform.localPosition += cannonDis * Time.deltaTime / m_prepareDuration;
                yield return null;
                elapsedTime += Time.deltaTime;
                if (elapsedTime >= m_prepareDuration) {
                    transform.localPosition = m_cannonPosition;
                    break;
                }
            }

            var barrelDis = m_barrelPosition - m_barrel.transform.localPosition;
            elapsedTime = 0f;
            while (true) {
                m_barrel.transform.localPosition += barrelDis * Time.deltaTime / m_prepareDuration;
                yield return null;
                elapsedTime += Time.deltaTime;
                if (elapsedTime >= m_prepareDuration) {
                    m_barrel.transform.localPosition = m_barrelPosition;
                    break;
                }
            }
            ShootShell(onFinishedCallback);
        }

        private IEnumerator ShootShellCoroutine(Action onFinishedCallback) {
            var elapsedTime = 0f;
            while (true) {
                var dir = (m_player.transform.position - m_enemy.transform.position).normalized;
                var rot = Quaternion.LookRotation(dir);
                var eulerAngles = rot.eulerAngles;
                eulerAngles.x = 0f;
                eulerAngles.z = 0f;
                rot = Quaternion.Euler(eulerAngles);
                m_enemy.transform.rotation = Quaternion.Slerp(m_enemy.transform.rotation, rot, elapsedTime / 2f);
                
                yield return null;
                elapsedTime += Time.deltaTime;

                if (elapsedTime >= 1f) {
                    m_enemyShellPool.GetNextShell().Activate(m_barrel.transform.position, dir);
                    break;
                }
            }
            onFinishedCallback?.Invoke();
        }
    }
}