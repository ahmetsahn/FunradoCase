using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Runtime.Core.Interface;
using Runtime.Gameplay.Abstract;
using Runtime.Signal;
using Runtime.Utilities;
using UnityEngine;
using Zenject;

namespace Runtime.Gameplay.Frog.View
{
    public class FrogView : ScalableObject
    {
        public event Action OnClick;
        
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
        
        public void ScaleDown()
        {
            AnimateScaleToZero(transform, 0, Constants.FROG_CLICK_SCALE_DURATION);
        }
        
        public void CellScaleDown()
        {
            AnimateScaleToZero(CellViewBelow.transform, 0, Constants.CELL_SCALE_DOWN_DURATION,true);
        }
    }
}
