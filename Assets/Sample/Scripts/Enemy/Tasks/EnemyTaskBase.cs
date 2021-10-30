using RitsGameSeminar.AI.TaskSystem;
using UnityEngine;

namespace RitsGameSeminar.Sample {
    public abstract class EnemyTaskBase : ITask<EEnemyTaskType> {
        public virtual EEnemyTaskType TaskType { get; } = EEnemyTaskType.None;

        protected readonly GameObject owner = null;
        protected readonly GameObject player = null;
        protected readonly Rigidbody rb;
        protected readonly EnemyStatus enemyStatus;

        protected EnemyTaskBase(GameObject owner) {
            this.owner = owner;
            player = GameObject.FindWithTag("Player");
            rb = owner.GetComponent<Rigidbody>();
            enemyStatus = ServiceLocator.Resolve<IResourceProvider>()
                .LoadResource<EnemyStatus>(EResourceID.EnemyStatus);
        }

        public abstract void OnEnter();

        public abstract bool OnUpdate();

        public abstract void OnExit();
    }
}