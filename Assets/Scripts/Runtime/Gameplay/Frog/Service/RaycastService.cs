using System.Collections.Generic;
using System.Linq;
using Runtime.Core.Interface;
using Runtime.Enums;
using Runtime.Utilities;
using Runtime.Utilities.Extensions;
using UnityEngine;

namespace Runtime.Gameplay.Frog.Service
{
    public class RaycastService
    {
        public bool RaycastAndDetectObjects(Vector3 startPosition, Vector3 rayDirection, SplineService splineService, List<IColor> collectedObjects, ColorType colorType)
        {
            var sortedHits = Physics.RaycastAll(startPosition, rayDirection, Constants.TONGUE_MAX_RAYCAST_DISTANCE)
                .OrderBy(hit => hit.distance)
                .ToArray();

            foreach (var hit in sortedHits)
            {
                var color = hit.collider.GetComponent<IColor>();
                var arrow = hit.collider.GetComponent<IArrow>();

                if (!IsRelevantHit(color, arrow))
                {
                    if (collectedObjects.Count > 0)
                    {
                        return true;
                    }
                    break;
                }

                splineService.AddKnot(hit.GetFixedPosition());
                collectedObjects.Add(color);

                if (ShouldTerminateOnColorMismatch(color, colorType))
                {
                    return false; 
                }

                if (IsRelevantHit(arrow))
                {
                    HandleArrowHit(hit, arrow, splineService, collectedObjects, colorType);
                    break;
                }
            }

            return true; 
        }

        private bool IsRelevantHit(IColor color, IArrow arrow) => color != null || arrow != null;

        private bool IsRelevantHit(IArrow arrow) => arrow != null;

        private bool ShouldTerminateOnColorMismatch(IColor color, ColorType colorType) => color?.ColorType != colorType;

        private void HandleArrowHit(RaycastHit hit, IArrow arrow, SplineService splineService, List<IColor> collectedObjects, ColorType colorType)
        {
            var newRayDirection = arrow.DirectionType.GetNewRayDirection();
            var newStartPosition = new Vector3(hit.point.x, Constants.TONGUE_FIXED_Y_POSITION, hit.point.z);
            RaycastAndDetectObjects(newStartPosition, newRayDirection, splineService, collectedObjects, colorType);
        }
    }
}
