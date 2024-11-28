using DG.Tweening;
using UnityEngine;

namespace Runtime.Gameplay.Abstract
{
    public abstract class ScalableObject : MonoBehaviour
    {
        protected Transform CellTransform;
        protected virtual void Awake()
        {
            SetScaleToZero();
            FindGroundTransform();
        }

        private void Start()
        {
            ScaleUp();
        }

        private void FindGroundTransform()
        {
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1))
            {
                CellTransform = hit.transform;
            }
        }
        
        private void SetScaleToZero()
        {
            transform.localScale = Vector3.zero;
        }

        protected virtual void ScaleUp()
        {
            
        }
        
        protected void AnimateScaleToZero(Transform targetTransform, float endValue, float duration)
        {
            targetTransform.DOScale(endValue, duration).SetEase(Ease.Linear);
        }
    }
}