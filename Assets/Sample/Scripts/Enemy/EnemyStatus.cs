using UnityEngine;

namespace RitsGameSeminar.Sample {
    [CreateAssetMenu(menuName = "Sample/EnemyStatus")]
    public class EnemyStatus : ScriptableObject {
        public float MoveSpeed;
        public float WaveAttackPower;
        public float ShootAttackPower;
        public float TaskDuration;
        public float HP;
        public float BulletSpeed;
        public float BulletInterval;
        public int MaxBulletCount;
        public float BulletLife;
    }
}