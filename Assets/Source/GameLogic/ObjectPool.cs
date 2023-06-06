using UnityEngine;

namespace Assets.Source.GameLogic
{
    public class ObjectPool : MonoBehaviour
    {
        [SerializeField] private int _capacity;
        [SerializeField] private Transform _container;

        private GameObject[] _pool;
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
            _pool = new GameObject[_capacity];
        }

        protected void Init(GameObject prefab, float step, float minPositionX, float maxPositionX)
        {
            for (int i = 0; i < _capacity; i++)
            {
                Vector3 spawnPosition = new(0, i * Constants.SpawnPositionY, 0);
                GameObject spawnedGameObject = Instantiate(prefab, spawnPosition, prefab.transform.rotation);
                spawnedGameObject.transform.SetParent(_container);
                _pool[i] = spawnedGameObject;
            }

            SetChildrenPositionX(step, minPositionX, maxPositionX);
        }

        protected void ResetPool(float step, float minPositionX, float maxPositionX)
        {
            for (int i = 0; i < _pool.Length; i++)
            {
                Vector3 spawnPosition = new(0, i * Constants.SpawnPositionY, 0);
                _pool[i].transform.position = spawnPosition;
            }

            SetChildrenPositionX(step, minPositionX, maxPositionX);
        }

        protected void SetObjectAbroadScreen(float step, float minPositionX, float maxPositionX)
        {
            var disablePoint = _camera.ViewportToWorldPoint(new Vector2(Constants.HalfScreenWidth, 0));

            for (int i = 0; i < _pool.Length; i++)
            {
                if (_pool[i].transform.position.y <= disablePoint.y)
                {
                    Vector3 objectAbroadScreenPosition = _pool[i].transform.position;
                    Vector3 lastObjectPosition = _pool[^1].transform.position;
                    _pool[i].transform.position = new Vector3(objectAbroadScreenPosition.x, lastObjectPosition.y + Constants.SpawnPositionY, 0);
                    SetPositionX(step, minPositionX, maxPositionX, i);
                    (_pool[^1], _pool[i]) = (_pool[i], _pool[^1]);
                }
            }
        }

        private void SetPositionX(float step, float minPositionX, float maxPositionX, int index)
        {
            for (int j = 0; j < _pool[index].transform.childCount; j++)
            {
                Transform child = _pool[index].transform.GetChild(j);
                float positionX = Random.Range(minPositionX, maxPositionX);

                if (j == 0)
                    positionX = index % step == 0 ? positionX : 0;
                else
                    positionX = index % step - 1 == 0 ? -positionX : 0;

                child.position = new(positionX, child.position.y, child.position.z);
            }
        }

        private void SetChildrenPositionX(float step, float minPositionX, float maxPositionX)
        {
            for (int i = 0; i < _pool.Length; i++)
                SetPositionX(step, minPositionX, maxPositionX, i);
        }
    }
}