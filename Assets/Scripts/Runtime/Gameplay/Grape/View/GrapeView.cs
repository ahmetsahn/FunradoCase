using System;
using Cysharp.Threading.Tasks;
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
        
        [SerializeField]
        private MeshRenderer meshRenderer;
        
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

        public void ScaleDownWithAnimation()
        {
            transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.Linear);
        }

        public async void ScaleUpAndDown(bool freeze = false)
        {
            try
            {
                await transform.DOScale(1.2f, 0.1f).SetEase(Ease.Linear).AsyncWaitForCompletion();
            
                if (freeze)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
                }
            
                await transform.DOScale(0.8f, 0.1f).SetEase(Ease.Linear).AsyncWaitForCompletion();
            }
            
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        
        public async void ShowErrorFeedback()
        {
            try
            {
                await meshRenderer.material.DOColor(Color.red, 0.1f).AsyncWaitForCompletion();
                await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
                await meshRenderer.material.DOColor(Color.white, 0.1f).AsyncWaitForCompletion();
            }
            
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public void DestroyCell()
        {
            _cellTransform.DOScale(0, 0.5f).SetEase(Ease.Linear);
        }
    }
}