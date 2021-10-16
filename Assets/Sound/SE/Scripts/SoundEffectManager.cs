using System;
using System.Collections.Generic;
using UnityEngine;

namespace RitsGameSeminar.Sound {
    public class SoundEffectManager : SingletonMonoBehaviour<SoundEffectManager> {
        [Serializable]
        public class SoundEffectData {
            public ESoundEffectID ID;
            public AudioClip Clip;
        }

        [SerializeField, Tooltip("初期音量")] private float m_defaultVolume = 0.5f;
        [SerializeField, Tooltip("同時再生数")] private int m_maxPlayCount = 5;
        [SerializeField] private List<SoundEffectData> m_soundEffectDatas = new List<SoundEffectData>();
        private readonly List<AudioSource> m_audioSources = new List<AudioSource>();

        public float Volume {
            get {
                if (m_audioSources.Count > 0) {
                    if (m_audioSources[0] != null) {
                        return m_audioSources[0].volume;
                    }
                }
                return 0f;
            }
            set {
                foreach (var audioSource in m_audioSources) {
                    audioSource.volume = value;
                }
            }
        }

        private void Start() {
            //初期設定.
            for (int i = 0; i < m_maxPlayCount; i++) {
                var src = gameObject.AddComponent<AudioSource>();
                src.volume = m_defaultVolume;
                src.playOnAwake = false;
                src.loop = false;
                m_audioSources.Add(src);
            }
        }

        /// <summary>
        /// IDに対応するSEを再生するメソッド.
        /// </summary>
        /// <param name="soundEffectID"></param>
        public void Play(ESoundEffectID soundEffectID) {
            var data = m_soundEffectDatas.Find(value => value.ID == soundEffectID);
            if (data == null) {
                Debug.LogError(soundEffectID + "に対応するSEが登録されていません.");
                return;
            }

            var audioSource = m_audioSources.Find(src => !src.isPlaying);
            if (audioSource == null) {
                audioSource = m_audioSources[0];
            }
            audioSource.PlayOneShot(data.Clip);
        }

        /// <summary>
        /// 全てのSEの再生を終了するメソッド.
        /// </summary>
        public void StopAll() {
            foreach (var audioSource in m_audioSources) {
                audioSource.Stop();
            }
        }

        /// <summary>
        /// 全てのSEを一時停止するメソッド.
        /// </summary>
        public void PauseAll() {
            foreach (var audioSource in m_audioSources) {
                audioSource.Pause();
            }
        }

        /// <summary>
        /// 全ての一時停止中のSEを再開するメソッド.
        /// </summary>
        public void ResumeAll() {
            foreach (var audioSource in m_audioSources) {
                audioSource.UnPause();
            }
        }
    }
}