using Code.DataClasses;
using UnityEngine;

namespace Code.Core {

    [DefaultExecutionOrder(1)]
    public class SoundSystem : Singleton<SoundSystem> {

        public static SoundSystem _instance;

        [SerializeField] private SoundSettings _settings;

        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;

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

        public void PlayMusic(MusicType musicToPlay) {

            var musicClip = musicToPlay == MusicType.Menu ? _settings.menuMusic : musicToPlay == MusicType.Game ? _settings.gameMusic : null;

            if (musicClip == null) {
                return;
            }

            musicSource.clip = musicClip;
            musicSource.PlayOneShot(musicClip);
        }

        public void PlaySound(SoundEffectType soundToPlay) { 
            
            foreach(var sfx in _settings.effects) { 
                
                if (sfx.effect != soundToPlay) {
                    continue;
                }

                sfxSource.PlayOneShot(sfx.variations[Random.Range(0, sfx.variations.Length)]);
            }
        }

        [ContextMenu("Play Random")]
        public void PlayRandomSound() {

            var randomizedSound = Random.Range(0, _settings.effects.Count);
            PlaySound(_settings.effects[randomizedSound].effect);
        }
    }
}