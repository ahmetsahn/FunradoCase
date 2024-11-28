using System;
using System.Linq;
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
            transform.DOScale(Constants.FROG_SCALE, Constants.OBJECT_SCALE_DURATION).SetEase(Ease.OutBounce);
        }
        
        public void ScaleDown()
        {
            AnimateScaleToZero(transform, 0, 0.1f);
        }
        
        public void CellScaleDown()
        {
            AnimateScaleToZero(CellTransform, 0, 0.1f);
        }
    }
}
