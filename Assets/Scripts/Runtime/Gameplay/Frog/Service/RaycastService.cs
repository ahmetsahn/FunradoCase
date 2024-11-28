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
        public bool RaycastAndDetectObjects(Vector3 startPosition, Vector3 rayDirection, SplineService splineService, List<IInteractable> interactedObjects, ColorType frogColorType)
        {
            var sortedHits = Physics.RaycastAll(startPosition, rayDirection, Constants.TONGUE_MAX_RAYCAST_DISTANCE)
                .OrderBy(hit => hit.distance)
                .ToArray();

            foreach (var hit in sortedHits)
            {
                var interactable = hit.collider.GetComponent<IInteractable>();
                var arrow = hit.collider.GetComponent<IArrow>();

                if (interactable == null)
                {
                    if (interactedObjects.Count > 0)
                    {
                        return true;
                    }

                    break; 
                }
                
                splineService.AddKnot(hit.GetFixedPosition());
                interactedObjects.Add(interactable);
                
                if (interactable.ColorType != frogColorType)
                {
                    return false; 
                }
                
                if (arrow != null)
                {
                    var newRayDirection = arrow.DirectionType.GetNewRayDirection();
                    var newStartPosition = new Vector3(hit.point.x, Constants.TONGUE_FIXED_Y_POSITION, hit.point.z);
                    RaycastAndDetectObjects(newStartPosition, newRayDirection, splineService, interactedObjects, frogColorType);
                    break;
                }
            }

            return true; 
        }
    }
}
