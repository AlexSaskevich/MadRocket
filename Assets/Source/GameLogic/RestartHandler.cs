using Assets.Source.Interfaces;
using Assets.Source.UI;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Source.GameLogic
{
    public sealed class RestartHandler : MonoBehaviour
    {
        [SerializeField] private GameOverScreen _gameOverScreen;
        [SerializeField] private List<MonoBehaviour> _resettableBehaviours;

        private readonly List<IResettable> _resettableObjects = new();

        private void OnValidate()
        {
            for (int i = 0; i < _resettableBehaviours.Count; i++)
            {
                if (_resettableBehaviours[i] is IResettable == false)
                {
                    Debug.LogError(_resettableBehaviours[i].name + " needs to implement " + nameof(IResettable));
                    _resettableBehaviours[i] = null;
                }
            }
        }

        private void Awake()
        {
            for (int i = 0; i < _resettableBehaviours.Count; i++)
                _resettableObjects.Add((IResettable)_resettableBehaviours[i]);
        }

        private void OnEnable()
        {
            _gameOverScreen.Clicked += OnGameOverScreenClicked;
        }

        private void OnDisable()
        {
            _gameOverScreen.Clicked -= OnGameOverScreenClicked;
        }

        private void OnGameOverScreenClicked()
        {
            foreach (var resettableObject in _resettableObjects)
                resettableObject.Reset();
        }
    }
}