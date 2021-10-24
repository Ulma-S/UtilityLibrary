using RitsGameSeminar.AI;
using UnityEngine;

namespace RitsGameSeminar.Sample {
    public class EnemyTaskController : MonoBehaviour {
        private ITaskSystem<EEnemyTaskType> m_taskSystem;

        private void Start() {
            m_taskSystem = new TaskSystem<EEnemyTaskType>();
            
            m_taskSystem.RegisterTask(new ITask<EEnemyTaskType>[] {
                new EnemyIdleTask(gameObject),
                new EnemyMoveTask(gameObject),
                new EnemyShootAttackTask(gameObject),
                new EnemyShockWaveAttackTask(gameObject),
                new EnemyCannonAttackTask(gameObject),
                new EnemyPowerUpTask(gameObject),
            });

            m_taskSystem
                .EnqueueTask(EEnemyTaskType.ShootAttack)
                .EnqueueTask(EEnemyTaskType.ShockWaveAttack)
                .EnqueueTask(EEnemyTaskType.CannonAttack);
        }

        private void Update() {
            //Taskがすべて完了していたらTaskを選択する.
            if (m_taskSystem.IsEndAllTasks) {
                SelectTask();
            }
            m_taskSystem.UpdateTask();
        }

        private void SelectTask() {
            m_taskSystem
                .EnqueueTask(EEnemyTaskType.ShootAttack)
                .EnqueueTask(EEnemyTaskType.ShockWaveAttack)
                .EnqueueTask(EEnemyTaskType.CannonAttack);
        }
    }
}