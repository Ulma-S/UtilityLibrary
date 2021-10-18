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
  //例) GameManager Componentがシーン上の複数のGameObjectにアタッチされている場合、1つになるよう破棄されます.
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
      //以下は自由に定義してください.
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
  
### BgmManager
- 概要<br>
  BGMの再生機能を提供します.
    
- 準備<br>
  
  1. EBgmID.csが存在しない場合は作成し、以下のように定義します.<br>
     (本ライブラリのスクリプトを全てインポートする場合は既に存在するため、列挙要素の追加のみ行ってください.)
     ```c#
     //EBgmID.cs
     public enum EBgmID{
         None,
         //以下は自由に定義してください.
         Title,
         Game,
     }
  2. 常駐シーンにBGMManagerプレハブを配置します.
  3. BGMManagerプレハブのBgmManager ComponentのBgmDatasに、対応するIDとAudioClipを設定します.
    
- 使い方<br>
  ```c#
  using namespace RitsGameSeminar.Sound;
  //音楽の再生.
  //例) Game曲の再生.
  BgmManager.Instance.Play(EBgmID.Game);
  
  //再生を終了する.
  BgmManager.Instance.Stop();
  
  //一時停止する.
  BgmManager.Instance.Pause();
  
  //再開する.
  BgmManager.Instance.Resume();
  
  //フェードインで再生する.
  //例) Game曲の再生.
  BgmManager.Instance.FadeIn(EBgmID.Game);
  //フェード時間(秒)を指定することもできます.
  BgmManager.Instance.FadeIn(EBgmID.Game, 2f);
  
  //フェードアウトで再生を終了する.
  BgmManager.Instance.FadeOut();
  //フェード時間(秒)を指定することもできます.
  BgmManager.Instance.FadeOut(2f);
  
  //ミュートする.
  BgmManager.Instance.Mute();
  
  //音量を取得、変更する.
  //取得
  Debug.Log(BgmManager.Instance.Volume);
  //変更 (フェード中は変更できません)
  BgmManager.Instance.Volume = 0.8f;
  
  //現在の再生状態を取得する.
  //None, Play, Pauseのいずれかが返されます.
  Debug.Log(BgmManager.Instance.CurrentState);
  
  //現在再生されている曲のIDを取得する.
  Debug.Log(BgmManager.Instance.CurrentBgmID);
  ```

### SoundEffectManager
- 概要<br>
  SEの再生機能を提供します.

- 準備<br>
  1. ESoundEffectID.csが存在しない場合は作成し、以下のように定義します.<br>
     (本ライブラリのスクリプトを全てインポートする場合は既に存在するため、列挙要素の追加のみ行ってください.)
     ```c#
     //ESoundEffectID.cs
     public enum ESoundEffectID{
         None,
         //以下は自由に定義してください.
         Hit,
     }
  2. 常駐シーンにSEManagerプレハブを配置します.
  3. SEManagerプレハブのSeManager ComponentのMaxPlayCountに最大同時再生数を設定します.
  4. SEManagerプレハブのSeManager ComponentのSoundEffectDatasに、対応するIDとAudioClipを設定します.

- 使い方<br>
  ```c#
  using namespace RitsGameSeminar.Sound;
  
  //SEの再生.
  //例) ヒット音の再生.
  SoundEffectManager.Instance.Play(ESoundEffectID.Hit);
  
  //全てのSEの再生を終了.
  SoundEffectManager.Instance.StopAll();
  
  //全てのSEを一時停止.
  SoundEffectManager.Instance.PauseAll();
  
  //全てのSEを再開.
  SoundEffectManager.Instance.ResumeAll();
  
  //音量の取得、変更.
  //取得
  Debug.Log(SoundEffectManager.Instance.Volume);
  //変更
  SoundEffectManager.Instance.Volume = 0.8f;
  ```
  
### VisualEffectManager
- 概要<br>
  視覚エフェクトの再生機能を提供します.

- 準備<br>
  1. EVisualEffectID.csが存在しない場合は作成し、以下のように定義します.<br>
     (本ライブラリのスクリプトを全てインポートする場合は既に存在するため、列挙要素の追加のみ行ってください.)
     ```c#
     //EVisualEffectID.cs
     public enum EVisualEffectID{
         None,
         //以下は自由に定義してください.
         Hit,
     }
  2. 常駐シーンにVFXManagerプレハブを配置します.
  3. VFXManagerプレハブのVisualEfffectManager ComponentのVisualEffectDatasに、対応するIDとエフェクトを設定します.

- 使い方<br>
  ```c#
  using namespace RitsGameSeminar.VFX;
  
  //エフェクトを再生.
  //例) ヒットエフェクトの再生.
  VisualEffectManager.Instance.Play(EVisualEffectID.Hit);
  ```

### StateMachine
- 概要<br>
  状態遷移を管理するための機能を提供します.
  
- 使い方<br>
  1. 状態の種類を示す列挙型を作成します.
     ```c#
     //例) プレイヤーキャラクターの状態がIdle, Run, Jumpの場合
     public enum EPlayerStateType{
         None,  //必ず記述してください.
         Idle,
         Run,
         Jump,
     }
     ```
  2. それぞれの状態に対応するクラスを作成します.
     ```c#
     using namespace RitsGameSeminar.StateMachine;
     
     //例) プレイヤーキャラクターのIdle State
     //    必ずStateBaseクラスを継承してください.
     public class PlayerIdleState : StateBase<EPlayerStateType> {
         public PlayerIdleState(StateMachine<EPlayerStateType> stateMachine) : base(stateMachine) {}
         public override EPlayerStateType StateType { get; protected set; } = EPlayerStateType.Idle;
         
         public override void OnEnter() {}
         public override void OnUpdate() {}
         public override void OnExit() {}
     }
     
     //同様に他のクラスも作成します.
     //中身は省略.
     public class PlayerRunState : StateBase<EPlayerStateType> {}
     public class PlayerJumpState : StateBase<EPlayerStateType> {}
     ```
  3. 2で作成したクラスをStateMachineに登録します.
     ```c#
     using namespace RitsGameSeminar.StateMachine;
     
     public class PlayerController : MonoBehaviour {
         private StateMachine<EPlayerStateType> m_stateMachine;
         
         private void Start() {
             m_stateMachine = new StateMachine<EPlayerStateType>(gameObject);
             
             //Stateの登録.
             m_stateMachine.RegisterState(new PlayerIdleState(m_stateMachine));
             m_stateMachine.RegisterState(new PlayerRunState(m_stateMachine));
             m_stateMachine.RegisterState(new PlayerJumpState(m_stateMachine));
             
             //最初のStateを設定.
             m_stateMachine.ChangeState(EPlayerStateType.Idle);
         }
     }
     
     //別の状態に遷移したい場合
     //例) Idle -> Run
     public class PlayerIdleState : StateBase<EPlayerStateType> {
         //一部省略
         public override void OnUpdate() {
             //左右入力が入ったらRun Stateに遷移.
             if(Mathf.Abs() > 0){
                 stateMachine.ChangeState(EPlayerTaskType.Run);
             }
         }
     }
     ```
