using System;
using System.Collections;
using UnityEngine;

namespace RitsGameSeminar.UIFade {
    /// <summary>
    /// 画面のフェード機能を提供するクラス.
    /// </summary>
    public class UIFadeManager : SingletonMonoBehaviour<UIFadeManager> {
        [SerializeField] private FadeCutoutController m_cutoutController;
        [SerializeField, Tooltip("模様の最大サイズ")] private float m_maxScale = 3f;
        [SerializeField, Tooltip("フェード時間")] private float m_fadeDuration = 2f;
        
        /// <summary>
        /// フェードイン終了時の処理を登録するハンドラ.
        /// </summary>
        public event Action OnFadeInFinishedHandler;
        
        /// <summary>
        /// フェードアウト終了時の処理を登録するハンドラ.
        /// </summary>
        public event Action OnFadeOutFinishedHandler;

        /// <summary>
        /// フェードイン.
        /// </summary>
        /// <param name="onFadeInFinishedCallback"></param>
        public void FadeIn(Action onFadeInFinishedCallback = null) {
            StartCoroutine(FadeInCoroutine(onFadeInFinishedCallback));
        }

        /// <summary>
        /// フェードアウト.
        /// </summary>
        /// <param name="onFadeOutFinishedCallback"></param>
        public void FadeOut(Action onFadeOutFinishedCallback = null) {
            StartCoroutine(FadeOutCoroutine(onFadeOutFinishedCallback));
        }

        private IEnumerator FadeInCoroutine(Action onFadeInFinishedCallback = null) {
            var time = 0f;
            while (true) {
                time += Time.deltaTime;
                var scale = DOInExpo(time) * m_maxScale;
                m_cutoutController.Scale = scale;
                
                yield return null;
                
                if (time >= m_fadeDuration) {
                    m_cutoutController.Scale = m_maxScale;
                    break;
                }
            }
            onFadeInFinishedCallback?.Invoke();
            OnFadeInFinishedHandler?.Invoke();
        }

        private IEnumerator FadeOutCoroutine(Action onFadeOutFinishedCallback = null) {
            var time = 0f;
            while (true) {
                time += Time.deltaTime;
                var scale = m_maxScale - DOOutExpo(time) * m_maxScale;
                m_cutoutController.Scale = scale;
                
                yield return null;
                
                if (time >= m_fadeDuration) {
                    m_cutoutController.Scale = 0f;
                    break;
                }
            }
            onFadeOutFinishedCallback?.Invoke();
            OnFadeOutFinishedHandler?.Invoke();
        }

        private float DOInExpo(float value) {
            return Mathf.Pow(2.5f, 13f * (value / m_fadeDuration - 1f));
        }

        private float DOOutExpo(float value) {
            return 1f - Mathf.Pow(2f, -15f * value / m_fadeDuration);
        } 
    }
}