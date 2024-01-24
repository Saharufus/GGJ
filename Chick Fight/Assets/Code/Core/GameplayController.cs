using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code.Core {

    [DefaultExecutionOrder(2)]
    public class GameplayController : Singleton<GameplayController> {

        public static GameplayController _instance;

        [SerializeField] private GameplaySettings _settings;
        [SerializeField] private List<CharacterController> _characters;
        [SerializeField] private Transform[] _platforms;
        [SerializeField] private Transform _powerUpTransform;
        [SerializeField] private PowerUp _powerUp;

        private float powerupSpawnTimer = 0;

        public void SpawnPowerUp() {

            Transform platformToSpawnOn = _platforms[Random.Range(0, _platforms.Length)];
            SpriteRenderer platformSprite = platformToSpawnOn.GetComponent<SpriteRenderer>();

            float platformXScale = (platformToSpawnOn.localScale.x * platformSprite.sprite.bounds.extents.x) - 1;

            float spawnXPos = Random.Range(-platformXScale, platformXScale);
            float spawnYPos = platformToSpawnOn.localScale.y * platformSprite.sprite.bounds.extents.y;
            Vector2 spawnPos = platformToSpawnOn.position + platformToSpawnOn.TransformDirection(new Vector2(spawnXPos, spawnYPos));

            var worm = Instantiate(_powerUp, _powerUpTransform.parent);
            var powerUpEffects = Random.Range(0, _settings.powerUpSettings.powerUps.Count);
            worm.Init(_settings.powerUpSettings.powerUps[powerUpEffects]);
            worm.transform.SetPositionAndRotation(spawnPos, platformToSpawnOn.rotation);
        }

        public void ActivatePowerUp(CharacterController characterController, PowerUp powerUp) {

            Debug.Log($"{characterController} collected powerup "+ powerUp._stats.Count);
            // todo: activate the powerup for a duration on the character and then deactivate

            SoundSystem.Instance.PlaySound(DataClasses.SoundEffectType.PowerUpActivate);
        }

        public void Update() {

            powerupSpawnTimer += Time.deltaTime;
            if (powerupSpawnTimer >= _settings.powerupSpawnTime) {
                powerupSpawnTimer = 0;
                SpawnPowerUp();
            }
        }

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

            if (_characters == null) {
                Debug.LogError("Missing ref to Characters");
                return;
            }

            InitCharacters();
            SoundSystem.Instance.PlayMusic(DataClasses.MusicType.Menu);
        }

        private void InitCharacters() {

            if (_characters == null || _characters.Count == 0) {
                return;
            }

            foreach (CharacterController character in _characters) {

                if (character == null) {
                    Debug.Log($"GameplayController has missing character ref {character.gameObject.name}");
                    continue;
                }
                character.Init(_settings);
            }
        }
    }
}