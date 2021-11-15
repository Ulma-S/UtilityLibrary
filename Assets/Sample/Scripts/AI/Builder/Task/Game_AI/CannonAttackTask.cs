using RitsGameSeminar.AI.BehaviourTree;
using UnityEngine;

namespace RitsGameSeminar.Sample {
    public class CannonAttackTask : MonoBehaviour, IActionable {
        private EnemyCannonController m_cannonController;
        private bool m_isStarted = false;
        private bool m_isFinished = false;

        private void Start() {
            m_cannonController = FindObjectOfType<EnemyCannonController>();
        }

        public ENodeStatus DoAction() {
            if (!m_isStarted) {
                m_cannonController.PrepareBarrel(() => {
                    m_isFinished = true;
                });
                m_isStarted = true;
                m_isFinished = false;
            }

            if (m_isFinished) {
                return ENodeStatus.Success;
            }

            return ENodeStatus.Ready;
        }
    }
}
