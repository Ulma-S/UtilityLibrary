# UtilityLibrary
## 本リポジトリについて
Unity向けの汎用ライブラリです.
1. Singleton
2. SceneManager (複数Sceneの遷移)
3. UIFadeManager (画面のフェード)
4. BgmManager (BGMの再生機能管理)
5. SoundEffectManager (SEの再生機能管理)
6. VisualEffectManager (VFXの再生機能管理)

以下は少し難しいかもしれません.<br>

7. StateMachine (状態遷移管理)
8. TaskSystem (ルールベースAIの線形的な行動制御)
9. ServiceLocator (簡易的な依存性の注入)
11. ResourceProvider (動的リソースの管理)

## 詳細説明
### 常駐Sceneについて
- 概要<br>
  Unityでは複数のSceneを同時に読み込むことができます.<br>
  本ライブラリではこの仕組みを応用し、常駐Sceneを使った管理手法を複数個所で使用しています.<br>

- 使い方<br>
  例) Manager Sceneを常駐Sceneとする場合
  1. Managerという名前のSceneを作成します.
  2. 以下のスクリプトを作成します. <br>
    (サンプルで既に作成済みのため、本ライブラリのスクリプトを全てインポートする場合2の手順は不要です.)
    ```c#
    //RuntimeInitializer.cs
    public static class RuntimeInitializer {
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
    }
    ```
    配置は任意の場所で結構です.
    以上により、実行時に自動で常駐Sceneを読み込むようになります.

### Singleton / SingletonMonoBehaviour
- 概要<br>
  Scene全体で対象クラスのインスタンスが1つのみであることを保証します。<br>
  
- 準備<br>
  ```c#
  using namespace RitsGameSeminar;
  
  //MonoBehaviourを継承する場合
  //例) GameManager.cs
  //例えばGameManager Componentがシーン上の複数のGameObjectにアタッチされている場合、自動的に1つになるよう破棄されます.
  public class GameManager : SingletonMonoBehaviour<GameManager> {}
  
  //MonoBehaviourが不要な場合
  //例) CsvReader.cs
  public class CsvReader : Singleton<CsvReader> {}
  ```
  
### SceneManager
- 概要<br>
  Stage IDに対応するSceneの遷移機能を提供します.<br>
  
- 準備<br>
  1. EStageID.csが存在しない場合は作成し、以下のように定義します.<br>
    (本ライブラリのスクリプトを全てインポートする場合は既に存在するため、列挙要素の追加のみ行ってください.)
  ```c#
  //EStageID.cs
  public enum EStageID{
      None,
      Title,
      Game,
  }
  ```
  2. 常駐SceneにSceneManagerプレハブを配置します.
  3. SceneManagerプレハブのStageListProvider ComponentのStageDatas/SceneListに、対応するScene名を入力します.<br>
    例) サンプルの場合<br>
        StageID:Title - StageList:Title<br>
        StageID:Game - StageList:Game, GameUI<br>
        
- 使い方<br>
  ```c#
  using namespace RitsGameSeminar.SceneManagement;
  
  //例) Game Stageを読み込む.
  //現在のSceneが破棄され、Game, GameUI Sceneが読み込まれる.
  SceneManager.Instance.LoadStage(EStageID.Game);
  
  //応用 (コールバックの利用)
  //Scene遷移終了後に呼ばれる処理を登録したい場合
  SceneManager.Instance.OnStageLoadedHandler += () => {
      Debug.Log("遷移終了");
  };
  ```

- 注意<br>
  それぞれのSceneListでは一番上に記述されたSceneがActive Sceneとして設定されます.
  
### UIFadeManager
- 概要<br>
画面のフェード機能を提供します.<br>

- 準備<br>
  1. 常駐SceneにUIFadeManagerプレハブを配置します.

- 使い方<br>
  ```c#
  using namespace RitsGameSeminar.UIFade;
  
  //フェードイン.
  UIFadeManager.Instance.FadeIn();
  
  //フェードアウト.
  UIFadeManager.Instance.FadeOut();
  
  //応用 (コールバックの利用)
  //フェードイン/アウト後に呼ばれる処理を登録したい場合
  //例) フェードイン後に初期化処理.
  UIFadeManager.Instance.FadeIn(() => {
      //ここに初期化処理を書く.
  });
  //以下のようにも書けます.
  UIFadeManager.Instance.OnFadeInFinishedHandler += () => {
      //ここに初期化処理を書く.
  };
  UIFadeManager.Instance.FadeIn();
  
  //例) フェードアウト後にScene遷移.
  UIFadeManager.Instance.FadeOut(() => {
      SceneManager.Instance.LoadStage(EStageID.Game);
  });
  //以下のようにも書けます.
  UIFadeManager.Instance.OnFadeOutFinishedHandler += () => {
      SceneManager.Instance.LoadStage(EStageID.Game);
  };
  UIFadeManager.Instance.FadeOut();
  ```
