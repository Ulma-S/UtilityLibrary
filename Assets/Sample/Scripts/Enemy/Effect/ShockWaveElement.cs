using UnityEngine;

namespace RitsGameSeminar.Sample {
    public class ShockWaveElement : MonoBehaviour {
        [SerializeField] private float m_damageValue = 10f;

        private void OnTriggerEnter(Collider col) {
            if (col.TryGetComponent(out IDamageable damageable)) {
                damageable.ApplyDamage(m_damageValue);
            }
        }
    }
}