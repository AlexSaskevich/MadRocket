using Assets.Source.Rocket;
using Assets.Source.UI;
using UnityEngine;

namespace Assets.Source.GameLogic
{
    [RequireComponent(typeof(RocketMovement))]
    public sealed class CollisionHandler : MonoBehaviour
    {
        [SerializeField] private RocketFollower _rocketFollower;
        [SerializeField] private GameOverScreen _gameOverScreen;

        private RocketMovement _rocketMovement;

        private void Awake()
        {
            _rocketMovement = GetComponent<RocketMovement>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            _rocketMovement.Rigidbody.velocity = Vector3.zero;
            _rocketMovement.enabled = false;
            _rocketFollower.enabled = false;
            _gameOverScreen.Show();
        }
    }
}