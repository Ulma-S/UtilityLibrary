using System.Collections.ObjectModel;

namespace RitsGameSeminar.AI {
    public interface ITaskSystem<T> { 
        /// <summary>
        /// 現在積まれているTask typeのリスト.
        /// </summary>
        ReadOnlyCollection<T> CurrentTaskList { get; }

        /// <summary>
        /// 現在のTaskの種類.
        /// </summary>
        T CurrentTaskType { get; }
        
        /// <summary>
        /// 一つ前に実行されていたTaskの種類.
        /// </summary>
        T PrevTaskType { get; }
        
        /// <summary>
        /// 全てのTaskが終了しているか?
        /// </summary>
        bool IsEndAllTasks { get; }
        
        /// <summary>
        /// Taskの登録.
        /// </summary>
        void RegisterTask(ITask<T> task);
        
        /// <summary>
        /// Taskの実行待ちキューに入れる.
        /// </summary>
        /// <param name="type"></param>
        void EnqueueTask(T type);
        
        /// <summary>
        /// Taskの更新処理.
        /// </summary>
        void UpdateTask();
        
        /// <summary>
        /// 現在のTaskを強制終了する.
        /// </summary>
        void KillCurrentTask();
        
        /// <summary>
        /// 全てのTaskを強制終了する.
        /// </summary>
        void KillAllTasks();
    }
}