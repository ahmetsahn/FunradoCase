using System;
using System.Collections.Generic;
using DG.Tweening;
using Runtime.Utilities;
using Runtime.Utilities.Extensions;
using UnityEngine;
using UnityEngine.Splines;
using Object = UnityEngine.Object;

namespace Runtime.Gameplay.Frog.Service
{
    public class SplineService
    {
        public SplineContainer SplineContainer { get; private set; }
        
        private readonly SplineExtrude _splineExtrude;
        
        public SplineService(SplineServiceConfig config)
        {
            SplineContainer = Object.Instantiate(config.TongueSplinePrefab).GetComponent<SplineContainer>();
            _splineExtrude = SplineContainer.GetComponent<SplineExtrude>();
            MeshFilter meshFilter = SplineContainer.GetComponent<MeshFilter>();
            meshFilter.mesh = Object.Instantiate(meshFilter.sharedMesh);
        }

        public void AddKnot(Vector3 newPosition)
        {
            Spline spline = SplineContainer.Spline;
            BezierKnot newKnot = new BezierKnot(newPosition);
            spline.Add(newKnot);
            spline.SetLastKnotLinear();
        }

        public void AnimateSplineRange(float targetValue, float animationDuration)
        {
            float rangeValue = _splineExtrude.Range.y;

            DOTween.To(() => rangeValue, x => rangeValue = x, targetValue, animationDuration)
                .OnUpdate(() =>
                {
                    _splineExtrude.Range = new Vector2(0, rangeValue);
                }).SetEase(Ease.Linear);
        }

        public List<Vector3> GetSplinePointsReverse()
        {
            int pointCount = SplineContainer.Spline.Count;
            List<Vector3> positions = new List<Vector3>(pointCount);

            for (int i = pointCount - 1; i >= 0; i--)
            {
                var newPos = new Vector3(
                    SplineContainer.Spline[i].Position.x,
                    Constants.GRAPE_FIXED_Y_POSITION,
                    SplineContainer.Spline[i].Position.z
                );
                positions.Add(newPos);
            }

            return positions;
        }

        public void ResetSpline()
        {
            SplineContainer.Spline.Clear();
        }
    }
    
    [Serializable]
    public struct SplineServiceConfig
    {
        public GameObject TongueSplinePrefab;
    }
}