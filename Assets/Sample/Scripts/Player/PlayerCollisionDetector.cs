using UnityEngine;

namespace RitsGameSeminar.Sample {
    /// <summary>
    /// Playerの接触判定を管理するクラス.
    /// </summary>
    public class PlayerCollisionDetector : MonoBehaviour {
        /// <summary>
        /// 地面に接しているか?
        /// </summary>
        public bool IsHitGround { get; private set; } = false;
        
        private void OnCollisionEnter(Collision other) {
            if (other.gameObject.CompareTag("Ground")) {
                IsHitGround = true;
            }
        }

        private void OnCollisionExit(Collision other) {
            if (other.gameObject.CompareTag("Ground")) {
                IsHitGround = false;
            }
        }
    }
}
