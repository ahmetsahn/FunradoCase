using System;
using System.Linq;
using DG.Tweening;
using Runtime.Core.Interface;
using Runtime.Signal;
using UnityEngine;
using Zenject;

namespace Runtime.Gameplay.Frog.View
{
    public class FrogView : MonoBehaviour
    {
        public event Action OnClick;
        
        public Action OnTongueAnimationStart;
        
        public Action OnTongueAnimationEnd;
        
        private Transform _cellTransform;

        private void Awake()
        {
            FindGroundTransform();
        }
        
        private void FindGroundTransform()
        {
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1))
            {
                _cellTransform = hit.transform;
            }
        }

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
        
        public void ScaleDownWithAnimation()
        {
            transform.DOScale(0, 0.1f).SetEase(Ease.Linear);
        }
        
        public void ScaleDownCell()
        {
            _cellTransform.DOScale(0, 0.1f).SetEase(Ease.Linear).OnComplete(() =>
            {
                Destroy(_cellTransform.gameObject);
            });
        }
    }
}
