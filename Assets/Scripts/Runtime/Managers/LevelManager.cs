using Runtime.Data.Scriptable;

namespace Runtime.Managers
{
    public class LevelManager
    {
        private readonly LevelDataListSo _levelDataListSo;
        
        private readonly GameManager _gameManager;
        
        private readonly int _currentLevelIndex;
        
        public int RemainingMoves { get; private set; }
        public int RemainingFrogs { get; private set; }
        
        public LevelManager(LevelDataListSo levelDataListSo, GameManager gameManager)
        {
            _levelDataListSo = levelDataListSo;
            _gameManager = gameManager;
            _currentLevelIndex = _gameManager.GetCurrentLevelIndex();
            
            InitializeLevel();
        }
        
        private void InitializeLevel()
        {
            RemainingMoves = _levelDataListSo.Levels[_currentLevelIndex].MaxMoves;
            RemainingFrogs = _levelDataListSo.Levels[_currentLevelIndex].FrogCount;
        }
        
        private void CompleteLevel()
        {
            int nextLevelIndex = _currentLevelIndex + 1;
            _gameManager.SetCurrentLevelIndex(nextLevelIndex);
        }
        
    }
}