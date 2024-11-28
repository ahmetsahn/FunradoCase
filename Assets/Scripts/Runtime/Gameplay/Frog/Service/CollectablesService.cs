using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Runtime.Core.Interface;
using Runtime.Enums;
using Runtime.Utilities;
using UnityEngine;
using UnityEngine.Splines;

namespace Runtime.Gameplay.Frog.Service
{
    public class CollectablesService
    {
        public List<IInteractable> InteractedObjects { get; } = new();
        public bool IsCollectionSuccessful { get; set; } = true;
        
        public async void AnimateCollectablesAlongPath(List<Vector3> pathPositions, SplineContainer splineContainer)
        {
            try
            {
                float durationShortener = 0f;
                for (int i = InteractedObjects.Count - 1; i >= 0; i--)
                {
                    
                    if (pathPositions.Count > 0)
                    {
                        pathPositions.RemoveAt(0);
                    }
                    
                    if (InteractedObjects[i] is ICollectable collectable)
                    {
                        MoveObjectAlongPath(collectable, pathPositions, durationShortener, splineContainer);
                        await UniTask.Delay(TimeSpan.FromSeconds(Constants.GRAPE_COLLECT_DELAY));
                    }
                    
                    else
                    {
                        await UniTask.Delay(TimeSpan.FromSeconds(Constants.EXTRA_DELAY_FOR_NON_COLLECTABLES));
                    }

                    durationShortener += Constants.DELAY_INCREMENT_PER_GRAPE;
                }
            }
            
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        
        private void MoveObjectAlongPath(ICollectable collectable, List<Vector3> pathPositions, float durationShortener, SplineContainer splineContainer)
        {
            collectable.Transform.DOPath(pathPositions.ToArray(), 
                    Constants.TONGUE_ANIMATION_DURATION * splineContainer.Spline.Count - durationShortener, 
                    PathType.CatmullRom)
                .SetEase(Ease.Linear)
                .From();
        }
        
        public async void DestroyInteractedObjectsCell()
        {
            try
            {
                for (int i = InteractedObjects.Count - 1; i >= 0; i--)
                {
                    if (InteractedObjects[i] is ICollectable collectable)
                    {
                        await UniTask.Delay(TimeSpan.FromSeconds(Constants.GRAPE_CELL_DESTROY_INTERVAL));
                    }
                    
                    if (InteractedObjects[i] is IArrow arrow)
                    {
                        await UniTask.Delay(TimeSpan.FromSeconds(Constants.ARROW_CELL__DESTROY_INTERVAL));
                        arrow.ScaleDownWithAnimation();
                    }
                    
                    InteractedObjects[i].DestroyCell();
                }
            }
            
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        
        public async void AnimateInteractedObjectsWithFeedback(ColorType frogColorType)
        {
            try
            {
                for (int i = 0; i < InteractedObjects.Count; i++)
                {
                    if (InteractedObjects[i] is ICollectable collectable)
                    {
                        await UniTask.Delay(TimeSpan.FromSeconds(Constants.SPLINE_ANIMATION_DURATION));
                        
                        if(i == InteractedObjects.Count - 1 && !collectable.ColorType.Equals(frogColorType))
                        {
                            collectable.ScaleUpAndDown(true);
                            collectable.ShowErrorFeedback();
                            return;
                        }
                        
                        collectable.ScaleUpAndDown();
                    }
                    
                    else
                    {
                        await UniTask.Delay(TimeSpan.FromSeconds(Constants.EXTRA_DELAY_FOR_NON_COLLECTABLES));
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        

        public void Reset()
        {
            InteractedObjects.Clear();
            IsCollectionSuccessful = true;
        }
    }
}
