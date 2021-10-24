using RitsGameSeminar.StateMachine;
using UnityEngine;

namespace RitsGameSeminar.Sample {
    /// <summary>
    /// PlayerのStateを管理するクラス.
    /// </summary>
    public class PlayerStateController : MonoBehaviour {
        private StateMachine<EPlayerStateType> m_stateMachine;

        private void Start() {
            m_stateMachine = new StateMachine<EPlayerStateType>(gameObject);
            
            //stateの登録.
            m_stateMachine.RegisterState(new StateBase<EPlayerStateType>[] {
                new PlayerIdleState(m_stateMachine),
                new PlayerRunState(m_stateMachine),
                new PlayerJumpState(m_stateMachine),
            });
            
            //最初のstateの設定.
            m_stateMachine.ChangeState(EPlayerStateType.Idle);
        }

        private void Update() {
            m_stateMachine.Update();
        }
    }
}