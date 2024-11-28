using Runtime.Enums;

namespace Runtime.Core.Interface
{
    public interface IArrow : IColor
    {
        public DirectionType DirectionType { get; }
    }
}