using RitsGameSeminar.SceneManagement;
using UnityEngine;
using RitsGameSeminar.UIFade;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace RitsGameSeminar.Sample {
    public class GameSceneManager : MonoBehaviour {
        private IInputProvider m_inputProvider;
        private bool m_hasLoaded = false;

        private void Start() {
            m_inputProvider = ServiceLocator.Resolve<IInputProvider>();
            
            //シーンのロード完了後にフェードインを開始する.
            //ラムダ式という書き方です.
            SceneManager.Instance.OnStageLoadedHandler += () => {
                UIFadeManager.Instance.FadeIn();
            };
            
            //以下の処理は正確には完了するまで待機する必要があります.
            //セーブデータがあれば読み込む.
            if (SaveManager.Instance.ExistSaveData()) {
                SaveManager.Instance.Load();
            }
            //なければ新規作成.
            else {
                SaveManager.Instance.CreateSaveData();
            }
        }

        private void Update() {
            if (m_inputProvider.IsEscapeButtonDown && !m_hasLoaded) {
                UIFadeManager.Instance.FadeOut(() => {
                    SceneManager.Instance.LoadStage(EStageID.Title);
                });
                m_hasLoaded = true;
            }

            if (m_inputProvider.IsReturnButtonDown) {
                Debug.Log(SaveManager.Instance.SaveData.Level);
                var data = SaveManager.Instance.SaveData;
                data.Level++;
            }
        }
    }
}