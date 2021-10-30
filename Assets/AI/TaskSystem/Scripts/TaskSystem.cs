using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

namespace RitsGameSeminar.AI.TaskSystem {
    /// <summary>
    /// 事前に登録されたTaskをキューに入れて、順に実行するクラス.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TaskSystem<T> : ITaskSystem<T> where T : Enum{
        /// <summary>
        /// 登録されたTaskの連想配列.
        /// </summary>
        private readonly Dictionary<T, ITask<T>> m_registeredTaskMap = new Dictionary<T, ITask<T>>();

        /// <summary>
        /// 現在積まれているTaskのキュー.
        /// </summary>
        private readonly Queue<ITask<T>> m_currentTaskQueue = new Queue<ITask<T>>();

        public ReadOnlyCollection<T> CurrentTaskList {
            get { return m_currentTaskQueue.Select(value => value.TaskType).ToList().AsReadOnly(); }
        }

        /// <summary>
        /// 現在のTask.
        /// </summary>
        private ITask<T> m_currentTask = null;
        
        public T CurrentTaskType {
            get {
                if (m_currentTask == null) {
                    return default;
                }
                return m_currentTask.TaskType;
            }
        }
        
        public T PrevTaskType { get; private set; } = default;

        public bool IsEndAllTasks => m_currentTaskQueue.Count == 0 && m_currentTask == null;
        
        public void RegisterTask(ITask<T> task) {
            if (task == null) {
                Debug.LogError("nullです.");
                return;
            }

            var type = task.TaskType;
            if (m_registeredTaskMap.ContainsKey(type)) {
                Debug.LogError(type + "は既に登録されています。");
                return;
            }

            m_registeredTaskMap.Add(type, task);
        }

        public void RegisterTask(ITask<T>[] tasks) {
            foreach (var task in tasks) {
                RegisterTask(task);
            }
        }

        public ITaskSystem<T> EnqueueTask(T type) {
            if (!m_registeredTaskMap.TryGetValue(type, out var task)) {
                Debug.Log(type + "のタスクが登録されていません.");
                return this;
            }

            m_currentTaskQueue.Enqueue(task);
            return this;
        }
        
        public void UpdateTask() {
            //全てのTaskが終了していればreturn
            if (IsEndAllTasks) {
                return;
            }

            //現在のTaskが空の場合
            if (m_currentTask == null) {
                m_currentTask = m_currentTaskQueue.Dequeue();
                m_currentTask?.OnEnter();
            }

            //Taskの更新
            var isEndCurrentTask = m_currentTask.OnUpdate();

            //Taskが終了していれば次の処理を呼ぶ
            if (isEndCurrentTask) {
                //終了処理
                m_currentTask?.OnExit();
                PrevTaskType = CurrentTaskType;
                m_currentTask = null;
                
                //全てのTaskを終了していればreturn
                if (IsEndAllTasks) {
                    m_currentTask = null;
                    m_currentTaskQueue.Clear();
                    return;
                }

                //次のTaskを取得
                m_currentTask = m_currentTaskQueue.Dequeue();
                m_currentTask?.OnEnter();
            }
        }

        
        public void KillCurrentTask() {
            m_currentTask?.OnExit();
            if (m_currentTaskQueue.Count == 0) {
                return;
            }
            PrevTaskType = CurrentTaskType;
            m_currentTask = m_currentTaskQueue.Dequeue();
            m_currentTask?.OnEnter();
        }

        
        public void KillAllTasks() {
            m_currentTask?.OnExit();
            m_currentTask = null;
            m_currentTaskQueue.Clear();
        }
    }
}