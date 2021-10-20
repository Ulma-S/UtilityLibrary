using System.Collections;
using UnityEngine;

namespace RitsGameSeminar {
    /// <summary>
    /// 委譲されたコルーチンの実行を管理するクラス.
    /// </summary>
    public class CoroutineManager : SingletonMonoBehaviour<CoroutineManager> {
        /// <summary>
        /// コルーチンの開始.
        /// </summary>
        /// <param name="coroutine"></param>
        /// <returns></returns>
        public Coroutine StartDelegatedCoroutine(IEnumerator coroutine) {
            return Instance.StartCoroutine(coroutine);
        }

        /// <summary>
        /// コルーチンの一時停止.
        /// </summary>
        /// <param name="coroutine"></param>
        public void StopDelegatedCoroutine(IEnumerator coroutine) {
            Instance.StopCoroutine(coroutine);
        }
    }
}