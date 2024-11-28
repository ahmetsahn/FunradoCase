using System;
using Runtime.Data.Scriptable;
using Runtime.Signal;
using Zenject;

namespace Runtime.Managers
{
    public class LevelManager : IDisposable
    {
        private readonly LevelDataListSo _levelDataListSo;
        
        private readonly GameManager _gameManager;
        
        private readonly SignalBus _signalBus;
        
        private int _currentLevelIndex;

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
            _currentLevelIndex = _gameManager.GetCurrentLevelIndex();
            _remainingMoves = _levelDataListSo.Levels[_currentLevelIndex].MaxMoves;
            _remainingFrogs = _levelDataListSo.Levels[_currentLevelIndex].FrogCount;
        }
        
        private void SubscribeEvents()
        {
            _signalBus.Subscribe<LoadLevelSignal>(OnLoadLevel);
            _signalBus.Subscribe<ReduceCountOfRemainingMoveSignal>(OnReduceCountOfMove);
        }

        private void OnLoadLevel(LoadLevelSignal _)
        {
            _signalBus.Fire(new UpdateCountOfRemainingMovesSignal(_remainingMoves));
        }

        private void OnReduceCountOfMove()
        {
            _remainingMoves--;
            _signalBus.Fire(new UpdateCountOfRemainingMovesSignal(_remainingMoves));
            
            if (_remainingMoves <= 0)
            {
                //TODO: Game Over
            }
        }
        
        private void OnReduceCountOfFrog()
        {
            _remainingFrogs--;
            
            if (_remainingFrogs <= 0)
            {
                //TODO: Level Complete
            }
        }
        
        private void CompleteLevel()
        {
            int nextLevelIndex = _currentLevelIndex + 1;
            _gameManager.SetCurrentLevelIndex(nextLevelIndex);
        }
        
        private void UnsubscribeEvents()
        {
            _signalBus.Unsubscribe<ReduceCountOfRemainingMoveSignal>(OnReduceCountOfMove);
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