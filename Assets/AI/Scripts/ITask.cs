namespace RitsGameSeminar.AI {
    /// <summary>
    /// Taskの機能を提供するインターフェース.
    /// </summary>
    /// <typeparam name="T">Task type (Enum).</typeparam>
    public interface ITask<out T> {
        /// <summary>
        /// Taskの種類.
        /// </summary>
        T TaskType { get; }
        
        /// <summary>
        /// 開始処理.
        /// </summary>
        void OnEnter();
        
        /// <summary>
        /// 更新処理.
        /// </summary>
        /// <returns></returns>
        bool OnUpdate();
        
        /// <summary>
        /// 終了処理.
        /// </summary>
        void OnExit();
    }
}