namespace RitsGameSeminar.Sample {
    public interface IInputProvider {
        public float HorizontalInput { get; }
        public float VerticalInput { get; }
        public bool IsJumpButtonDown { get; }
        public bool IsShoot { get; }
        public bool IsShootDown { get; }
    }
}