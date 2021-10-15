using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace RitsGameSeminar.SceneManagement {
    /// <summary>
    /// ステージに対応するシーン遷移機能を提供するクラス.
    /// </summary>
    public class SceneManager : SingletonMonoBehaviour<SceneManager> {
        [SerializeField, Tooltip("常駐シーン名")] private string m_permanentSceneName = "Manager";

        /// <summary>
        /// シーンのロード完了時の処理を登録するハンドラ.
        /// </summary>
        public event Action OnStageLoadedHandler;
        
        /// <summary>
        /// StageIDに対応するシーンをまとめて読み込むメソッド.
        /// </summary>
        /// <param name="stageID"></param>
        public void LoadStage(EStageID stageID) {
            StartCoroutine(LoadStageCoroutine(stageID));
        }

        private IEnumerator LoadStageCoroutine(EStageID stageID) {
            //常駐シーン以外をアンロード.
            var unloadSceneNames = new List<string>();
            for (int i = 0; i < UnitySceneManager.sceneCount; i++) {
                var sceneName = UnitySceneManager.GetSceneAt(i).name;
                if (sceneName == m_permanentSceneName) {
                    continue;
                }
                unloadSceneNames.Add(sceneName);
            }

            foreach (var sceneName in unloadSceneNames) {
                yield return UnitySceneManager.UnloadSceneAsync(sceneName);
            }
            
            //シーンをまとめてロード.
            var sceneNames = StageListProvider.Instance.GetSceneNames(stageID);
            var loadSceneNames = new List<string>();
            for (int i = 0; i < sceneNames.Count; i++) {
                loadSceneNames.Add(sceneNames[i]);
            }

            foreach (var sceneName in loadSceneNames) {
                yield return UnitySceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            }
            
            //アクティブシーンを設定.
            for (int i = 0; i < UnitySceneManager.sceneCount; i++) {
                if (UnitySceneManager.GetSceneAt(i).name == sceneNames[0]) {
                    UnitySceneManager.SetActiveScene(UnitySceneManager.GetSceneAt(i));
                }
            }
            
            //読み込み完了時のコールバック.
            OnStageLoadedHandler?.Invoke();
        }
    }
}
