using RitsGameSeminar.AI.BehaviourTree;
using UnityEngine;

namespace RitsGameSeminar.Sample {
    public class AICharacterController : MonoBehaviour {
        private BehaviourTreeSystem m_btSystem;
        
        private void Start() {
            m_btSystem = new BehaviourTreeSystem();

            FindObjectOfType<AIBehaviourBuilder>().BtSystem = m_btSystem;
            m_btSystem.SetRoot(null);
        }

        private void Update() {
            m_btSystem.Execute();
        }
    }
}