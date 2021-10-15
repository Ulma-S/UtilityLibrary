using System;

namespace RitsGameSeminar.StateMachine {
    /// <summary>
    /// Stateの基底クラス.
    /// </summary>
    /// <typeparam name="T">stateの種類のenum.</typeparam>
    public abstract class StateBase<T> where T : Enum {
        /// <summary>
        /// Stateの種類.
        /// </summary>
        public abstract T StateType { get; protected set; }

        /// <summary>
        /// 登録先のstateMachine.
        /// </summary>
        protected StateMachine<T> stateMachine { get; }
        
        protected StateBase(StateMachine<T> stateMachine) {
            this.stateMachine = stateMachine;
        }

        /// <summary>
        /// Stateに入る時の処理.
        /// </summary>
        public abstract void OnEnter();

        /// <summary>
        /// Stateの更新処理.
        /// </summary>
        public abstract void OnUpdate();

        /// <summary>
        /// Stateを出るときの処理.
        /// </summary>
        public abstract void OnExit();
    }
}