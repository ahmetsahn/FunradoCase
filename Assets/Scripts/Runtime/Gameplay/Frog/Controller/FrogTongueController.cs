using System;
using Cysharp.Threading.Tasks;
using Runtime.Gameplay.Frog.Model;
using Runtime.Gameplay.Frog.Service;
using Runtime.Gameplay.Frog.View;
using Runtime.Utilities;
using UnityEngine;

namespace Runtime.Gameplay.Frog.Controller
{
    public class FrogTongueController : IDisposable
    {
        private readonly FrogView _view;
        
        private readonly FrogModel _model;
        
        private readonly RaycastService _raycastService;
        
        private readonly SplineService _splineService;
        
        private readonly CollectablesService _collectablesService;

        public FrogTongueController(
            FrogView view, 
            FrogModel model, 
            RaycastService raycastService, 
            SplineService splineService, 
            CollectablesService collectablesService)
        {
            _view = view;
            _model = model;
            _raycastService = raycastService;
            _splineService = splineService;
            _collectablesService = collectablesService;

            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            _view.OnClick += HandleOnClick;
        }

        private async void HandleOnClick()
        {
            try
            {
                _view.OnTongueAnimationStart?.Invoke();
                AddStartKnotToSpline();
                DetectObjects();
                await AnimateSpline(1);
                CollectAndAnimateObjects();
                await AnimateSpline(0);
                _view.OnTongueAnimationEnd?.Invoke();
            }
            
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        
        private void AddStartKnotToSpline()
        {
            var position = new Vector3(_view.transform.position.x, Constants.GRAPE_FIXED_Y_POSITION, _view.transform.position.z);
            _splineService.AddKnot(position);
        }
        
        private void DetectObjects()
        {
            bool collectionSuccess = _raycastService.RaycastAndDetectObjects(_view.transform.position, _view.transform.forward, _splineService, _collectablesService.CollectedObjects, _model.ColorType);
            _collectablesService.IsCollectionSuccessful = collectionSuccess;
        }

        private async UniTask AnimateSpline(int targetRangeValue)
        {
            float animationDuration = _splineService.SplineContainer.Spline.Count * Constants.SPLINE_ANIMATION_DURATION;
            _splineService.AnimateSplineRange(targetRangeValue, animationDuration);
            await UniTask.Delay(TimeSpan.FromSeconds(animationDuration));
        }

        private void CollectAndAnimateObjects()
        {
            _collectablesService.CollectObjects(_splineService.GetSplinePointsReverse(), _splineService.SplineContainer);
            float animationDuration = _splineService.SplineContainer.Spline.Count * Constants.SPLINE_ANIMATION_DURATION;
            _splineService.AnimateSplineRange(0, animationDuration);
        }

        private void UnsubscribeEvents()
        {
            _view.OnClick -= HandleOnClick;
        }

        public void Dispose()
        {
            UnsubscribeEvents();
        }
    }
}
