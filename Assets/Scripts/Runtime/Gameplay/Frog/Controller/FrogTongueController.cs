using System;
using Cysharp.Threading.Tasks;
using Runtime.Gameplay.Frog.Model;
using Runtime.Gameplay.Frog.Service;
using Runtime.Gameplay.Frog.View;
using Runtime.Managers;
using Runtime.Signal;
using Runtime.Utilities;
using UnityEngine;
using Zenject;

namespace Runtime.Gameplay.Frog.Controller
{
    public class FrogTongueController : IDisposable
    {
        private readonly FrogView _view;
        
        private readonly FrogModel _model;
        
        private readonly SignalBus _signalBus;
        
        private readonly RaycastService _raycastService;
        
        private readonly SplineService _splineService;
        
        private readonly CollectablesService _collectablesService;
        
        private readonly LevelManager _levelManager;
        
        private bool _isAnimationInProgress;

        public FrogTongueController(
            FrogView view, 
            FrogModel model, 
            SignalBus signalBus,
            RaycastService raycastService, 
            SplineService splineService, 
            CollectablesService collectablesService,
            LevelManager levelManager)
        {
            _view = view;
            _model = model;
            _signalBus = signalBus;
            _raycastService = raycastService;
            _splineService = splineService;
            _collectablesService = collectablesService;
            _levelManager = levelManager;

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
                if (_isAnimationInProgress || !_levelManager.IsHasRemainingMoves())
                {
                    return;
                }
                
                _isAnimationInProgress = true;
                _levelManager.ReduceCountOfMove();
                _view.OnTongueAnimationStart?.Invoke();
                _levelManager.RegisterFrogAnimation();
                AddStartKnotToSpline();
                DetectObjects();
                _collectablesService.AnimateInteractedObjectsWithFeedback(_model.ColorType);
                await AnimateSpline(1);
                
                bool isCollectionSuccessful = _collectablesService.IsCollectionSuccessful;
                if (isCollectionSuccessful)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(Constants.GRAPE_COLLECT_DELAY));
                    CollectAndAnimateObjects();
                    _collectablesService.DestroyInteractedObjectsCell();
                    _levelManager.ReduceCountOfFrog();
                }

                else
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(Constants.GRAPE_INCORRECT_DELAY));
                }
                
                await AnimateSpline(0);
                _view.OnTongueAnimationEnd?.Invoke();
                _splineService.ResetSpline();
                _collectablesService.Reset();
                _isAnimationInProgress = false;
                _levelManager.RegisterAnimationEnd();
                
                if (isCollectionSuccessful)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(Constants.TONGUE_ANIMATION_DURATION));
                    _view.ScaleDownWithAnimation();
                    _view.ScaleDownCell();
                }
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
            bool collectionSuccess = _raycastService.RaycastAndDetectObjects(_view.transform.position, _view.transform.forward, _splineService, _collectablesService.InteractedObjects, _model.ColorType);
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
            _collectablesService.AnimateCollectablesAlongPath(_splineService.GetSplinePointsReverse(), _splineService.SplineContainer);
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
