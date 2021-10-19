using UnityEngine;

namespace RitsGameSeminar.Sample {
    public class ResourceRegisterer : MonoBehaviour {
        [SerializeField] private KeyboardInputProvider m_inputProvider;
        
        private void Awake() {
            ServiceLocator.Register<IInputProvider>(m_inputProvider);
        }
    }
}
