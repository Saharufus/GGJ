using Code.DataClasses;
using UnityEngine;

namespace Code.Core {

    public class CharacterController : MonoBehaviour {

        [SerializeField] private KeyCode _leftInput;
        [SerializeField] private KeyCode _rightInput;

        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private Transform _body;

        private CharacterData _stats;
        private GameplaySettings _settings;

        private bool _isAlive;
        private bool _isGrounded;
        private bool _hasPower;
        private float _powerTimer;

        private void Awake() {

            _rb ??= GetComponent<Rigidbody2D>();
        }

        public void Init(GameplaySettings settings) {

            _stats = settings.characterSettings;
            _settings = settings;

            _isAlive = true;
        }

        private void Update() {

            if (GetInputRotation(out float rotationInput)) {
                UpdateRotation(ref rotationInput);
            }

            if (_isGrounded) {
                _isGrounded = false;
                Jump();
            }

            UpdatePowerUp();
        }

        private void FixedUpdate() {

            if (_isAlive) {

                CheckGrounded();
            }
        }

        private bool GetInputRotation(out float rotationInput) {

            rotationInput = 0;

            if (_leftInput is KeyCode.None || _rightInput is KeyCode.None) {
                return false;
            }

            if (Input.GetKey(_leftInput)) {
                rotationInput = 1;
            
            } else if (Input.GetKey(_rightInput)) {
                rotationInput = -1;
            }

            return true;
        }

        private void UpdateRotation(ref float rotationInput) {

            float rotationAmount = rotationInput * _stats.rotationSpeed * Time.deltaTime;
            _rb.MoveRotation(_rb.rotation + rotationAmount);
        }

        private void UpdatePowerUp() {

            if (!_hasPower) {
                return;
            }

            _powerTimer -= Time.deltaTime;
            if (_powerTimer <= 0) {
                EndPower();
            }
        }

        private void Jump() {

            Vector2 jumpVelocity = transform.up * _stats.jumpForce;
            _rb.velocity = new Vector2(_rb.velocity.x / (_stats.jumpXFriction + 1), 0) + jumpVelocity;
            _rb.angularVelocity = 0;
        }

        private void CheckGrounded() {

            var hits = Physics2D.RaycastAll(transform.position, -transform.up, 1f, _settings.whatIsGround);

            _isGrounded = false;
            foreach (var hit in hits) {

                if (hit.collider == null || hit.collider.transform == _body) {
                    continue;
                }

                _isGrounded = true;
                return;
            }
            
        }

        private void OnTriggerEnter2D(Collider2D collision) {

            if (collision.gameObject.layer == _settings.whatIsPowerUp && !_hasPower)
            {
                Destroy(collision.gameObject);
                Debug.Log("powerup picked");
                PickUpPower();
            }
        }

        private void PickUpPower() {

            _hasPower = true;
            _powerTimer = _settings.powerUpDurtaionInSeconds;
            GameplayController.Instance.ActivatePowerUp(this);
        }

        private void EndPower() {

            _hasPower = false;
        }

    }
}