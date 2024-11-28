using System;
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

        private void Awake()
        {
            GetDirectionFromRotation();
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
    }
}