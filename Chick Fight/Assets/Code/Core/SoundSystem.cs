using System.Collections.Generic;
using UnityEngine;

namespace Code.Core {

    [DefaultExecutionOrder(2)]
    public class SoundSystem : Singleton<SoundSystem> {

        public static SoundSystem _instance;

        [SerializeField] private SoundSettings _settings;

        private void Awake() {

            if (_instance != null && _instance != this) {
                Destroy(this.gameObject);

            } else {
                _instance = this;
            }

            Init();
        }

        private void Init() {
            
            if (_settings == null) {
                Debug.LogError("Missing ref to GameplaySettings");
                return;
            }

            Debug.Log($"Loaded Music: Menu - {_settings.menuMusic.name}, Game {_settings.gameMusic.name}");
            Debug.Log($"Loaded Sound Effects {_settings.effects.Count}");
        }
    }
}