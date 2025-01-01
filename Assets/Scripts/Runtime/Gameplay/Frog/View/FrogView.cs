using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Runtime.Core.Interface;
using Runtime.Gameplay.Abstract;
using Runtime.Utilities;
using UnityEngine;

namespace Runtime.Gameplay.Frog.View
{
    public class FrogView : ScalableObject
    {
        public event Action OnClick;
        public event Action OnDestroy;
        public Action OnTongueAnimationStart;
        public Action OnTongueAnimationEnd;
        
        private void OnMouseDown()
        {
            OnClick?.Invoke();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ICollectable collectable))
            {
                collectable.ScaleDownWithAnimation();
            }
        }

        protected override void ScaleUp()
        {
            transform.DOScale(Constants.FROG_DEFAULT_SCALE, Constants.OBJECT_SCALE_DURATION).SetEase(Ease.OutBounce);
        }
        
        public async UniTask ScaleUpAndDown()
        {
            transform.DOScale(Constants.FROG_CLICK_SCALE_UP, Constants.FROG_CLICK_SCALE_DURATION).SetEase(Ease.Linear).OnComplete(() =>
            {
                transform.DOScale(Constants.FROG_DEFAULT_SCALE, Constants.FROG_CLICK_SCALE_DURATION).SetEase(Ease.Linear);
            });
            
            await UniTask.Delay(TimeSpan.FromSeconds(Constants.FROG_CLICK_ANIMATION_DURATION));
        }
        
        public async void ScaleDown()
        {
            try
            {
                AnimateScaleToZero(transform, 0, Constants.FROG_CLICK_SCALE_DURATION);
                await UniTask.Delay(TimeSpan.FromSeconds(Constants.FROG_CLICK_ANIMATION_DURATION));
                OnDestroy?.Invoke();
            }
            
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        
        public void CellScaleDown()
        {
            AnimateScaleToZero(CellViewBelow.transform, 0, Constants.CELL_SCALE_DOWN_DURATION,true);
        }

        private void OnDrawGizmos()
        {
            // start position = vector3(1,0.432,0), direction = vector3(0,0,1), distance = 10
            Gizmos.DrawRay(new Vector3(1, 0.432f, 0), new Vector3(0, 0, 1) * Constants.TONGUE_MAX_RAYCAST_DISTANCE);
        }
    }
}
