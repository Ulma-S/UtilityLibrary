# UtilityLibrary
## 本リポジトリについて
Unity向けの汎用ライブラリです.
- [常駐Scene](#常駐Scene)
- [拡張メソッド](#拡張メソッド)
- [StreamingAssets](#StreamingAssets)
- [AssetBundle](#AssetBundle)
1. [Singleton](#Singleton)
2. [SceneManager](#SceneManager) (複数Sceneの遷移)
3. [UIFadeManager](#UIFadeManager) (画面のフェード)
4. [BgmManager](#BgmManager) (BGMの再生機能管理)
5. [SoundEffectManager](#SoundEffectManager) (SEの再生機能管理)
6. [VisualEffectManager](#VisualEffectManager) (VFXの再生機能管理)
7. [SaveManager](#SaveManager) (セーブデータの管理) 
8. [CsvReader](#CsvReader) (csvファイルの読み込み)

以下は少し難しいかもしれません.<br>

9. [StateMachine](#StateMachine) (状態遷移管理)
10. [TaskSystem](#TaskSystem) (ルールベースAIの線形的な行動制御)
11. [BehaviourTree](#BehaviourTree) (ビヘイビアベースAI)
12. [CoroutineManager](#CoroutineManager) (非MonoBehaviourクラスからコルーチン処理を委譲)
13. [ServiceLocator](#ServiceLocator) (簡易的な依存性の注入)
14. [ResourceProvider](#ResourceProvider) (動的リソースの管理)

## 詳細説明
<a id="常駐Scene"></a>
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
    
<a id="拡張メソッド"></a>
### 拡張メソッド
- 概要<br>
  既存のクラスに外部からメソッドを追加する仕組みです.<br>
  本ライブラリではUnityの既存クラスをいくつか拡張しています.

- 作り方<br>
  ```c#
  //例) Transformクラスを拡張する.
  //    positionの各要素をそれぞれ変更できるようにする.
  //TransformExtension.cs
  public class TransformExtension {
      public static void SetPositionX(this Transform self, float value) {
          var position = self.transform.position;
          position.x = value;
          self.transform = position;
      }
      //以下略
  } 
  ```
  
- 使い方<br>
  ```c#
  //例) 座標のx成分のみ設定する.
  transform.SetPositionX(2f);
  ```

<a id="StreamingAssets"></a>
### StreamingAssets
- 概要<br>
  動的リソースを管理する仕組みの一つです.<br>
  適切に使用することで、Resourcesに比べパフォーマンスを向上させることができます.<br>
  
<a id="AssetBundle"></a>
### AssetBundle
- 概要<br>
  動的リソースを管理する仕組みの一つです.<br>
  ビルド後に外部からリソースを差し替えることができます.<br>
  
- 使い方<br>
  AssetBundleをビルドする必要があります.<br>
  詳しくは[Asset Bundle Browser](https://docs.unity3d.com/ja/2019.4/Manual/AssetBundles-Browser.html)を参考にしてください.<br>

<a id="Singleton"></a>
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
<a id="SceneManager"></a>
### SceneManager
- 概要<br>
  Stage IDに対応するSceneの遷移機能を提供します.<br>
  
- 準備<br>
  1. EStageID.csを以下のように定義します.<br>
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
  
<a id="UIFadeManager"></a>
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
<a id="BgmManager"></a>  
### BgmManager
- 概要<br>
  BGMの再生機能を提供します.
    
- 準備<br>
  
  1. EBgmID.csを以下のように定義します.<br>
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
<a id="SoundEffectManager"></a>
### SoundEffectManager
- 概要<br>
  SEの再生機能を提供します.

- 準備<br>
  1. ESoundEffectID.csを以下のように定義します.<br>
     ```c#
     //ESoundEffectID.cs
     public enum ESoundEffectID{
         None,
         //以下は自由に定義してください.
         Hit,
     }
     ```
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
<a id="VisualEffectManager"></a>
### VisualEffectManager
- 概要<br>
  視覚エフェクトの再生機能を提供します.

- 準備<br>
  1. EVisualEffectID.csを以下のように定義します.<br>
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
<a id="SaveManager"></a>
### SaveManager
- 概要<br>
  セーブデータを管理するための機能を提供します.<br>
  
- 準備<br>
  - アプリケーション中断時にオートセーブする.<br>
    SaveManager componentのインスペクタプロパティ`Enable Save On Pause`をチェックする.<br>
    
  - アプリケーション終了時にオートセーブする.<br>
    SaveManager componentのインスペクタプロパティ`Enable Save On Quit`をチェックする.<br>
    
  - セーブデータを暗号化する.<br>
    SaveManager componentのインスペクタプロパティ`Enable Encription`をチェックする.<br>
    セーブデータが既に存在する状態でこのプロパティを切り替える際は、先にセーブデータを削除してください.<br>
    
  - セーブデータの形式を決定する.<br>
    以下のスクリプトを編集してください.<br>
    ```c#
    //SaveData.cs
    namespace RitsGameSeminar {
        [System.Serializable]
        public class SaveData {
           //例) 名前.
           public string Name;
           
           //例) レベル.
           public int Level;
        }
    }
    ```

- 使い方<br>
  ```c#
  using namespace RitsGameSemiar;
  
  public class GameManager : MonoBehaviour {
      private void Awake() {
          //各タイミングで処理を実行したい場合は、以下のように処理を登録してください.
          //各処理のメソッドに引数として登録することも可能です. (APIリファレンス参照)
          //セーブ終了時.
          SaveManager.Instance.OnSaveFinishedHandler += () => { Debug.Log("Save finished."); };
          
          //ロード終了時.
          SaveManager.Instance.OnLoadFinishedHandler += () => { Debug.Log("Load finished."); };
      
          //セーブデータ作成時.
          SaveManager.Instance.OnSaveDataCreatedHandler += () => { Debug.Log("SaveData created."); };
      
          
          //セーブデータの有無によって処理を分岐.
          //セーブデータが存在するならロード.
          if (ExistSaveData()) {
              SaveManager.Instance.Load();
          }
          //セーブデータが存在しないなら新規作成.
          else {
              SaveManager.Instance.CreateSaveData();
          }
      }
      
      private void Update() {
          //スペースキーを押したらセーブ.
          if (Input.GetKeyDown(KeyCode.Space) {
              SaveManager.Instance.Save();
          }
          
          //セーブデータにアクセスする.
          //例) レベルを出力.
          Debug.Log(SaveManager.Instance.SaveData.Level);
      }
  }
  ```
  
- APIリファレンス<br>
  ```c#
  //SaveManager.cs
  //セーブデータが存在するか判定する.
  bool ExistSaveData();
  
  //セーブする.
  void Save(Action onSaveFinishedCallback = null);
  
  //ロードする.
  void Load(Action onLoadFinishedCallback = null);
  
  //セーブデータを作成する.
  void CreateSaveData(Action onSaveDataCreatedCallback = null);
  
  //セーブ終了時のevent.
  Action OnSaveFinishedHandler;
  
  //ロード終了時のevent.
  Action OnLoadFinishedHandler;
  
  //セーブデータ作成時のevent.
  Action OnSaveDataCreatedHandler;
  ```
  
<a id="CsvReader"></a>
### CsvReader
- 概要<br>
  csvファイルを読み込むための機能を提供します.<br>

- 使い方<br>
  ```c#
  using namespace RitsGameSeminar;
  
  //mapdataファイルを読み込む場合
  List<string[]> data = CsvReader.Instance.Load(mapdata);
  
  //後述のServiceLocator, ResourceProviderを併用する場合
  //ResourceProviderの項目を参考にしてassetを登録します.
  //抜粋
  public enum EResourceID {
      MapData,
  }
  List<string[]> data = CsvReader.Instance.Load(MapData);
  ```
  

<a id="StateMachine"></a>
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
  3. 2で作成したクラスのインスタンスをStateMachineに登録します.
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
             if(Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0){
                 stateMachine.ChangeState(EPlayerTaskType.Run);
             }
         }
     }
     ```
<a id="TaskSystem"></a>
### TaskSystem
- 概要<br>
  Taskを単位としてAIを制御する仕組みを提供します.

- 使い方<br>
  1. Taskの種類を示す列挙型を作成します.
     ```c#
     //例) 敵キャラクターのTaskがMove, Attack, PowerUpの場合
     public enum EEnemyTaskType{
         None,  //必ず記述してください.
         Move,
         Attack,
         PowerUp,
     }
     ```
  2. それぞれのTaskに対応するクラスを作成します.
     ```c#
     using namespace RitsGameSeminar.AI;
     
     //例) 敵キャラクターのMove Task
     //    必ずITask<T>を実装してください.
     public class EnemyMoveTask : ITask<EEnemyTaskType> {
         public EEnemyTaskType TaskType { get; } = EEnemyTaskType.Move;
         public void OnEnter() {}
         //TaskはOnUpdateでtrueが返されるまで更新されます.
         public bool OnUpdate() { return false; }
         public void OnExit() {}
     }
     //同様に他のクラスも作成します.
     //中身は省略.
     public class EnemyAttackTask {}
     public class EnemyPowerUpTask {}
     ```
  3. 2で作成したクラスのインスタンスをTaskSystemに登録します.
     ```c#
     using namespace RitsGameSeminar.AI;
     
     public class EnemyTaskController : MonoBehaviour {
         private ITaskSystem<EEnemyTaskType> m_taskSystem;
         
         private void Start() {
             m_taskSystem = new TaskSystem<EEnemyTaskType>();
             
             //Taskの登録.
             m_taskSystem.RegisterTask(new EnemyMoveTask(gameObject));
             
             //まとめて登録する際は、以下のように書けます.
             m_taskSystem.RegisterTask(new ITask<EEnemyTaskType>[] {
                 new EnemyMoveTask(gameObject),
                 new EnemyAttackTask(gameObject),
                 new EnemyPowerUpTask(gameObject),
             });
         }
     }
     ```
  4. TaskSystemの実行待ちQueueに実行したいTaskを入れます.
     ```c#
     using namespace RitsGameSeminar.AI; 
     
     public class EnemyTaskController : MonoBehaviour {
         //一部省略.         
         private void Start() {
             //例) Move Taskを入れる.
             m_taskSystem.EnqueueTask(EEnemyTaskType.Move);
             
             //まとめて入れたい場合は、メソッドチェーンを利用して以下のように書けます.
             m_taskSystem.EnqueueTask(EEnemyTaskType.Move).EnqueueTask(EEnemyTaskType.Attack);
         }
     }
     ```
  5. TaskSystemを更新し、必要に応じて実行待ちQueueにTaskを入れます.
     ```c#
     using namespace RitsGameSeminar.AI;
     
     public class EnemyTaskController : MonoBehaviour {
         //一部省略.
         private void Update() {
             //Taskがすべて完了していたら新たに入れる.
             if(m_taskSystem.IsEndAllTasks) {
                 SelectTask();
             }
             //Taskの更新
             m_taskSystem.UpdateTask();
         }
         
         //この中で適宜Taskを入れてください.
         private void SelectTask() {
             //例
             m_taskSystem.EnqueueTask(EEneymTask.Move).EnqueueTask(EEnemyTask.Attack);
         }
     }
     ```

- APIリファレンス
  ```c#
  //interface ITaskSystem<T>
  //現在入っているTaskの種類のlist.
  ReadOnlyCollection<T> CurrentTaskList;
  
  //現在のTaskの種類.
  T CurrentTaskType;
  
  //全てのTaskが終了しているか?
  bool IsEndAllTasks;
  
  //Taskの登録.
  void RegisterTask(ITask<T> task);
  void RegisterTask(ITask<T>[] tasks);
  
  //Taskを実行待ちQueueに入れる.
  ITaskSystem<T> EnqueueTask(T type);
  
  //Taskの更新.
  void UpdateTask();
  
  //現在のTaskを強制終了する.
  void KillCurrentTask();
  
  //現在のTaskを強制終了し、実行待ちQueueを空にする.
  void KillAllTasks();
  
  //interface ITask<T>
  //Taskの種類.
  T TaskType;
  
  //開始処理.
  void OnEnter();
  
  //更新処理. (trueを返すと終了処理が呼ばれ、Taskを終了する)
  bool OnUpdate();
  
  //終了処理.
  void OnExit();
  ```
<a id="BehaviourTree"></a>
### BehaviourTree
- 概要<br>
  ビヘイビアベースAIを制御するための仕組みを提供します.<br>
  Behaviour TreeとはゲームAIを制御する上でしばしば使われる仕組みです.<br>
  Unreal Engine 4には標準で搭載されています.<br>
  ここでは仕組みの詳細は割愛します.<br>
  
  - BehaviourTreeSystem<br>
    BehaviourTreeの実行機能を提供します.<br>
    
  - TaskNode<br>
    基本となるnodeです.
    
  - SelectorNode<br>
    sub nodeを登録し,いずれかが成功するまで実行します.<br>
    
  - SequenceNode<br>
    sub nodeを登録し,いずれかが失敗するまで実行します.<br>
  
  - DecoratorNode<br>
    登録した条件が満たされる場合,sub nodeを実行します.<br>
  
- 使い方<br>
  例) お金を持っていたら在庫に応じてジュースか水を買い、お金がなければ銀行に行くAI.<br>
  ```c#
  using namespace RitsGameSeminar.AI.BehaviourTree;
  
  public class BTController : MonoBehaviour {
      private BehaviourTreeSystem m_btSystem;
        
      private void Start() {
          //BehaviourTreeのインスタンスを作成.
          m_btSystem = new BehaviourTreeSystem();
      
          //店に行くTask.
          var goToShopTask = new TaskNode(m_btSystem, GoToShop);        
          //銀行に行くTask.
          var goToBankTask = new TaskNode(m_btSystem, GoToBank);
          //家に帰るTask.
          var goHomeTask = new TaskNode(m_btSystem, GoHome);
          //ジュースを買うTask.
          var buyJuiceTask = new TaskNode(m_btSystem, BuyJuice);
          //水を買うTask.
          var buyWaterTask = new TaskNode(m_btSystem, BuyWater);
          
          //nodeを設定.
          var rootNode = new SequenceNode(m_btSystem, new Node[] {
              new SelectorNode(m_btSysyem, new Node[] {
                  new DecoratorNode(m_btSystem, goToShopTask, HasMoney),
                  new SequenceNode(m_btSystem, new Node[] {
                      goToBankTask,
                      goToShopTask,
                  }),
              }),
              new SequenceNode(m_btSystem, new Node[] {
                  new SelectorNode(m_btSystem, new Node[] {
                      buyJuiceTask,
                      buyWaterTask,
                  }),
                  goHomeTask,
              }),
          });
          
          //root nodeを設定.
          m_btSystem.SetRoot(rootNode);
      }
      
      private void Update() {
          //Systemの更新.
          m_btSystem.Execute();
      }
      
      private ENodeStatus GoToShop() {
          //実装は省略.
          //成功したらENodeStatus.Success,
          //失敗したらENodeStatus.Failure,
          //実行中ならENodeStatus.Running,
          //を返すようにします.
      }
      
      //以下同様に実装します.
      private ENodeStatus GoToBank() { }
      private ENodeStatus GoHome() { }
      private ENodeStatus BuyJuice() { }
      private ENodeStatus BuyWater() { }
      
      //お金を持っているか判定するメソッド.
      private bool HasMoney() { }
  }
  ```

- APIリファレンス<br>
  ```c#
  //class BehaviourTreeSystem
  //BehaviourTreeの実行.
  void Execute();
  
  //root nodeを設定する.
  void SetRoot(Node rootNode);
  
  
  /// <summary>
  /// TaskNodeのコンストラクタ.
  /// </summary>
  /// <param name="btSystem">System</param>
  public class TaskNode(BehaviourTreeSystem btSystem);
  
  /// <summary>
  /// SelectorNodeのコンストラクタ.
  /// </summary>
  /// <param name="btSystem">System</param>
  /// <param name="nodes">sub nodeの配列</param>
  public class SelectorNode(BehaviourTreeSystem btSystem, Node[] nodes);
  
  /// <summary>
  /// SequenceNodeのコンストラクタ.
  /// </summary>
  /// <param name="btSystem">System</param>
  /// <param name="nodes">sub nodeの配列</param>
  public class SequenceNode(BehaviourTreeSystem btSystem, Node[] nodes);
  
  /// <summary>
  /// DecoratorNodeのコンストラクタ.
  /// </summary>
  /// <param name="btSystem">System</param>
  /// <param name="node">sub node</param>
  /// <param name="judgeMethod">sub nodeを実行するか判定するメソッド</param>
  public class DecoratorNode(BehaviourTreeSystem btSystem, Node node, Func<bool> judgeMethod);
  ```

<a id="CoroutineManager"></a>
### CoroutineManager
- 概要<br>
  外部クラスからCoroutine処理を委譲し、非MonoBehaviourクラスでCoroutineを使える仕組みを提供します.<br>
  
- 使い方<br>
  ```c#
  using namespace RitsGameSeminar;
  
  //委譲元クラス
  public class SampleClass {
      public SampleClass(){
          //Coroutineの実行.
          CoroutineManager.Instance.StartDelegatedCoroutine(SampleCoroutine());
          
          //Coroutineの停止.
          CoroutineManager.Instance.StopDelegatedCoroutine(SampleCoroutine());
      }
      
      //実装は省略
      private IEnumerator SampleCoroutine() {}
  }
  ```
- APIリファレンス
  ```c#
  //class CoroutineManager
  //Coroutineの開始.
  Coroutine StartDelegatedCoroutine(IEnumerator coroutine);
  
  //Coroutineの停止.
  void StopDelegatedCoroutine(IEnumerator coroutine);
  ```

<a id="ServiceLocator"></a>
### ServiceLocator
- 概要<br>
  具象クラスに依存しないよう、抽象クラスの型とインスタンスを紐づける機能を提供します.
  
- 使い方<br>
  ```c#
  using namespace RitsGameSeminar;
  
  //例) 入力システムを抽象化する.
  //入力のインターフェース
  public interface IInputProvider {
      float HorizontalInput { get; }
      float VerticalInput { get; }
  }
  
  //キーボード入力の具象クラス.
  public class KeyboardInputProvider : MonoBehaviour, IInputProvider {
      float HorizontalInput { get; }
      float VerticalInput { get; }
      
      private void Update() {
          //実装は省略.
      }
  }
  
  //ServiceLocatorに登録する.
  public class ReferenceInjector : MonoBehaviour {
      [SerializeField] private KeyboardInputProvider m_inputProvider;
      
      private void Start() {
          ServiceLocator.Register<IInputProvider>(m_inputProvider);
      }
  }
  
  public class PlayerMover : MonoBehaviour {
      private IInputProvider m_inputProvider;
      
      private void Start(){
          //ServiceLocatorを使わない場合
          //結局参照を注入するところで具象クラスに依存してしまっていた.
          m_inputProvider = FindObjectOfType<KeyboardInputProvider>();
          
          //ServiceLocatorを使う場合
          //具象クラスに依存せず参照を注入できる.
          m_inputProvider = ServiceLocator.Resolve<IInputProvider>();
      }
  } 
  ```
- APIリファレンス
  ```c#
  //class ServiceLocator
  //インスタンスを登録.
  public static void Register<T>(T instance);
  
  //インスタンスを取得するメソッド.
  publicc static T Resolve<T>();
  ```

<a id="ResourceProvider"></a>
### ResourceProvider
- 概要<br>
  様々な動的リソース管理をまとめる仕組みを提供します.<br>
  ResourcesやStreamingAssetsをラップしたようなものです.<br>
  ([ServiceLocator](#ServiceLocator), [StreamingAssets](#StreamingAssets), [AssetBundle](#AssetBundle)と併用することを推奨します)<br>
  本ライブラリではUnity Editor上で使う用とWindows用を用意しています.<br>
  対応プラットフォームを増やす際は、適宜IResourceProviderを実装してください.<br>

- 準備<br>
  1. EResourceID.csを以下のように定義します.
     ```c#
     //EResourceID.cs
     public enum EResourceID {
         None,  //必ず記述してください.
         PlayerStatus,
     }
     ```
  2. ServiceLocatorにResourceProviderの参照を注入する.
     ```c#
     using namespace RitsGameSeminar;
     
     public static class ReferenceInjector {
         [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
         private static void Inject() {
             //例) プラットフォームによって切り替える.
         #if UNITY_EDITOR
             //Unity Editor用
             ServiceLocator.Register<IResourceProvider>(new DebugResourceProvider());
         #elif UNITY_STANDALONE_WIN
             //Window用
             ServiceLocator.Register<IResourceProvider>(new WindowsResourceProvider());
         #endif
         }
     }
     ```
  3. ResourceProviderにリソースを登録する.
     ```c#
     //PlayerStatus.cs
     //登録するasset
     public class PlayerStatus : ScriptableObject {
         public float MoveSpeed;
     }
     
     //ReferenceInjector.cs
     using namespace RitsGameSeminar;
     
     public static class ReferenceInjector {
         [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
         private static void Inject() {
             //一部省略
             
             //ResourceProviderにリソースを登録する.
             //ここではPlayerStatus型のScriptableObjectをEResourceID.PlayerStatusとして登録しています.
             //Unity Editor用ではResourcesフォルダに"PlayerStatus" assetを配置してください.
             //Windows用ではStreamingAssets/Windows/にAssetBundle化した"playerstatus" assetを配置してください.
             ServiceLocator.Resolve<IResourceProvider>().RegisterResource<PlayerStatus>(EResourceID.PlayerStatus, "PlayerStatus");
         }
     }
     ```

- 使い方<br>
  ```c#
  using namespace RitsGameSeminar;
  
  public class PlayerMover : MonoBehaviour {
      private float m_moveSpeed;
      
      private void Start() {
          //ServiceLocatorからプラットフォームに対応したResourceProviderを取得し、
          //登録されたリソースを読み込む.
          m_moveSpeed = ServiceLocator.Resolve<IResourceProvider>()
                                      .LoadResource<PlayerStatus>(EResourceID.PlayerStatus).MoveSpeed;
      }
  }
  ```
