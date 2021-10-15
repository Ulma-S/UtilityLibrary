using RitsGameSeminar.SceneManagement;
using UnityEngine;
using RitsGameSeminar.UIFade;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace RitsGameSeminar.Sample {
    public class GameSceneManager : MonoBehaviour {
        private bool m_hasLoaded = false;
        
        private void Start() {
            //シーンのロード完了後にフェードインを開始する.
            //ラムダ式という書き方です.
            SceneManager.Instance.OnStageLoadedHandler += () => UIFadeManager.Instance.FadeIn();
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Escape) && !m_hasLoaded) {
                UIFadeManager.Instance.FadeOut(() => {
                    SceneManager.Instance.LoadStage(EStageID.Title);
                });
                m_hasLoaded = true;
            }
        }
    }
}