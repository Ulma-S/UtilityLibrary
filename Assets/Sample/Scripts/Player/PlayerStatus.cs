using UnityEngine;

namespace RitsGameSeminar.Sample {
    [CreateAssetMenu(menuName = "Sample/PlayerStatus")]
    public class PlayerStatus : ScriptableObject {
        public float MoveSpeed;
        public float JumpForce;
    }
}