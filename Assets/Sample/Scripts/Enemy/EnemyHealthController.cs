using UnityEngine;

namespace RitsGameSeminar.Sample {
    public class EnemyHealthController : MonoBehaviour, IDamageable {
        private float m_maxHP;
        public float CurrentHP { get; private set; }

        private void Start() {
            m_maxHP = ServiceLocator.Resolve<IResourceProvider>()
                .LoadResource<EnemyStatus>(EResourceID.EnemyStatus).HP;
            CurrentHP = m_maxHP;
        }

        public void ApplyDamage(float damageValue) {
            CurrentHP -= damageValue;
            if (CurrentHP < 0f) {
                CurrentHP = 0f;
            }
        }
    }
}