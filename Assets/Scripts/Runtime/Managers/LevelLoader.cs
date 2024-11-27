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
        
        private const string LEVEL_PREFAB_PATH = "Levels/Level ";

        private GameObject _currentLevelInstance;
        
        public LevelLoader(SignalBus signalBus)
        {
            _signalBus = signalBus;
            
            SubscribeEvents();
        }
        
        private void SubscribeEvents()
        {
            _signalBus.Subscribe<LoadLevelSignal>(OnLoadLevel);
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
                _currentLevelInstance = Object.Instantiate(levelPrefab);
                Debug.Log($"Level {signal.LevelIndex} loaded successfully.");
            } 
            else 
            {
                Debug.LogError($"Level prefab not found at path: {levelPath}");
            }
        }
        
        private void UnsubscribeEvents()
        {
            _signalBus.Unsubscribe<LoadLevelSignal>(OnLoadLevel);
        }
        
        public void Dispose()
        {
            UnsubscribeEvents();
        }
    }
}