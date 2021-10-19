using UnityEngine;

namespace RitsGameSeminar.Sample {
    public class KeyboardInputProvider : MonoBehaviour, IInputProvider {
        public float HorizontalInput { get; private set; }
        public float VerticalInput { get; private set; }
        public bool IsJumpButtonDown { get; private set; }
        public Vector2 CursorPosition { get; private set; }
        public bool IsCursorDown { get; private set; }

        private void Update() {
            HorizontalInput = Input.GetAxisRaw("Horizontal");
            VerticalInput = Input.GetAxisRaw("Vertical");
            IsJumpButtonDown = Input.GetKeyDown(KeyCode.Space);
            CursorPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            IsCursorDown = Input.GetMouseButton(0);
        }
    }
}