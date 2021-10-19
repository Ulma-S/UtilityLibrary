using RitsGameSeminar.AI;
using UnityEngine;

namespace RitsGameSeminar.Sample {
    public class EnemyTaskController : MonoBehaviour {
        private ITaskSystem<EEnemyTaskType> m_taskSystem;

        private void Start() {
            m_taskSystem = new TaskSystem<EEnemyTaskType>();
            
            m_taskSystem.RegisterTask(new EnemyIdleState(gameObject));
            m_taskSystem.RegisterTask(new EnemyMoveTask(gameObject));
            m_taskSystem.RegisterTask(new EnemyAttackTask(gameObject));
            m_taskSystem.RegisterTask(new EnemyPowerUpTask(gameObject));
            
            m_taskSystem.EnqueueTask(EEnemyTaskType.Idle);
        }
    }
}