namespace RitsGameSeminar.Sample {
    public interface IInputProvider {
        public float HorizontalInput { get; }
        public float VerticalInput { get; }
        public bool IsJumpButtonDown { get; }
        public bool IsShootButton { get; }
        public bool IsShootButtonDown { get; }
        public bool IsEscapeButtonDown { get; }
        public bool IsReturnButtonDown { get; }
        public bool IsSettingButtonDown { get; }
    }
}