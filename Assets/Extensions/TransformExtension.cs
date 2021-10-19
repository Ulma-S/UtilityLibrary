using UnityEngine;

namespace RitsGameSeminar.Extension {
    /// <summary>
    /// Transformの拡張クラス.
    /// </summary>
    public static class TransformExtension {
        /// <summary>
        /// positionのx成分を設定するメソッド.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="value"></param>
        public static void SetPositionX(this Transform self, float value) {
            var position = self.position;
            position.x = value;
            self.position = position;
        }
        
        /// <summary>
        /// positionのy成分を設定するメソッド.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="value"></param>
        public static void SetPositionY(this Transform self, float value) {
            var position = self.position;
            position.y = value;
            self.position = position;
        }
        
        /// <summary>
        /// positionのz成分を設定するメソッド.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="value"></param>
        public static void SetPositionZ(this Transform self, float value) {
            var position = self.position;
            position.z = value;
            self.position = position;
        }
    }
}