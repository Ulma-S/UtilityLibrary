using UnityEngine;

namespace RitsGameSeminar.Sample {
    public interface IInputProvider {
        public float HorizontalInput { get; }
        public float VerticalInput { get; }
        public bool IsJumpButtonDown { get; }
        public Vector2 CursorPosition { get; }
        public bool IsCursorDown { get; }
    }
}