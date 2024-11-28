using Runtime.Core.Interface;
using Runtime.Enums;
using UnityEngine;

namespace Runtime.Gameplay.Arrow.ArrowView
{
    public class ArrowView : MonoBehaviour, IArrow
    {
        [field: SerializeField]
        public ColorType ColorType { get; set; }
        
        [field: SerializeField]
        public DirectionType DirectionType { get; set; }
    }
}