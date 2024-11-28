using Runtime.Enums;

namespace Runtime.Core.Interface
{
    public interface IArrow : IColor, ICell
    {
        public DirectionType DirectionType { get; }
    }
}