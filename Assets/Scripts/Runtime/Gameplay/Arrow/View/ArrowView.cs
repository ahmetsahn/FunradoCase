using System;
using DG.Tweening;
using Runtime.Core.Interface;
using Runtime.Enums;
using Runtime.Gameplay.Abstract;
using Runtime.Utilities;
using UnityEngine;

namespace Runtime.Gameplay.Arrow.View
{
    public class ArrowView : ScalableObject, IArrow
    {
        [field: SerializeField]
        public ColorType ColorType { get; set; }
        public DirectionType DirectionType { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            GetDirectionFromRotation();

            Debug.Log(DirectionType);
        }

        private void GetDirectionFromRotation()
        {
            float eulerAnglesY = transform.eulerAngles.y;
            DirectionType = eulerAnglesY switch
            {
                0 => DirectionType.Forward,
                90 => DirectionType.Right,
                180 => DirectionType.Back,
                270 => DirectionType.Left,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        protected override void ScaleUp()
        {
            transform.DOScale(Constants.ARROW_SCALE, Constants.OBJECT_SCALE_DURATION).SetEase(Ease.OutBounce);
        }

        public void ScaleDownWithAnimation()
        {
            AnimateScaleToZero(transform, 0, 0.3f);
        }

        public void DestroyCell()
        {
            AnimateScaleToZero(CellViewBelow.transform, 0f, 0.3f,true);
        }
    }
}