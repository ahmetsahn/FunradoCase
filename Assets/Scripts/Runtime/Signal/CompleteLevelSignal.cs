namespace Runtime.Signal
{
    public readonly struct CompleteLevelSignal
    {
        public readonly bool HasWon;
        
        public CompleteLevelSignal(bool hasWon)
        {
            HasWon = hasWon;
        }
    }
}