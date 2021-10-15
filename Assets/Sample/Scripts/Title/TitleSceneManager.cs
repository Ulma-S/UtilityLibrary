using RitsGameSeminar.SceneManagement;
using RitsGameSeminar.UIFade;
using UnityEngine;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace RitsGameSeminar.Sample {
    public class TitleSceneManager : SingletonMonoBehaviour<TitleSceneManager> {
        private bool m_hasLoaded = false;
        
        private void Start() {
            //シーンのロード完了後にフェードインを開始する.
            //ラムダ式という書き方です.
            SceneManager.Instance.OnStageLoadedHandler += () => UIFadeManager.Instance.FadeIn();
        }

        private void Update() {
            //エンターキーが押されたらGameステージをロードする.
            if (Input.GetKeyDown(KeyCode.Return) && !m_hasLoaded) {
                UIFadeManager.Instance.FadeOut(() => {
                    SceneManager.Instance.LoadStage(EStageID.Game);
                });
                m_hasLoaded = true;
            }
        }
    }
}