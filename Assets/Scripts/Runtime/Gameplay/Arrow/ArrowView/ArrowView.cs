using System;
using DG.Tweening;
using Runtime.Core.Interface;
using Runtime.Enums;
using UnityEngine;

namespace Runtime.Gameplay.Arrow.ArrowView
{
    public class ArrowView : MonoBehaviour, IArrow
    {
        [field: SerializeField]
        public ColorType ColorType { get; set; }
        public DirectionType DirectionType { get; private set; }
        
        private Transform _cellTransform;

        private void Awake()
        {
            GetDirectionFromRotation();
            FindGroundTransform();
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
        
        private void FindGroundTransform()
        {
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1))
            {
                _cellTransform = hit.transform;
            }
        }

        public void ScaleDownWithAnimation()
        {
            transform.DOScale(0, 0.5f).SetEase(Ease.Linear);
        }

        public void DestroyCell()
        {
            _cellTransform.DOScale(0, 0.5f).SetEase(Ease.Linear);
        }
    }
}