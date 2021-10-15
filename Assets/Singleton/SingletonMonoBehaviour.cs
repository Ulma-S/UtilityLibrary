using UnityEngine;

namespace RitsGameSeminar {
    /// <summary>
    /// Component用Singletonクラス.
    /// シーン上でインスタンスが一つであることを保証する.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour {
        private static T instance;

        public static T Instance {
            get {
                if (instance == null) {
                    instance = FindObjectOfType<T>();
                    if (instance == null) {
                        Debug.LogError(typeof(T) + "型のコンポーネントが見つかりません.");
                    }
                }

                return instance;
            }
        }


        protected virtual void Awake() {
            if (this != Instance) {
                Debug.LogError(typeof(T) + "型のコンポーネントが既に存在するため削除しました." +
                               Instance.gameObject.name + "を確認してください.");
                Destroy(this);
            }
        }
    }
}