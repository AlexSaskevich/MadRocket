using Assets.Source.GameLogic;
using Assets.Source.Interfaces;
using UnityEngine;

namespace Assets.Source.Rocket
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class RocketMovement : MonoBehaviour, IResettable
    {
        [SerializeField] private float _movementForce;
        [SerializeField] private float _minFallVelocity;
        [SerializeField] private float _turnForce;
        [SerializeField] private float _rotateSpeed;
        [SerializeField] private float _fallSpeed;
        [SerializeField] private float _riseSpeed;
        [SerializeField] private float _fallAngle;

        private PlayerInput _playerInput;
        private Vector3 _deltaPosition;
        private Vector3 _startPosition;

        public Rigidbody Rigidbody { get; private set; }

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
            _startPosition = transform.position;
            _playerInput = new PlayerInput();
        }

        private void OnEnable()
        {
            _playerInput.Player.Enable();
        }

        private void OnDisable()
        {
            _playerInput.Player.Disable();
        }

        private void FixedUpdate()
        {
            if (_playerInput.Player.Move.IsPressed())
            {
                Move(Vector3.up, _movementForce);
                ClampVelocity(0, _movementForce);
            }

            ClampVelocity(_minFallVelocity, _movementForce);

            if (Input.touchCount > 0)
            {
                _deltaPosition = new();
                Vector3 touchPosition = _playerInput.Player.Turn.ReadValue<Vector2>();

                _deltaPosition += touchPosition.x > GetHalfScreenWidth() ? Vector3.right : Vector3.left;
                _deltaPosition *= _turnForce;

                Move(_deltaPosition, _turnForce);

                Rotate(-_deltaPosition.x, _rotateSpeed);
            }

            if (Rigidbody.velocity.y <= 0)
                Fall(_deltaPosition);
            else if (_playerInput.Player.Move.IsPressed() == false)
                Rotate(0, _riseSpeed);
        }

        public void Reset()
        {
            enabled = true;
            Rigidbody.velocity = Vector3.zero;
            Rigidbody.angularVelocity = Vector3.zero;
            Rigidbody.transform.SetPositionAndRotation(_startPosition, Quaternion.identity);
        }

        private void Move(Vector3 direction, float force)
        {
            Rigidbody.AddForce(force * Time.fixedDeltaTime * direction, ForceMode.VelocityChange);
        }

        private void ClampVelocity(float minVelocity, float maxVelocity)
        {
            var clampedVelocity = Mathf.Clamp(Rigidbody.velocity.y, minVelocity, maxVelocity);
            var finalVelocity = new Vector3(0, clampedVelocity, 0);
            Rigidbody.velocity = finalVelocity;
        }

        private float GetHalfScreenWidth()
        {
            return Screen.width * Constants.HalfScreenWidth;
        }

        private void Rotate(float targetValue, float speed)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, targetValue), Time.fixedDeltaTime * speed);
        }

        private void Fall(Vector3 deltaPosition)
        {
            if (deltaPosition.x < 0)
                Rotate(_fallAngle, _fallSpeed);
            else
                Rotate(-_fallAngle, _fallSpeed);
        }
    }
}