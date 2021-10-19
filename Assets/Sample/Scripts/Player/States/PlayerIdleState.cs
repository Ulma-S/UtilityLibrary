using RitsGameSeminar.Extension;
using RitsGameSeminar.StateMachine;
using UnityEngine;

namespace RitsGameSeminar.Sample {
    /// <summary>
    /// Playerの待機状態を管理するクラス.
    /// </summary>
    public class PlayerIdleState : PlayerStateBase {
        public PlayerIdleState(StateMachine<EPlayerStateType> stateMachine) : base(stateMachine) { }

        public override EPlayerStateType StateType { get; protected set; } = EPlayerStateType.Idle;

        public override void OnEnter() {
            rb.SetVelocityX(0f);
            rb.SetVelocityZ(0f);
        }

        public override void OnUpdate() {
            //移動入力があればRunStateに遷移する.
            if (Mathf.Abs(inputProvider.HorizontalInput) > 0f || Mathf.Abs(inputProvider.VerticalInput) > 0f) {
                stateMachine.ChangeState(EPlayerStateType.Run);
            }
            
            //ジャンプボタンが押されたらJumpStateに遷移する.
            if (inputProvider.IsJumpButtonDown) {
                stateMachine.ChangeState(EPlayerStateType.Jump);
            }
        }

        public override void OnExit() { }
    }
}