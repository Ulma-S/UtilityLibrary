using UnityEngine;

namespace RitsGameSeminar.Sample {
    [CreateAssetMenu(menuName = "Sample/EnemyStatus")]
    public class EnemyStatus : ScriptableObject {
        public float MoveSpeed;
        public float AttackPower;
        public float TaskDuration;
    }
}