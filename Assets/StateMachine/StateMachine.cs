using System;
using System.Collections.Generic;
using UnityEngine;

namespace RitsGameSeminar.StateMachine {
    /// <summary>
    /// StateMachineクラス.
    /// </summary>
    /// <typeparam name="T">stateの種類のenum.</typeparam>
    public class StateMachine<T> where T : Enum {
        private readonly Dictionary<T, StateBase<T>> m_statesMap = new Dictionary<T, StateBase<T>>();
        
        /// <summary>
        /// StateMachineを動かしているGameObject.
        /// </summary>
        public readonly GameObject owner;

        /// <summary>
        /// 現在のState
        /// </summary>
        private StateBase<T> currentState;

        /// <summary>
        /// 現在のstateの種類.
        /// </summary>
        public T CurrentStateType {
            get {
                if (currentState != null) {
                    return currentState.StateType;
                }
                return default;
            }
        }

        public StateMachine(GameObject owner) {
            this.owner = owner;
        }

        /// <summary>
        /// stateMachineの更新をするメソッド.
        /// </summary>
        public void Update() {
            currentState?.OnUpdate();
        }

        /// <summary>
        /// stateを登録するメソッド.
        /// </summary>
        /// <param name="state"></param>
        public void RegisterState(StateBase<T> state) {
            if (state == null) {
                return;
            }
            //キーが存在したら何もしない.
            if (m_statesMap.ContainsKey(state.StateType)) {
                Debug.LogError(state.StateType + "stateは既に登録されています.");
            }
            //stateの登録.
            m_statesMap[state.StateType] = state;
        }
        
        /// <summary>
        /// stateの登録を解除するメソッド.
        /// </summary>
        /// <param name="stateType"></param>
        public void RemoveState(T stateType) {
            if (!m_statesMap.Remove(stateType)) {
                Debug.LogError(stateType + "stateが登録されていません.");
            }
        }
        
        /// <summary>
        /// stateを変更するメソッド.
        /// </summary>
        public void ChangeState(T stateType) {
            //キーが存在しなければ何もしない.
            if (!m_statesMap.ContainsKey(stateType)) {
                Debug.LogError(stateType + "stateが存在しません.");
                return;
            }
            //現在のstateの終了処理.
            currentState?.OnExit();

            //次のstateの起動処理.
            currentState = m_statesMap[stateType];
            currentState?.OnEnter();
        }
    }
}