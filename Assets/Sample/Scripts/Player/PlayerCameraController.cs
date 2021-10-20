using UnityEngine;

namespace RitsGameSeminar.Sample {
    public class PlayerCameraController : MonoBehaviour {
        private GameObject m_enemy;

        private void Start() {
            m_enemy = GameObject.FindWithTag("Enemy");
        }

        private void Update() {
            var aim = m_enemy.transform.position - transform.position;
            var rot = Quaternion.LookRotation(aim);
            transform.rotation = rot;
        }
    }
}