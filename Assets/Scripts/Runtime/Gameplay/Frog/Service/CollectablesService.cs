using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Runtime.Core.Interface;
using Runtime.Utilities;
using UnityEngine;
using UnityEngine.Splines;

namespace Runtime.Gameplay.Frog.Service
{
    public class CollectablesService
    {
        public List<IColor> CollectedObjects { get; private set; } = new();
        public bool IsCollectionSuccessful { get; set; } = true;
        
        public async void CollectObjects(List<Vector3> pathPositions, SplineContainer splineContainer)
        {
            try
            {
                if (!IsCollectionSuccessful) return;

                float durationShortener = 0f;
                for (int i = CollectedObjects.Count - 1; i >= 0; i--)
                {
                    
                    if (CollectedObjects[i] is ICollectable collectable)
                    {
                        pathPositions.RemoveAt(0);
                        MoveObjectAlongPath(collectable, pathPositions, durationShortener, splineContainer);
                        await UniTask.Delay(TimeSpan.FromSeconds(Constants.GRAPE_COLLECT_DELAY));
                    }
                    
                    else
                    {
                        pathPositions.RemoveAt(0);
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
        

        public void Reset()
        {
            CollectedObjects.Clear();
            IsCollectionSuccessful = true;
        }
    }
}
