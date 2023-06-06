using Assets.Source.GameLogic;
using Assets.Source.Interfaces;
using UnityEngine;

namespace Assets.Source.Rocket
{
    public sealed class RocketFollower : MonoBehaviour, IResettable
    {
        [SerializeField] private RocketMovement _rocketMovement;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private float _speed;

        private Vector3 _startPosition;
        private Camera _camera;

        public void Start()
        {
            _camera = Camera.main;
            _startPosition = transform.position;
        }

        private void FixedUpdate()
        {
            Vector3 desiredPosition = _rocketMovement.transform.position + _offset;

            Move(desiredPosition.y);
        }

        public void Reset()
        {
            enabled = true;
            transform.position = _startPosition;
        }

        private void Move(float positionY)
        {
            Vector3 targetPosition = new(_offset.x, positionY, _offset.z);
            var disablePoint = _camera.ViewportToWorldPoint(new Vector2(Constants.HalfScreenWidth, 0));

            if (targetPosition.y <= disablePoint.y)
                return;

            Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, _speed * Time.deltaTime);

            transform.position = smoothedPosition;
        }
    }
}