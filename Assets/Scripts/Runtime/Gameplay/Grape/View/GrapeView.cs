using DG.Tweening;
using Runtime.Core.Interface;
using Runtime.Enums;
using UnityEngine;

namespace Runtime.Gameplay.Grape.View
{
    public class GrapeView : MonoBehaviour, ICollectable
    {
        [field: SerializeField]
        public ColorType ColorType { get; set; }
        public Transform Transform => transform;
        
        public void ScaleDownWithAnimation()
        {
            transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.Linear);
        }
    }
}