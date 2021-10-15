namespace RitsGameSeminar {
    public abstract class Singleton<T> where T : class, new() {
        private static T s_instance;

        public static T Instance {
            get {
                if (s_instance == null) {
                    s_instance = new T();
                }
                return s_instance;
            }
        }

        protected Singleton() {
        }
    }
}