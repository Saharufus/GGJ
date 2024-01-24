using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Core {

    [DefaultExecutionOrder(2)]
    public class GameplayController : Singleton<GameplayController> {

        public static GameplayController _instance;

        [SerializeField] private GameplaySettings _settings;
        [SerializeField] private List<CharacterController> _characters;

        public void ActivatePowerUp(CharacterController characterController) {
            
            // todo: activate the powerup for a duration on the character and then deactivate
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
                character.Init(_settings.whatIsGround, _settings.characterSettings);
            }
        }
    }
}