using UnityEngine;

namespace RitsGameSeminar.Sample {
    public class CanvasController : MonoBehaviour {
        private GameObject[] m_uiComponents;
        private IInputProvider m_inputProvider;
        private bool m_isOpen = false;
        
        private void Start() {
            var childTransforms = GetComponentsInChildren<RectTransform>();
            m_uiComponents = new GameObject[childTransforms.Length];
            for (int i = 0; i < m_uiComponents.Length; i++) {
                if (childTransforms[i].gameObject == gameObject) {
                    continue;
                }
                m_uiComponents[i] = childTransforms[i].gameObject;
            }

            m_inputProvider = ServiceLocator.Resolve<IInputProvider>();
        }
        

        private void Update() {
            if (m_inputProvider.IsSettingButtonDown) {
                if (m_isOpen) {
                    Close();
                }
                else {
                    Open();
                }
            }
        }
        

        public void Open() {
            foreach (var component in m_uiComponents) {
                if (component == null) {
                    continue;
                }
                component.SetActive(true);
            }
            m_isOpen = true;
        }
        

        public void Close() {
            foreach (var component in m_uiComponents) {
                if (component == null) {
                    continue;
                }
                component.SetActive(false);
            }
            m_isOpen = false;
        }
    }
}
