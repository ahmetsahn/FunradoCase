using System;
using Cysharp.Threading.Tasks;
using Runtime.Data.Scriptable;
using Runtime.Gameplay.Frog.Controller;
using Runtime.Gameplay.Frog.View;
using Runtime.Signal;
using Runtime.Utilities;
using UnityEngine;
using Zenject;

namespace Runtime.Managers
{
    public class LevelManager : IDisposable
    {
        private readonly LevelDataListSo _levelDataListSo;
        
        private readonly GameManager _gameManager;
        
        private readonly SignalBus _signalBus;
        
        private int _activeAnimationCount;

        private int _remainingMoves;
        private int _remainingFrogs;
        
        public LevelManager(
            LevelManagerConfig config, 
            GameManager gameManager,
            SignalBus signalBus)
        {
            _levelDataListSo = config.LevelDataListSo;
            _gameManager = gameManager;
            _signalBus = signalBus;
            
            InitializeLevel();
            SubscribeEvents();
        }
        
        private void InitializeLevel()
        {
            var currentLevelIndex = _gameManager.GetCurrentLevelIndex();
            _remainingMoves = _levelDataListSo.Levels[currentLevelIndex].MaxMoves;
            _remainingFrogs = _levelDataListSo.Levels[currentLevelIndex].FrogCount;
        }
        
        private void SubscribeEvents()
        {
            _signalBus.Subscribe<LoadLevelSignal>(OnLoadLevel);
        }

        private void OnLoadLevel(LoadLevelSignal _)
        {
            InitializeLevel();
            _signalBus.Fire(new UpdateCountOfRemainingMovesSignal(_remainingMoves));
        }

        public void ReduceCountOfMove()
        {
            _remainingMoves--;
            _signalBus.Fire(new UpdateCountOfRemainingMovesSignal(_remainingMoves));
        }
        
        public void ReduceCountOfFrog()
        {
            _remainingFrogs--;
        }
        
        public void RegisterFrogAnimation()
        {
            _activeAnimationCount++;
        }
        
        public void RegisterAnimationEnd()
        {
            _activeAnimationCount--;
            
            if (_activeAnimationCount == 0)
            {
                CheckLevelCompletion();
            }
        }
        
        private async void CheckLevelCompletion()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(Constants.COMPLETED_LEVEL_DELAY));
            
            if (_remainingFrogs <= 0)
            {
                _signalBus.Fire(new CompleteLevelSignal(true));
                return;
            }
            
            if (_remainingMoves <= 0)
            {
                _signalBus.Fire(new CompleteLevelSignal(false));
            }
        }
        
        public bool IsHasRemainingMoves()
        {
            return _remainingMoves > 0;
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
    
    [Serializable]
    public struct LevelManagerConfig
    {
        public LevelDataListSo LevelDataListSo;
    }
}