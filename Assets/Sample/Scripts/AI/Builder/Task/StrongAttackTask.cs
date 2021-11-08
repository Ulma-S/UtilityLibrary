using RitsGameSeminar.AI.BehaviourTree;
using UnityEngine;

namespace RitsGameSeminar.Sample {
    public class StrongAttackTask : MonoBehaviour, IActionable {
        private float m_elapsedTime = 0f;
        
        public ENodeStatus DoAction() {
            if (m_elapsedTime >= 1f) {
                m_elapsedTime = 0f;
                return ENodeStatus.Success;
            }
            Debug.Log("強攻撃！");
            m_elapsedTime += Time.deltaTime;
            return ENodeStatus.Running;
        }
    }
}