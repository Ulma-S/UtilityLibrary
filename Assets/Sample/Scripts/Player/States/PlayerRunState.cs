using RitsGameSeminar.Extension;
using RitsGameSeminar.StateMachine;
using UnityEngine;

namespace RitsGameSeminar.Sample {
    /// <summary>
    /// Playerの走る状態を管理するクラス.
    /// </summary>
    public class PlayerRunState : PlayerStateBase {
        public PlayerRunState(StateMachine<EPlayerStateType> stateMachine) : base(stateMachine) { }

        public override EPlayerStateType StateType { get; protected set; } = EPlayerStateType.Run;

        public override void OnEnter() { }

        public override void OnUpdate() {
            var dir = owner.transform.forward * inputProvider.VerticalInput + owner.transform.right * inputProvider.HorizontalInput;
            dir.Normalize();
            rb.SetVelocityX(dir.x * playerStatus.MoveSpeed);
            rb.SetVelocityZ(dir.z * playerStatus.MoveSpeed);

            //移動入力がなければIdleStateに遷移する.
            if (Mathf.Approximately(inputProvider.HorizontalInput, 0f) && Mathf.Approximately(inputProvider.VerticalInput, 0f)) {
                stateMachine.ChangeState(EPlayerStateType.Idle);
            }

            //ジャンプボタンが押されたらJumpStateに遷移する.
            if (inputProvider.IsJumpButtonDown) {
                stateMachine.ChangeState(EPlayerStateType.Jump);
            }
        }

        public override void OnExit() { }
    }
}