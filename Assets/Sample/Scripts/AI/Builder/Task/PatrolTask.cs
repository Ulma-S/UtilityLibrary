using RitsGameSeminar.AI.BehaviourTree;
using UnityEngine;

namespace RitsGameSeminar.Sample {
    public class PatrolTask : MonoBehaviour, IActionable {
        private float m_elapsedTime = 0f;
        
        public ENodeStatus DoAction() {
            if (m_elapsedTime >= 3f) {
                m_elapsedTime = 0f;
                return ENodeStatus.Success;
            }
            Debug.Log("巡回");
            m_elapsedTime += Time.deltaTime;
            return ENodeStatus.Running;
        }
    }
}