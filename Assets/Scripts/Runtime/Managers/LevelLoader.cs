using System;
using Runtime.Signal;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Runtime.Managers
{
    public class LevelLoader : IDisposable
    {
        private readonly SignalBus _signalBus;
        
        private readonly IInstantiator _instantiator;
        
        private const string LEVEL_PREFAB_PATH = "Levels/Level ";

        private GameObject _currentLevelInstance;
        
        public LevelLoader(SignalBus signalBus, IInstantiator instantiator)
        {
            _signalBus = signalBus;
            _instantiator = instantiator;
            
            SubscribeEvents();
        }
        
        private void SubscribeEvents()
        {
            _signalBus.Subscribe<LoadLevelSignal>(OnLoadLevel);
            _signalBus.Subscribe<DestroyCurrentLevelSignal>(OnDestroyCurrentLevel);
        }

        private void OnLoadLevel(LoadLevelSignal signal) 
        {
            if (_currentLevelInstance != null) 
            {
                Object.Destroy(_currentLevelInstance);
            }
            
            string levelPath = $"{LEVEL_PREFAB_PATH}{signal.LevelIndex}";
            GameObject levelPrefab = Resources.Load<GameObject>(levelPath);

            if (levelPrefab != null) {
                _currentLevelInstance = _instantiator.InstantiatePrefab(levelPrefab);
                Debug.Log($"Level {signal.LevelIndex} loaded successfully.");
            } 
            else 
            {
                Debug.LogError($"Level prefab not found at path: {levelPath}");
            }
        }
        
        private void OnDestroyCurrentLevel()
        {
            if (_currentLevelInstance != null)
            {
                Object.Destroy(_currentLevelInstance);
            }
        }
        
        private void UnsubscribeEvents()
        {
            _signalBus.Unsubscribe<LoadLevelSignal>(OnLoadLevel);
            _signalBus.Unsubscribe<DestroyCurrentLevelSignal>(OnDestroyCurrentLevel);
        }
        
        public void Dispose()
        {
            UnsubscribeEvents();
        }
    }
}