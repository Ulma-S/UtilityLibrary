using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RitsGameSeminar.Sound {
    /// <summary>
    /// BGMプレイヤー機能を提供するクラス.
    /// </summary>
    public class BgmManager : SingletonMonoBehaviour<BgmManager> {
        [Serializable]
        public class BgmData {
            public EBgmID ID;
            public AudioClip Clip;
        }
        
        /// <summary>
        /// BGMの再生状態.
        /// </summary>
        public enum EBgmState {
            None,
            Play,
            Pause,
        }

        [SerializeField, Tooltip("初期音量")] private float m_defaultVolume = 0.5f;
        [SerializeField] private AudioSource m_audioSource;
        [SerializeField] private List<BgmData> m_bgmDatas = new List<BgmData>();
        
        /// <summary>
        /// フェード中か?
        /// </summary>
        private bool m_isFade = false;

        /// <summary>
        /// BGMの音量.
        /// </summary>
        public float Volume {
            get => m_audioSource.volume;
            set {
                if (!m_isFade) {
                    m_audioSource.volume = value;
                }
            }
        }

        /// <summary>
        /// 現在の再生状態.
        /// </summary>
        public EBgmState CurrentState { get; private set; } = EBgmState.None;

        /// <summary>
        /// 現在再生中のBGMのID.
        /// </summary>
        public EBgmID CurrentBgmID { get; private set; } = EBgmID.None;

        private void Start() {
            m_audioSource.volume = m_defaultVolume;
            m_audioSource.loop = true;
        }

        /// <summary>
        /// IDに対応するBGMを再生するメソッド.
        /// </summary>
        /// <param name="bgmID"></param>
        public void Play(EBgmID bgmID) {
            var data = m_bgmDatas.Find(value => value.ID == bgmID);
            if (data == null) {
                Debug.LogError(bgmID + "に対応するBGMデータが登録されていません."); 
                return;
            }
            m_audioSource.clip = data.Clip;
            m_audioSource.Play();
            CurrentBgmID = bgmID;
            CurrentState = EBgmState.Play;
        }

        /// <summary>
        /// BGMの再生を終了するメソッド.
        /// </summary>
        public void Stop() {
            m_audioSource.Stop();
            CurrentBgmID = EBgmID.None;
            CurrentState = EBgmState.None;
        }

        /// <summary>
        /// 再生を一時停止するメソッド.
        /// </summary>
        public void Pause() {
            if (CurrentState != EBgmState.Play) {
                return;
            }
            m_audioSource.Pause();
            CurrentState = EBgmState.Pause;
        }
        
        /// <summary>
        /// 一時停止中のBGMを再開するメソッド.
        /// </summary>
        public void Resume() {
            //一時停止中でなければ何もしない.
            if (CurrentState != EBgmState.Pause) {
                return;
            }
            m_audioSource.UnPause();
            CurrentState = EBgmState.Play;
        }

        /// <summary>
        /// BGMをフェードインで再生するメソッド.
        /// </summary>
        public void FadeIn(EBgmID bgmID, float duration = 1f) {
            StartCoroutine(FadeInCoroutine(bgmID, duration));
        }

        /// <summary>
        /// BGMをフェードアウトするメソッド.
        /// </summary>
        /// <param name="duration"></param>
        public void FadeOut(float duration = 1f) {
            //再生中でなければ何もしない.
            if (CurrentState != EBgmState.Play) {
                return;
            }
            StartCoroutine(FadeOutCoroutine(duration));
        }

        private IEnumerator FadeInCoroutine(EBgmID bgmID, float duration) {
            m_isFade = true;
            //現在の音量を保存.
            var targetVolume = Volume;
            m_audioSource.volume = 0f;
            Play(bgmID);
            
            while (true) {
                m_audioSource.volume += (targetVolume / duration) * Time.deltaTime;
                if (m_audioSource.volume >= targetVolume) {
                    m_audioSource.volume = targetVolume;
                    break;
                }
                yield return null;
            }
            m_isFade = false;
        }

        private IEnumerator FadeOutCoroutine(float duration) {
            m_isFade = true;
            var originVolume = Volume;
            while (true) {
                m_audioSource.volume -= (originVolume / duration) * Time.deltaTime;
                if (m_audioSource.volume <= 0f) {
                    m_audioSource.volume = 0f;
                    break;
                }
                yield return null;
            }
            Stop();
            m_isFade = false;
        }

        /// <summary>
        /// ミュートするメソッド.
        /// </summary>
        public void Mute() {
            m_audioSource.volume = 0f;
        }
        
        private void Reset() {
            m_audioSource = GetComponent<AudioSource>();
        }
    }
}