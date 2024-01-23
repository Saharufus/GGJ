using UnityEngine;

namespace Code.Core {

    public class CharacterController : MonoBehaviour {

        [SerializeField] private KeyCode leftInput;
        [SerializeField] private KeyCode rightInput;

        [SerializeField] private Rigidbody2D _rb;

        [SerializeField][Min(1)] private float _rotationSpeed = 100f;
        [SerializeField][Min(1)] private float _jumpForce = 5f;
        [SerializeField] private float _powerDuration = 5f;

        private bool _isAlive;
        private bool _isGrounded;
        private bool _hasPower;
        private float _powerTimer;

        // move this to gameplay controller
        [SerializeField] private LayerMask _whatIsCharacter;
        [SerializeField] private LayerMask _whatIsGround;

        private void Awake() {

            _rb ??= GetComponent<Rigidbody2D>();

            Init();
        }

        private void Init() {

            _isAlive = true;
        }

        private void Update() {

            float rotationInput = GetInputRotation();

            float rotationAmount = rotationInput * _rotationSpeed * Time.deltaTime;

            _rb.MoveRotation(_rb.rotation + rotationAmount);

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

        private float GetInputRotation() {
            return Input.GetKey(leftInput) ? 1 : Input.GetKey(rightInput) ? -1 : 0f;
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

            Vector2 jumpVelocity = Quaternion.Euler(0f, 0f, _rb.rotation) * Vector2.up * _jumpForce;
            _rb.velocity = jumpVelocity;
        }

        private void CheckGrounded() {

            RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, 1f, _whatIsGround);
            _isGrounded = hit.collider != null;
        }

        private void OnTriggerEnter2D(Collider2D collision) {

            // Check if the character collides with the power up item
            // PickUpPower();
        }

        private void PickUpPower() {

            _hasPower = true;
            // todo: get the duration of the powerup from the powerup item or by the gameplay controller
            _powerTimer = _powerDuration;
        }

        private void EndPower() {

            _hasPower = false;
        }

    }
}