using RitsGameSeminar.Extension;
using RitsGameSeminar.StateMachine;
using UnityEngine;

namespace RitsGameSeminar.Sample {
    /// <summary>
    /// Stateの種類のenum.
    /// (1行目は空のtypeを設定すること)
    /// </summary>
    public enum EPlayerState {
        None,
        Idle,
        Run,
        Jump,
    }
    
    /// <summary>
    /// PlayerのStateを管理するクラス.
    /// </summary>
    public class PlayerStateController : MonoBehaviour {
        private StateMachine<EPlayerState> m_stateMachine;

        private void Start() {
            m_stateMachine = new StateMachine<EPlayerState>(gameObject);
            
            //stateの登録.
            m_stateMachine.RegisterState(new PlayerIdleState(m_stateMachine));
            m_stateMachine.RegisterState(new PlayerRunState(m_stateMachine));
            m_stateMachine.RegisterState(new PlayerJumpState(m_stateMachine));

            //最初のstateの設定.
            m_stateMachine.ChangeState(EPlayerState.Idle);
        }

        private void Update() {
            m_stateMachine.Update();
            Debug.Log(m_stateMachine.CurrentStateType);
        }
    }

    //--------------------------------------------------------//
    // 以下はサンプル用に1スクリプトにまとめていますが、
    // 実際に書くときはクラスごとに別のスクリプトにすることを推奨します.
    //--------------------------------------------------------//
    
    /// <summary>
    /// PlayerのStateの基底クラス.
    /// </summary>
    public abstract class PlayerStateBase : StateBase<EPlayerState> {
        protected readonly Rigidbody rb;
        protected readonly PlayerCollisionDetector collisionDetector;
        protected readonly PlayerStatus playerStatus;

        protected PlayerStateBase(StateMachine<EPlayerState> stateMachine) : base(stateMachine) {
            rb = stateMachine.owner.GetComponent<Rigidbody>();
            collisionDetector = stateMachine.owner.GetComponent<PlayerCollisionDetector>();
            playerStatus = ServiceLocator.Resolve<IResourceProvider>()
                .LoadResource<PlayerStatus>(EResourceID.PlayerStatus);
        }
    }

    /// <summary>
    /// Playerの走る状態を管理するクラス.
    /// </summary>
    public class PlayerRunState : PlayerStateBase {
        public PlayerRunState(StateMachine<EPlayerState> stateMachine) : base(stateMachine) { }

        public override EPlayerState StateType { get; protected set; } = EPlayerState.Run;

        public override void OnEnter() { }

        public override void OnUpdate() {
            rb.SetVelocityX(Input.GetAxisRaw("Horizontal") * playerStatus.MoveSpeed);
            
            //左右入力がなければIdleStateに遷移する.
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) < 0.01f) {
                stateMachine.ChangeState(EPlayerState.Idle);
            }

            //ジャンプボタンが押されたらJumpStateに遷移する.
            if (Input.GetKeyDown(KeyCode.Space)) {
                stateMachine.ChangeState(EPlayerState.Jump);
            }
        }

        public override void OnExit() { }
    }
    
    /// <summary>
    /// Playerの待機状態を管理するクラス.
    /// </summary>
    public class PlayerIdleState : PlayerStateBase {
        public PlayerIdleState(StateMachine<EPlayerState> stateMachine) : base(stateMachine) { }

        public override EPlayerState StateType { get; protected set; } = EPlayerState.Idle;

        public override void OnEnter() {
            rb.SetVelocityX(0f);
            rb.SetVelocityZ(0f);
        }

        public override void OnUpdate() {
            //左右入力があればRunStateに遷移する.
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0f) {
                stateMachine.ChangeState(EPlayerState.Run);
            }
            
            //ジャンプボタンが押されたらJumpStateに遷移する.
            if (Input.GetKeyDown(KeyCode.Space)) {
                stateMachine.ChangeState(EPlayerState.Jump);
            }
        }

        public override void OnExit() { }
    }

    /// <summary>
    /// Playerのジャンプ状態を管理するクラス.
    /// </summary>
    public class PlayerJumpState : PlayerStateBase {
        public override EPlayerState StateType { get; protected set; } = EPlayerState.Jump;
        
        public PlayerJumpState(StateMachine<EPlayerState> stateMachine) : base(stateMachine) { }

        private bool m_prevIsHitGround = false;

        public override void OnEnter() {
            rb.AddForce(Vector3.up * playerStatus.JumpForce);
            m_prevIsHitGround = collisionDetector.IsHitGround;
        }

        public override void OnUpdate() {
            //着地したらIdleStateに遷移する.
            if (collisionDetector.IsHitGround && !m_prevIsHitGround) {
                stateMachine.ChangeState(EPlayerState.Idle);
            }

            m_prevIsHitGround = collisionDetector.IsHitGround;
        }

        public override void OnExit() { }
    }
}