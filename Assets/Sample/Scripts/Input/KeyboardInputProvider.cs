using UnityEngine;

namespace RitsGameSeminar.Sample {
    public class KeyboardInputProvider : MonoBehaviour, IInputProvider {
        public float HorizontalInput { get; private set; }
        public float VerticalInput { get; private set; }
        public bool IsJumpButtonDown { get; private set; }
        public bool IsShootButton { get; private set; }
        public bool IsShootButtonDown { get; private set; }
        public bool IsEscapeButtonDown { get; private set; }
        public bool IsReturnButtonDown { get; private set; }

        private void Update() {
            HorizontalInput = Input.GetAxisRaw("Horizontal");
            VerticalInput = Input.GetAxisRaw("Vertical");
            IsJumpButtonDown = Input.GetKeyDown(KeyCode.Space);
            IsShootButton = Input.GetKey(KeyCode.J);
            IsShootButtonDown = Input.GetKeyDown(KeyCode.J);
            IsEscapeButtonDown = Input.GetKeyDown(KeyCode.Escape);
            IsReturnButtonDown = Input.GetKeyDown(KeyCode.Return);
        }
    }
}