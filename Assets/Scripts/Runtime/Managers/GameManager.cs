using System;
using Runtime.Enums;
using Runtime.Signal;
using Zenject;

namespace Runtime.Managers
{
    public class GameManager : IInitializable, IDisposable
    {
        private readonly SaveManager _saveManager;
        
        private readonly SignalBus _signalBus;

        private int _currentLevelIndex;

        private int _maxLevel;
        
        public GameManager(SaveManager saveManager, SignalBus signalBus, GameManagerConfig config)
        {
            _saveManager = saveManager;
            _signalBus = signalBus;
            _maxLevel = config.MaxLevel;
            _currentLevelIndex = _saveManager.LoadLevelIndex();
            
            SubscribeEvents();
        }

        public void Initialize()
        {
            _signalBus.Fire(new OpenUIPanelSignal(UIPanelType.GamePanel));
            LoadCurrentLevel();
        }
        
        private void SubscribeEvents()
        {
            _signalBus.Subscribe<CompleteLevelSignal>(OnCompleteLevel);
        }
        
        public int GetCurrentLevelIndex()
        {
            return _currentLevelIndex;
        }
        
        private void SetCurrentLevelIndex(int index)
        {
            _currentLevelIndex = index;
            _saveManager.SaveLevelIndex(index);
        }
        
        public void LoadCurrentLevel() 
        {
            _signalBus.Fire(new LoadLevelSignal(_currentLevelIndex));
        }
        
        private void OnCompleteLevel(CompleteLevelSignal signal) {
            
            _signalBus.Fire(new DestroyCurrentLevelSignal());
            
            if (signal.HasWon) {
                IncreaseCurrentLevelIndex();
                SetCurrentLevelIndex(_currentLevelIndex);
                _signalBus.Fire(new CloseUIPanelSignal(UIPanelType.GamePanel)) ;
                _signalBus.Fire(new OpenUIPanelSignal(UIPanelType.WinPanel));
            }
            
            else 
            {
                _signalBus.Fire(new CloseUIPanelSignal(UIPanelType.GamePanel));
                _signalBus.Fire(new OpenUIPanelSignal(UIPanelType.GameOverPanel));
            }
        }
        
        private void IncreaseCurrentLevelIndex()
        {
            _currentLevelIndex++;
            
            if (_currentLevelIndex >= _maxLevel)
            {
                _currentLevelIndex = 0;
            }
        }
        
        private void UnsubscribeEvents()
        {
            _signalBus.Unsubscribe<CompleteLevelSignal>(OnCompleteLevel);
        }
        
        public void Dispose()
        {
            UnsubscribeEvents();
        }
    }

    [Serializable]
    public class GameManagerConfig
    {
        public int MaxLevel;
    }
}