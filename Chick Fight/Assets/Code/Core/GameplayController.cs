using System.Collections.Generic;
using UnityEngine;

namespace Code.Core {

    [DefaultExecutionOrder(2)]
    public class GameplayController : Singleton<GameplayController> {

        public static GameplayController _instance;

        [Header("Settings")]
        [SerializeField] private GameplaySettings _settings;

        [Header("UI Screens")]
        [SerializeField] private Transform _menu;
        [SerializeField] private Transform _startGame;
        [SerializeField] private Transform _endGame;

        [Header("Game")]
        [SerializeField] private List<CharacterController> _characters;
        [SerializeField] private Transform[] _spawnZones;
        [SerializeField] private Transform[] _platforms;
        [SerializeField] private Transform _powerUpTransform;
        [SerializeField] private PowerUp _powerUp;

        private List<CharacterController> _aliveCharacters = new();
        private float powerupSpawnTimer = 0;

        public void SpawnPowerUp() {

            Transform platformToSpawnOn = _platforms[Random.Range(0, _platforms.Length)];
            SpriteRenderer platformSprite = platformToSpawnOn.GetComponent<SpriteRenderer>();

            float platformXScale = (platformToSpawnOn.localScale.x * platformSprite.sprite.bounds.extents.x) - 1;

            float spawnXPos = Random.Range(-platformXScale, platformXScale);
            float spawnYPos = platformToSpawnOn.localScale.y * platformSprite.sprite.bounds.extents.y;
            Vector2 spawnPos = platformToSpawnOn.position + platformToSpawnOn.TransformDirection(new Vector2(spawnXPos, spawnYPos));

            var worm = Instantiate(_powerUp, _powerUpTransform);
            var powerUpEffects = Random.Range(0, _settings.powerUpSettings.powerUps.Count);
            worm.Init(_settings.powerUpSettings.powerUps[powerUpEffects]);
            worm.transform.SetPositionAndRotation(spawnPos, platformToSpawnOn.rotation);
        }

        public void ActivatePowerUp(CharacterController characterController, PowerUp powerUp) {

            Debug.Log($"{characterController} collected powerup "+ powerUp._stats.Count);
            // todo: activate the powerup for a duration on the character and then deactivate

            SoundSystem.Instance.PlaySound(DataClasses.SoundEffectType.PowerUpActivate);

            Destroy(powerUp.gameObject);
        }

        public void CleanPowerUps() {

            var powerUps = _powerUpTransform;
            foreach (Transform powerup in powerUps) {
                DestroyImmediate(powerup.gameObject);
            }

        }

        public void Update() {

            var gameIsRunning = _aliveCharacters.Count > 1;
            if (!gameIsRunning) { return; }

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
            GoToMenu();
        }

        public void GoToMenu() {

            SwitchUIScreen(_menu);
            SoundSystem.Instance.PlayMusic(DataClasses.MusicType.Menu);
        }

        public void StartGame() {

            CleanPowerUps();
            ActivateCharacters();
            SwitchUIScreen(_startGame);
            SoundSystem.Instance.PlayMusic(DataClasses.MusicType.Game);
        }

        private void SwitchUIScreen(Transform screenToDisplay) {

            _menu.gameObject.SetActive(_menu == screenToDisplay);
            _startGame.gameObject.SetActive(_startGame == screenToDisplay);
            _endGame.gameObject.SetActive(_endGame == screenToDisplay);
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

        private void ActivateCharacters() {

            if (_characters == null || _characters.Count == 0) {
                return;
            }

            _aliveCharacters = new List<CharacterController>();
            var randomPos = new List<Transform>(_spawnZones);

            foreach (CharacterController character in _characters) {

                if (character == null) {
                    Debug.Log($"GameplayController has missing character ref {character.gameObject.name}");
                    continue;
                }
                
                character.Activate(true);
                _aliveCharacters.Add(character);

                var randomSpawn = Random.Range(0, randomPos.Count);
                character.SetPosition(randomPos[randomSpawn].position);
                randomPos.RemoveAt(randomSpawn);
            }
        }

        public void PlayerOffScreen(CharacterController character) {

            _aliveCharacters.Remove(character);
            character.Activate(false);
            SoundSystem.Instance.PlaySound(DataClasses.SoundEffectType.FallOffScreen);

            CheckGameFinished();
        }

        private void CheckGameFinished() {
            
            if (_aliveCharacters.Count < 2) {
                SwitchUIScreen(_endGame);
            }
        }
    }
}