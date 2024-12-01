using UnityEngine;

namespace Runtime.Core.Interface
{
    public interface ICollectable :  IInteractable
    {
        public Transform Transform { get; }
        
        public void ScaleUpAndDown(bool freeze = false);
    }
}