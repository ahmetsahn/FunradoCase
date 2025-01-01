using System;
using Runtime.Signal;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;
using Object = UnityEngine.Object;

namespace Runtime.Managers
{
    public class LevelLoader : IDisposable
    {
        private readonly SignalBus _signalBus;
        
        private readonly IInstantiator _instantiator;
        
        private readonly AssetReferenceGameObject[] _levelPrefabs;

        private GameObject _currentLevelInstance;
        
        public LevelLoader(SignalBus signalBus, IInstantiator instantiator, LevelLoaderConfig config)
        {
            _signalBus = signalBus;
            _instantiator = instantiator;
            _levelPrefabs = config.LevelPrefabs;
            
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
                _signalBus.Fire(new DestroyCurrentLevelSignal());
            }
            
            Addressables.LoadAssetAsync<GameObject>(_levelPrefabs[signal.LevelIndex]).Completed += OnAddressableLoaded;
        }
        
        private void OnAddressableLoaded(AsyncOperationHandle<GameObject> levelPrefab)
        {
            _currentLevelInstance = _instantiator.InstantiatePrefab(levelPrefab.Result);
        }
        
        private void OnDestroyCurrentLevel(DestroyCurrentLevelSignal signal)
        {
            if (_currentLevelInstance != null)
            {
                Object.Destroy(_currentLevelInstance);
                Addressables.ReleaseInstance(_currentLevelInstance);
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
    
    [Serializable]
    public class LevelLoaderConfig
    {
        public AssetReferenceGameObject[] LevelPrefabs;
    }
}