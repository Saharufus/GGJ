using Code.DataClasses;
using UnityEngine;

namespace Code.Core {

    [DefaultExecutionOrder(2)]
    [RequireComponent(typeof(AudioSource))]
    public class SoundSystem : Singleton<SoundSystem> {

        public static SoundSystem _instance;

        [SerializeField] private SoundSettings _settings;
        
        private AudioSource audioSource;

        private void Awake() {

            if (_instance != null && _instance != this) {
                Destroy(this.gameObject);

            } else {
                _instance = this;
            }

            audioSource ??= gameObject.AddComponent<AudioSource>();

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

        public void PlaySound(SoundEffectType soundToPlay) { 
            
            foreach(var sfx in _settings.effects) { 
                
                if (sfx.effect != soundToPlay) {
                    continue;
                }

                audioSource.PlayOneShot(sfx.variations[Random.Range(0, sfx.variations.Length)]);
            }
        }

        [ContextMenu("Play Random")]
        public void PlayRandomSound(SoundEffectType soundToPlay) {

            var randomizedSound = Random.Range(0, _settings.effects.Count);
            PlaySound(_settings.effects[randomizedSound].effect);
        }
    }
}