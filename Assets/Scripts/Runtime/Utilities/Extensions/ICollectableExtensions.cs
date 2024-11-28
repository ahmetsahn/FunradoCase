using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Runtime.Core.Interface;
using Runtime.Gameplay.Frog.Model;
using UnityEngine;

namespace Runtime.Utilities.Extensions
{
    public static class ICollectableExtensions
    {
        public static async UniTask HandleCollectableAsync(this ICollectable collectable, List<Vector3> pathPositions, float delay)
        {
            collectable.Transform.DOPath(pathPositions.ToArray(), delay, PathType.CatmullRom)
                .SetEase(Ease.Linear)
                .From();
            await UniTask.Delay(TimeSpan.FromSeconds(Constants.GRAPE_COLLECT_DELAY));
        }
    }

}