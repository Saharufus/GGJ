using UnityEngine;

namespace Code.Core {

    [DefaultExecutionOrder(2)]
    public class GameplayController : Singleton<GameplayController> {

        public LayerMask _whatIsCharacter;
        public LayerMask _whatIsGround;

        public static GameplayController _instance;

        private void Awake() {

            if (_instance != null && _instance != this) {
                Destroy(this.gameObject);

            } else {
                _instance = this;
            }
        }

    }
}