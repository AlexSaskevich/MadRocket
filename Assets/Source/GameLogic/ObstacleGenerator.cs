using Assets.Source.Interfaces;
using UnityEngine;

namespace Assets.Source.GameLogic
{
    public sealed class ObstacleGenerator : ObjectPool, IResettable
    {
        [SerializeField] private GameObject _obstacle;
        [SerializeField] private float _step;
        [SerializeField] private float _timeBetweenSpawn;
        [SerializeField] private float _minSpawnPositionX;
        [SerializeField] private float _maxSpawnPositionX;

        private void Start()
        {
            Init(_obstacle, _step, _minSpawnPositionX, _maxSpawnPositionX);
        }

        private void Update()
        {
            SetObjectAbroadScreen(_step, _minSpawnPositionX, _maxSpawnPositionX);
        }

        public void Reset()
        {
            ResetPool(_step, _minSpawnPositionX, _maxSpawnPositionX);
        }
    }
}