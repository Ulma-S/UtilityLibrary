using UnityEngine;

namespace RitsGameSeminar.Sample {
    public class PlayerCameraController : MonoBehaviour {
        [SerializeField] private float m_rotSpeed = 100f;
        private IInputProvider m_inputProvider;
        private Vector2 m_prevCursorPosition;

        private void Start() {
            m_inputProvider = ServiceLocator.Resolve<IInputProvider>();
        }

        private void Update() {
            if (m_inputProvider.IsCursorDown) {
                var rotation = transform.rotation.eulerAngles;
                var dis = m_inputProvider.CursorPosition - m_prevCursorPosition;
                rotation.y += dis.x * m_rotSpeed;
                transform.rotation = Quaternion.Euler(rotation);
            }
            m_prevCursorPosition = m_inputProvider.CursorPosition;
        }
    }
}