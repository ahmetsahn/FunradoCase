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
        private SignalBus _signalBus;
        public event Action OnClick;
        
        public Action OnTongueAnimationStart;
        
        public Action OnTongueAnimationEnd;
        
        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        private void OnMouseDown()
        {
            OnClick?.Invoke();
            _signalBus.Fire(new ReduceCountOfRemainingMoveSignal());
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
