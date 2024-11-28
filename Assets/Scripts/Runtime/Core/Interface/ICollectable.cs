using UnityEngine;

namespace Runtime.Core.Interface
{
    public interface ICollectable :  IColor, ICell
    {
        public Transform Transform { get; }
        
        public void ScaleDownWithAnimation();
    }
}