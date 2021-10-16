using UnityEngine;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace RitsGameSeminar.Sample {
    public static class RuntimeInitializer {
        //シーンの読み込み前に実行されるメソッド.
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void LoadPermanentScene() {
            //常駐シーンがロードされていなければロードする.
            for (int i = 0; i < UnitySceneManager.sceneCount; i++) {
                if (UnitySceneManager.GetSceneAt(i).name == "Manager") {
                    break;
                }

                if (i == UnitySceneManager.sceneCount - 1) {
                    UnitySceneManager.LoadScene("Manager", LoadSceneMode.Additive);
                }
            }
        }

        //参照を注入するメソッド.
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Inject() {
            //IResourceProviderに対応する実体の登録.
            ServiceLocator.Register<IResourceProvider>(new DebugResourceProvider());
            ServiceLocator.Resolve<IResourceProvider>().RegisterResource<PlayerStatus>(EResourceID.PlayerStatus, "PlayerStatus");
        }
    }
}