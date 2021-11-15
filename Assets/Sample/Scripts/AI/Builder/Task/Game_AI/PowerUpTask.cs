using RitsGameSeminar.AI.BehaviourTree;
using UnityEngine;

namespace RitsGameSeminar.Sample {
    public class PowerUpTask : MonoBehaviour, IActionable {
        private float m_duration = 2f;
        private float m_elapsedTime = 0f;
        
        public ENodeStatus DoAction() {
            m_elapsedTime += Time.deltaTime;
            if (m_elapsedTime >= m_duration) {
                m_elapsedTime = 0f;
                return ENodeStatus.Success;
            }
            return ENodeStatus.Ready;
        }
    }
}