using RitsGameSeminar.AI.BehaviourTree;
using UnityEngine;

namespace RitsGameSeminar.Sample {
    public class BTController : MonoBehaviour {
        private BehaviourTreeSystem m_btSystem;
        private int m_currentMoney = 100;
        private int m_currentJuice = 3;
        [SerializeField] private GameObject m_shop;
        [SerializeField] private GameObject m_bank;

        private void Start() {
            m_btSystem = new BehaviourTreeSystem();

            var goToShopTask = new TaskNode(m_btSystem, GoToShop);
            var goToBankTask = new TaskNode(m_btSystem, GoToBank);
            var goToHomeTask = new TaskNode(m_btSystem, GoToHome);
            var buyJuiceTask = new TaskNode(m_btSystem, BuyJuiceTask);
            var buyWaterTask = new TaskNode(m_btSystem, BuyWaterTask);

            var rootNode = new SequenceNode(m_btSystem, new Node[] {
                new SelectorNode(m_btSystem, new Node[] {
                    new DecoratorNode(m_btSystem, goToShopTask, HasMoney),
                    new SequenceNode(m_btSystem, new Node[] {
                        goToBankTask,
                        goToShopTask,
                    })
                }),
                new SequenceNode(m_btSystem, new Node[] {
                    new SelectorNode(m_btSystem, new Node[] {
                        buyJuiceTask,
                        buyWaterTask,
                    }),
                    goToHomeTask,
                }),
            });
            
            //rootを設定.
            m_btSystem.SetRoot(rootNode);
        }

        private void Update() {
            m_btSystem.Execute();
        }

        private bool HasMoney() {
            if (m_currentMoney >= 150) {
                return true;
            }

            return false;
        }

        private ENodeStatus GoToShop() {
            var dir = (m_shop.transform.position - transform.position).normalized;
            transform.Translate(dir * 3 * Time.deltaTime);
            var dis = (m_shop.transform.position - transform.position).magnitude;
            if (dis < 0.5f) {
                return ENodeStatus.Success;
            }
            Debug.Log("店に行く.");
            return ENodeStatus.Running;
        }

        private ENodeStatus GoToBank() {
            var dir = (m_bank.transform.position - transform.position).normalized;
            transform.Translate(dir * 3 * Time.deltaTime);
            var dis = (m_bank.transform.position - transform.position).magnitude;
            if (dis < 0.5f) {
                m_currentMoney += 100;
                return ENodeStatus.Success;
            }
            Debug.Log("銀行に行く.");
            return ENodeStatus.Running;
        }
        
        private ENodeStatus GoToHome() {
            var dir = (Vector3.zero - transform.position).normalized;
            transform.Translate(dir * 3 * Time.deltaTime);
            var dis = (Vector3.zero - transform.position).magnitude;
            if (dis < 0.5f) {
                return ENodeStatus.Success;
            }
            Debug.Log("家に帰る.");
            return ENodeStatus.Running;
        }

        private ENodeStatus BuyJuiceTask() {
            if (m_currentJuice > 0 && m_currentMoney >= 150) {
                m_currentJuice--;
                m_currentMoney -= 150;
                Debug.Log("ジュースを買った！");
                return ENodeStatus.Success;
            }

            Debug.Log("ジュースを買えなかった...");
            return ENodeStatus.Failure;
        }

        private ENodeStatus BuyWaterTask() {
            if (m_currentMoney >= 150) {
                m_currentMoney -= 150;
                Debug.Log("水を買った！");
                return ENodeStatus.Success;
            }

            Debug.Log("水を買えなかった...");
            return ENodeStatus.Failure;
        }
    }
}