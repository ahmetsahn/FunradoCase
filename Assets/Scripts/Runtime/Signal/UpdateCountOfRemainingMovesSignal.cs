namespace Runtime.Signal
{
    public readonly struct UpdateCountOfRemainingMovesSignal
    {
        public readonly int MoveCount;
        
        public UpdateCountOfRemainingMovesSignal(int moveCount)
        {
            MoveCount = moveCount;
        }
    }
}