using RitsGameSeminar.AI.BehaviourTree;
using UnityEngine;

namespace RitsGameSeminar.Sample {
    public class ChargeTask : MonoBehaviour, IActionable {
        private float m_elapsedTime = 0f;
        
        public ENodeStatus DoAction() {
            if (m_elapsedTime >= 2f) {
                m_elapsedTime = 0f;
                return ENodeStatus.Success;
            }
            Debug.Log("エネルギーチャージ...");
            m_elapsedTime += Time.deltaTime;
            return ENodeStatus.Running;
        }
    }
}