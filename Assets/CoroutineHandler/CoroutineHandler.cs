using System.Collections;
using UnityEngine;

namespace RitsGameSeminar {
    /// <summary>
    /// 委譲されたコルーチンの実行を管理するクラス.
    /// </summary>
    public class CoroutineHandler : SingletonMonoBehaviour<CoroutineHandler> {
        /// <summary>
        /// コルーチンの開始.
        /// </summary>
        /// <param name="coroutine"></param>
        /// <returns></returns>
        public Coroutine StartStaticCoroutine(IEnumerator coroutine) {
            return Instance.StartCoroutine(coroutine);
        }

        /// <summary>
        /// コルーチンの一時停止.
        /// </summary>
        /// <param name="coroutine"></param>
        public void StopStaticCoroutine(IEnumerator coroutine) {
            Instance.StopCoroutine(coroutine);
        }
    }
}