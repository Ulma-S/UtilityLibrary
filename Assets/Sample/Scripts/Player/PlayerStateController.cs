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
            m_stateMachine.RegisterState(new PlayerIdleState(m_stateMachine));
            m_stateMachine.RegisterState(new PlayerRunState(m_stateMachine));
            m_stateMachine.RegisterState(new PlayerJumpState(m_stateMachine));

            //最初のstateの設定.
            m_stateMachine.ChangeState(EPlayerStateType.Idle);
        }

        private void Update() {
            m_stateMachine.Update();
            Debug.Log(m_stateMachine.CurrentStateType);
        }
    }
}