using System;
using System.Linq;
using Runtime.Core.Interface;
using Runtime.Signal;
using UnityEngine;
using Zenject;

namespace Runtime.Gameplay.Frog.View
{
    public class FrogView : MonoBehaviour
    {
        public event Action OnClick;
        
        public Action OnTongueAnimationStart;
        
        public Action OnTongueAnimationEnd;
        
        private void OnMouseDown()
        {
            OnClick?.Invoke();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ICollectable collectable))
            {
                collectable.ScaleDownWithAnimation();
            }
        }
    }
}
