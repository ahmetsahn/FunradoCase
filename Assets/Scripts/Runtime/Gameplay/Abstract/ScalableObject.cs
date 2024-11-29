using DG.Tweening;
using Runtime.Gameplay.Cell.View;
using UnityEngine;

namespace Runtime.Gameplay.Abstract
{
    public abstract class ScalableObject : MonoBehaviour
    {
        protected CellView CellViewBelow;
        
        protected virtual void Awake()
        {
            SetScaleToZero();
            FindCellBelow();
        }

        private void Start()
        {
            ScaleUp();
        }

        protected void FindCellBelow()
        {
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1))
            {
                CellViewBelow = hit.transform.GetComponent<CellView>();
            }
        }
        
        private void SetScaleToZero()
        {
            transform.localScale = Vector3.zero;
        }

        protected virtual void ScaleUp() { }
        
        protected void AnimateScaleToZero(Transform targetTransform, float endValue, float duration, bool destroy = false)
        {
            targetTransform.DOScale(endValue, duration).SetEase(Ease.Linear).OnComplete(() =>
            {
                if (destroy)
                {
                    Destroy(CellViewBelow.gameObject);
                }
            });
        }
    }
}