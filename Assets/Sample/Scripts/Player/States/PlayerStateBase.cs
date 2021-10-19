using RitsGameSeminar.StateMachine;
using UnityEngine;

namespace RitsGameSeminar.Sample {
    /// <summary>
    /// PlayerのStateの基底クラス.
    /// </summary>
    public abstract class PlayerStateBase : StateBase<EPlayerStateType> {
        protected readonly GameObject owner;
        protected readonly Rigidbody rb;
        protected readonly PlayerCollisionDetector collisionDetector;
        protected readonly PlayerStatus playerStatus;
        protected readonly IInputProvider inputProvider;

        protected PlayerStateBase(StateMachine<EPlayerStateType> stateMachine) : base(stateMachine) {
            owner = stateMachine.owner;
            rb = stateMachine.owner.GetComponent<Rigidbody>();
            collisionDetector = stateMachine.owner.GetComponent<PlayerCollisionDetector>();
            playerStatus = ServiceLocator.Resolve<IResourceProvider>()
                .LoadResource<PlayerStatus>(EResourceID.PlayerStatus);
            inputProvider = ServiceLocator.Resolve<IInputProvider>();
        }
    }
}