using System.Collections;
using RitsGameSeminar.Extension;
using UnityEngine;

namespace RitsGameSeminar.Sample {
    public class EnemyShockWaveAttackTask : EnemyTaskBase {
        public override EEnemyTaskType TaskType { get; } = EEnemyTaskType.ShockWaveAttack;
        private readonly ShockWaveController m_shockWaveController;
        private bool m_isFinished = false;
        private float m_effectDuration = 5f;
        
        public EnemyShockWaveAttackTask(GameObject owner) : base(owner) {
            m_shockWaveController = Object.FindObjectOfType<ShockWaveController>();
        }
        
        public override void OnEnter() {
            m_isFinished = false;
            CoroutineManager.Instance.StartDelegatedCoroutine(JumpCoroutine());
        }

        public override bool OnUpdate() {
            return m_isFinished;
        }

        public override void OnExit() { }

        private IEnumerator JumpCoroutine() {
            var originY = owner.transform.position.y;
            var velocity = Vector3.up * 20f;
            while (true) {
                owner.transform.Translate(velocity * Time.deltaTime);
                velocity.y -= 0.1f;
                yield return null;
                if (velocity.y <= 0f) {
                    break;
                }
            }

            while (true) {
                owner.transform.Translate(velocity * Time.deltaTime);
                velocity.y -= 0.5f;
                yield return null;
                if (owner.transform.position.y <= originY) {
                    owner.transform.SetPositionY(originY);
                    break;
                }
            }
            CoroutineManager.Instance.StartDelegatedCoroutine(FireEffectCoroutine());
        }

        private IEnumerator FireEffectCoroutine() {
            var position = owner.transform.position;
            position.y = -1f;
            m_shockWaveController.Fire(position);
            yield return new WaitForSeconds(m_effectDuration);
            m_isFinished = true;
        }
    }
}