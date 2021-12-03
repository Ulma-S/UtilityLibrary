using System.Collections;
using RitsGameSeminar.AI.BehaviourTree;
using RitsGameSeminar.Extension;
using UnityEngine;

namespace RitsGameSeminar.Sample {
    public class ShockWaveAttackTask : MonoBehaviour, IActionable {
        private GameObject m_enemy;
        private ShockWaveController m_shockWaveController;
        private bool m_isStarted = false;
        private bool m_isFinished = false;
        private float m_effectDuration = 2f;

        private void Start() {
            m_enemy = GameObject.FindWithTag("Enemy");
            m_shockWaveController = FindObjectOfType<ShockWaveController>();
        }

        public ENodeStatus DoAction() {
            if (!m_isStarted) {
                CoroutineManager.Instance.StartDelegatedCoroutine(JumpCoroutine());
                m_isStarted = true;
                m_isFinished = false;
            }
            
            if (m_isFinished) {
                m_isStarted = false;
                return ENodeStatus.Success;
            }
            return ENodeStatus.Ready;
        }
        
        private IEnumerator JumpCoroutine() {
            var originY = m_enemy.transform.position.y;
            var velocity = Vector3.up * 30f;
            while (true) {
                m_enemy.transform.Translate(velocity * Time.deltaTime);
                velocity.y -= 0.2f;
                yield return null;
                if (velocity.y <= 0f) {
                    break;
                }
            }

            while (true) {
                m_enemy.transform.Translate(velocity * Time.deltaTime);
                velocity.y -= 0.5f;
                yield return null;
                if (m_enemy.transform.position.y <= originY) {
                    m_enemy.transform.SetPositionY(originY);
                    break;
                }
            }

            CoroutineManager.Instance.StartDelegatedCoroutine(FireEffectCoroutine());
        }

        private IEnumerator FireEffectCoroutine() {
            var position = m_enemy.transform.position;
            position.y = -1f;
            m_shockWaveController.Fire(position);
            yield return new WaitForSeconds(m_effectDuration);
            m_isFinished = true;
        }
    }
}