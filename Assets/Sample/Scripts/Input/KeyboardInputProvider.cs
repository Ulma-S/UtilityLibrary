using UnityEngine;

namespace RitsGameSeminar.Sample {
    public class KeyboardInputProvider : MonoBehaviour, IInputProvider {
        public float HorizontalInput { get; private set; }
        public float VerticalInput { get; private set; }
        public bool IsJumpButtonDown { get; private set; }
        public bool IsShoot { get; private set; }
        public bool IsShootDown { get; private set; }

        private void Update() {
            HorizontalInput = Input.GetAxisRaw("Horizontal");
            VerticalInput = Input.GetAxisRaw("Vertical");
            IsJumpButtonDown = Input.GetKeyDown(KeyCode.Space);
            IsShoot = Input.GetKey(KeyCode.J);
            IsShootDown = Input.GetKeyDown(KeyCode.J);
        }
    }
}