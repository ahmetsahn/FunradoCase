using Runtime.Enums;

namespace Runtime.Core.Interface
{
    public interface IInteractable
    {
        public ColorType ColorType { get; }
        
        public void ScaleDownWithAnimation();
        
        public void DestroyCell();
    }
}