using RitsGameSeminar.StateMachine;
using UnityEngine;

namespace RitsGameSeminar.Sample {
    /// <summary>
    /// Playerのジャンプ状態を管理するクラス.
    /// </summary>
    public class PlayerJumpState : PlayerStateBase {
        public override EPlayerStateType StateType { get; protected set; } = EPlayerStateType.Jump;
        
        public PlayerJumpState(StateMachine<EPlayerStateType> stateMachine) : base(stateMachine) { }

        private bool m_prevIsHitGround = false;

        public override void OnEnter() {
            rb.AddForce(Vector3.up * playerStatus.JumpForce);
            m_prevIsHitGround = collisionDetector.IsHitGround;
        }

        public override void OnUpdate() {
            //着地したらIdleStateに遷移する.
            if (collisionDetector.IsHitGround && !m_prevIsHitGround) {
                stateMachine.ChangeState(EPlayerStateType.Idle);
            }

            m_prevIsHitGround = collisionDetector.IsHitGround;
        }

        public override void OnExit() { }
    }
}