namespace Runtime.Signal
{
    public readonly struct LoadLevelSignal
    {
        public readonly int LevelIndex;
        
        public LoadLevelSignal(int levelIndex)
        {
            LevelIndex = levelIndex;
        }
    }
}