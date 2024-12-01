using System;
using Runtime.Gameplay.Frog.Model;
using Runtime.Gameplay.Frog.View;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Runtime.Gameplay.Frog.Controller
{
    public class FrogVisualController : IDisposable
    {
        private readonly FrogView _view;

        private readonly FrogModel _model;
        
        public FrogVisualController(FrogView view, FrogModel model)
        {
            _view = view;
            _model = model;
            
            SubscribeEvents();
        }
        
        private void SubscribeEvents()
        {
            _view.OnDestroy += OnDestroy;
        }
        
        private void OnDestroy()
        {
            Object.Instantiate(_model.Data.DestroyParticle, _view.transform.position, Quaternion.identity);
        }
        
        private void UnsubscribeEvents()
        {
            _view.OnDestroy -= OnDestroy;
        }
        
        public void Dispose()
        {
            UnsubscribeEvents();
        }
    }
}