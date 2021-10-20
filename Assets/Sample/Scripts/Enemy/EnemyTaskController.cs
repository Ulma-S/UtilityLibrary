using RitsGameSeminar.AI;
using UnityEngine;

namespace RitsGameSeminar.Sample {
    public class EnemyTaskController : MonoBehaviour {
        private ITaskSystem<EEnemyTaskType> m_taskSystem;

        private void Start() {
            m_taskSystem = new TaskSystem<EEnemyTaskType>();
            
            m_taskSystem.RegisterTask(new EnemyIdleState(gameObject));
            m_taskSystem.RegisterTask(new EnemyMoveTask(gameObject));
            m_taskSystem.RegisterTask(new EnemyShootAttackState(gameObject));
            m_taskSystem.RegisterTask(new EnemyShockWaveAttackTask(gameObject));
            m_taskSystem.RegisterTask(new EnemyPowerUpTask(gameObject));

            m_taskSystem.EnqueueTask(EEnemyTaskType.ShootAttack);
            m_taskSystem.EnqueueTask(EEnemyTaskType.ShockWaveAttack);
        }

        private void Update() {
            //Taskがすべて完了していたらTaskを選択する.
            if (m_taskSystem.IsEndAllTasks) {
                SelectTask();
            }
            m_taskSystem.UpdateTask();
        }

        private void SelectTask() {
            m_taskSystem.EnqueueTask(EEnemyTaskType.ShootAttack);
            m_taskSystem.EnqueueTask(EEnemyTaskType.ShockWaveAttack);
        }
    }
}