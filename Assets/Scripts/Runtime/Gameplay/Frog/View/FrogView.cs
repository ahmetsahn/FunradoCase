using System;
using Runtime.Signal;
using UnityEngine;
using Zenject;

namespace Runtime.Gameplay.Frog.View
{
    public class FrogView : MonoBehaviour
    {
        private SignalBus _signalBus;
        public event Action OnClick; 
        
        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        private void OnMouseDown()
        {
            //TODO: Kontrol edilecek
            Debug.Log("Frog clicked.");
            OnClick?.Invoke();
            _signalBus.Fire(new ReduceCountOfRemainingMoveSignal());
        }
    }
}
