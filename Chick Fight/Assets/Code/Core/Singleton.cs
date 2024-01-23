using UnityEngine;

namespace Code.Core {
    [DefaultExecutionOrder(1)]
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
        private static T _instance;
        public static T Instance {
            get {
                _instance ??= FindObjectOfType<T>();
                if (_instance == null) {
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<T>();
                    singletonObject.name = typeof(T).ToString() + " (Singleton)";
                    DontDestroyOnLoad(singletonObject);
                }
                return _instance;
            }
        }
    }
}