namespace Runtime.Signal
{
    public readonly struct CompleteLevelSignal
    {
        public readonly int LevelIndex;
        
        public CompleteLevelSignal(int levelIndex)
        {
            LevelIndex = levelIndex;
        }
    }
}