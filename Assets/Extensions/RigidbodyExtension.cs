using UnityEngine;

namespace RitsGameSeminar.Extension {
    /// <summary>
    /// Rigidbodyの拡張クラス.
    /// </summary>
    public static class RigidbodyExtension {
        /// <summary>
        /// velocityのx成分を設定するメソッド.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="value"></param>
        public static void SetVelocityX(this Rigidbody self, float value) {
            var velocity = self.velocity;
            velocity.x = value;
            self.velocity = velocity;
        }

        /// <summary>
        /// velocityのy成分を設定するメソッド.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="value"></param>
        public static void SetVelocityY(this Rigidbody self, float value) {
            var velocity = self.velocity;
            velocity.y = value;
            self.velocity = velocity;
        }
        
        /// <summary>
        /// velocityのz成分を設定するメソッド.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="value"></param>
        public static void SetVelocityZ(this Rigidbody self, float value) {
            var velocity = self.velocity;
            velocity.z = value;
            self.velocity = velocity;
        }
    }
}