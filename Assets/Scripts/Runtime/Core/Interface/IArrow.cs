using Runtime.Enums;

namespace Runtime.Core.Interface
{
    public interface IArrow : IInteractable
    {
        public DirectionType DirectionType { get; }
    }
}