using Code.DataClasses;
using UnityEngine;

namespace Code.Core {

    public class CharacterController : MonoBehaviour {

        [SerializeField] private KeyCode _leftInput;
        [SerializeField] private KeyCode _rightInput;

        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private Transform _body;

        private CharacterData _stats;

        private bool _isAlive;
        private bool _isGrounded;
        private bool _hasPower;
        private float _powerTimer;
        private LayerMask _whatIsGround;

        private void Awake() {

            _rb ??= GetComponent<Rigidbody2D>();
        }

        public void Init(LayerMask whatIsGround, CharacterData settings) {

            _stats = settings;

            _isAlive = true;
            _whatIsGround = whatIsGround;
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
            _rb.angularVelocity = 0;
        }

        private void CheckGrounded() {

            var hits = Physics2D.RaycastAll(transform.position, -transform.up, 1f, _whatIsGround);

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

            if (collision.gameObject.layer == 8)
            {
                Destroy(collision.gameObject);
                Debug.Log("powerup picked");
                PickUpPower();
            }
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