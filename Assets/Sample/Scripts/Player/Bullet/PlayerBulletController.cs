using UnityEngine;

namespace RitsGameSeminar.Sample {
    public class PlayerBulletController : MonoBehaviour {
        [SerializeField] private float m_damageValue = 2f;
        
        private void OnTriggerEnter(Collider col) {
            if (col.TryGetComponent(out IDamageable damageable)) {
                damageable.ApplyDamage(m_damageValue);
            }
        }
    }
}