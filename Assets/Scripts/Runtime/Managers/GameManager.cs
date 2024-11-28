using Runtime.Enums;
using Runtime.Signal;
using Zenject;

namespace Runtime.Managers
{
    public class GameManager : IInitializable
    {
        private readonly SaveManager _saveManager;
        
        private readonly SignalBus _signalBus;

        private int _currentLevelIndex;
        
        public GameManager(SaveManager saveManager, SignalBus signalBus)
        {
            _saveManager = saveManager;
            _signalBus = signalBus;
            
            _currentLevelIndex = _saveManager.LoadLevelIndex();
        }

        public void Initialize()
        {
            FirePanelSignal(UIPanelType.GamePanel);
            LoadCurrentLevel();
        }
        
        public int GetCurrentLevelIndex()
        {
            return _currentLevelIndex;
        }
        
        public void SetCurrentLevelIndex(int index)
        {
            _currentLevelIndex = index;
            _saveManager.SaveLevelIndex(index);
        }
        
        private void LoadCurrentLevel() 
        {
            _signalBus.Fire(new LoadLevelSignal(_currentLevelIndex));
        }
        
        private void FirePanelSignal(UIPanelType panelType)
        {
            _signalBus.Fire(new OpenUIPanelSignal(panelType));
        }
        
        public void CompleteLevel(bool hasWon) {
            if (hasWon) {
                _currentLevelIndex++;
                SaveManager saveManager = new SaveManager();
                saveManager.SaveLevelIndex(_currentLevelIndex);
                _signalBus.Fire(new CompleteLevelSignal(_currentLevelIndex));
                FirePanelSignal(UIPanelType.WinPanel);
            }
            
            else 
            {
                _signalBus.Fire(new RetryLevelSignal());
                FirePanelSignal(UIPanelType.GameOverPanel);
            }
        }
    }
}