using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Runtime.Core.Interface;
using Runtime.Enums;
using Runtime.Gameplay.Abstract;
using Runtime.Utilities;
using UnityEngine;

namespace Runtime.Gameplay.Grape.View
{
    public class GrapeView : ScalableObject, ICollectable
    {
        [field: SerializeField]
        public ColorType ColorType { get; set; }
        
        public event Action OnShowErrorFeedback;

        public MeshRenderer MeshRenderer;

        public Transform Transform => transform;

        public async void ScaleUpAndDown(bool freeze = false)
        {
            try
            {
                await transform.DOScale(1.2f, 0.1f).SetEase(Ease.Linear).AsyncWaitForCompletion();
            
                if (freeze)
                {
                    OnShowErrorFeedback?.Invoke();
                    await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
                }
            
                await transform.DOScale(0.8f, 0.1f).SetEase(Ease.Linear).AsyncWaitForCompletion();
            }
            
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        protected override void ScaleUp()
        {
            transform.DOScale(Constants.GRAPE_DEFAULT_SCALE, Constants.OBJECT_SCALE_DURATION).SetEase(Ease.OutBounce);
        }

        public void ScaleDownWithAnimation()
        {
            AnimateScaleToZero(Transform, 0, Constants.CELL_SCALE_DOWN_DURATION);
        }

        public void DestroyCell()
        {
            AnimateScaleToZero(CellViewBelow.transform, 0, Constants.CELL_SCALE_DOWN_DURATION, true);
        }
    }
}