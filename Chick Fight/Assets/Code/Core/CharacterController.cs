using UnityEngine;

namespace Code.Core {

    public class CharacterController : MonoBehaviour {

        [SerializeField] private KeyCode _leftInput;
        [SerializeField] private KeyCode _rightInput;

        [SerializeField] private Rigidbody2D _rb;

        private CharacterSettings _stats;

        private bool _isAlive;
        private bool _isGrounded;
        private bool _hasPower;
        private float _powerTimer;
        private LayerMask _whatIsGround;

        private void Awake() {

            _rb ??= GetComponent<Rigidbody2D>();
        }

        public void Init(LayerMask whatIsGround, CharacterSettings settings) {

            _stats = settings;

            _isAlive = true;
            _whatIsGround = whatIsGround;
        }

        private void Update() {

            float rotationInput = GetInputRotation();

            UpdateRotation(ref rotationInput);

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

        private float GetInputRotation() => Input.GetKey(_leftInput) ? 1 : Input.GetKey(_rightInput) ? -1 : 0f;

        private void UpdateRotation(ref float rotationInput) {

            float rotationAmount = rotationInput * _stats.rotationSpeed * Time.deltaTime;
            float destRotation = _rb.rotation + rotationAmount;
            float maxRotationDegree = _stats.maxRotationDegree;

            if ((destRotation >= -maxRotationDegree && destRotation <= maxRotationDegree) || maxRotationDegree >= 180f) {

                _rb.MoveRotation(_rb.rotation + rotationAmount);
            }
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
            GameplayController.Instance.ActivatePowerUp(this);
        }

        private void EndPower() {

            _hasPower = false;
        }

    }
}