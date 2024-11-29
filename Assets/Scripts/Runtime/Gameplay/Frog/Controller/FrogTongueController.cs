using System;
using Cysharp.Threading.Tasks;
using Runtime.Enums;
using Runtime.Gameplay.Frog.Model;
using Runtime.Gameplay.Frog.Service;
using Runtime.Gameplay.Frog.View;
using Runtime.Managers;
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
        
        private readonly LevelManager _levelManager;
        
        private readonly SoundManager _soundManager;
        
        private bool _isAnimationInProgress;

        public FrogTongueController(
            FrogView view, 
            FrogModel model, 
            RaycastService raycastService, 
            SplineService splineService, 
            CollectablesService collectablesService,
            LevelManager levelManager,
            SoundManager soundManager)
        {
            _view = view;
            _model = model;
            _raycastService = raycastService;
            _splineService = splineService;
            _collectablesService = collectablesService;
            _levelManager = levelManager;
            _soundManager = soundManager;

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
                if (!CanStartAnimation())
                {
                    return;
                }
                
                await _view.ScaleUpAndDown();
                
                BeginAnimationSequence();
                
                await AnimateSpline(1);
                
                await HandleCollectionOutcome();
                
                await AnimateSpline(0);
                
                HandlePostCollectionFeedback();
                EndAnimationSequence();
                
            }
            
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        
        private bool CanStartAnimation()
        {
            return !_isAnimationInProgress && _levelManager.IsHasRemainingMoves();
        }
        
        private void SetAnimationInProgress(bool value)
        {
            _isAnimationInProgress = value;
        }

        private void BeginAnimationSequence()
        {
            SetAnimationInProgress(true);
            _levelManager.ReduceCountOfMove();
            _view.OnTongueAnimationStart?.Invoke();
            _levelManager.RegisterFrogAnimation();
            AddStartKnotToSpline();
            DetectObjects();
            _collectablesService.AnimateInteractedObjectsWithFeedback(_model.ColorType);
        }

        private void EndAnimationSequence()
        {
            _view.OnTongueAnimationEnd?.Invoke();
            _splineService.ResetSpline();
            _collectablesService.Reset();
            _isAnimationInProgress = false;
            _levelManager.RegisterAnimationEnd();
        }

        private async UniTask HandleCollectionOutcome()
        {
            try
            {
                if (_collectablesService.IsCollectionSuccessful)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(Constants.TONGUE_WAIT_DURATION));
                    CollectAndAnimateObjects();
                    _collectablesService.DestroyInteractedObjectsCell();
                    _levelManager.ReduceCountOfFrog();
                }

                else
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(Constants.GRAPE_INCORRECT_DELAY));
                }
                
                _soundManager.PlayPopSound(AudioClipType.Transition);
            }
            
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        private async void HandlePostCollectionFeedback()
        {
            try
            {
                if (!_collectablesService.IsCollectionSuccessful)
                {
                    return;
                }
                
                await UniTask.Delay(TimeSpan.FromSeconds(Constants.TONGUE_ANIMATION_DURATION));
                _view.ScaleDown();
                _view.CellScaleDown();
            }
            
            catch (Exception e)
            {
                Debug.LogError(e);
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
