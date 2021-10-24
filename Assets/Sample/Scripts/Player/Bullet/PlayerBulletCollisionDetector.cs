using System;
using UnityEngine;

namespace RitsGameSeminar.Sample {
    public class PlayerBulletCollisionDetector : MonoBehaviour {
        [SerializeField] private float m_damageValue = 2f;
        public event Action OnHitHandler;

        private void OnTriggerEnter(Collider col) {
            if (col.TryGetComponent(out IDamageable damageable)) {
                damageable.ApplyDamage(m_damageValue);
                OnHitHandler?.Invoke();
            }
        }
    }
}