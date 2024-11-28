using UnityEngine;

namespace Runtime.Core.Interface
{
    public interface ICollectable :  IColor
    {
        public Transform Transform { get; }
        
        public void ScaleDownWithAnimation();
    }
}