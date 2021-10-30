using RitsGameSeminar.SceneManagement;
using RitsGameSeminar.UIFade;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace RitsGameSeminar.Sample {
    public class TitleSceneManager : SingletonMonoBehaviour<TitleSceneManager> {
        private IInputProvider m_inputProvider;
        private bool m_hasLoaded = false;
        
        private void Start() {
            m_inputProvider = ServiceLocator.Resolve<IInputProvider>();
            
            //シーンのロード完了後にフェードインを開始する.
            //ラムダ式という書き方です.
            SceneManager.Instance.OnStageLoadedHandler += () => UIFadeManager.Instance.FadeIn();
        }

        private void Update() {
            //エンターキーが押されたらGameステージをロードする.
            if (m_inputProvider.IsReturnButtonDown && !m_hasLoaded) {
                UIFadeManager.Instance.FadeOut(() => {
                    SceneManager.Instance.LoadStage(EStageID.Game);
                });
                m_hasLoaded = true;
            }
        }
    }
}