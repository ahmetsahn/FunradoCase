using System;
using Cysharp.Threading.Tasks;
using Runtime.Gameplay.Grape.Model;
using Runtime.Gameplay.Grape.View;
using UnityEngine;

namespace Runtime.Gameplay.Grape.Controller
{
    public class GrapeVisualController : IDisposable
    {
        private readonly GrapeView _view;

        private readonly GrapeModel _model;

        private Material _defaultMaterial;

        public GrapeVisualController(GrapeView view, GrapeModel model)
        {
            _view = view;
            _model = model;

            SetDefaultMaterial();
            SubscribeEvents();
        }

        private void SetDefaultMaterial()
        {
            _defaultMaterial = _view.MeshRenderer.material;
        }

        private void SubscribeEvents()
        {
            _view.OnShowErrorFeedback += OnShowErrorFeedback;
        }

        private async void OnShowErrorFeedback()
        {
            try
            {
                _view.MeshRenderer.material = _model.Data.FailedMaterial;
                await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
                _view.MeshRenderer.material = _defaultMaterial;
            }

            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private void UnsubscribeEvents()
        {
            _view.OnShowErrorFeedback -= OnShowErrorFeedback;
        }

        public void Dispose()
        {
            UnsubscribeEvents();
        }
    }
}