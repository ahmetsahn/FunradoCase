using System;
using Runtime.Gameplay.Frog.Model;
using Runtime.Gameplay.Frog.View;
using UnityEngine;

namespace Runtime.Gameplay.Frog.Controller
{
    public class FrogAnimationController : IDisposable
    {
        private readonly FrogView _view;
        
        private readonly FrogModel _model;
        
        private readonly Animator _animator;
        
        public FrogAnimationController(
            FrogView view, 
            FrogModel model,
            FrogAnimationControllerConfig config)
        {
            _view = view;
            _model = model;
            _animator = config.Animator;
            
            SubscribeEvents();
        }
        
        private void SubscribeEvents()
        {
            _view.OnTongueAnimationStart += OnTongueAnimationStart;
            _view.OnTongueAnimationEnd += OnTongueAnimationEnd;
        }
        
        private void OnTongueAnimationStart()
        {
            CrossFadeAnimation(_model.AnimationHashMouthOpen);
        }
        
        private void OnTongueAnimationEnd()
        {
            CrossFadeAnimation(_model.AnimationHashMouthClose);
        }
        
        private void CrossFadeAnimation(int animationHash, float fadeTime = 0f)
        {
            _animator.CrossFade(animationHash, fadeTime);
        }
        
        
        private void UnsubscribeEvents()
        {
            _view.OnTongueAnimationStart -= OnTongueAnimationStart;
            _view.OnTongueAnimationEnd -= OnTongueAnimationEnd;
        }

        public void Dispose()
        {
            UnsubscribeEvents();
        }
    }
    
    [Serializable]
    public struct FrogAnimationControllerConfig
    {
        public Animator Animator;
    }
}