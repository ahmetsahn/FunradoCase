using UnityEngine;

namespace Runtime.Utilities.Extensions
{
    public static class RaycastHitExtensions
    {
        public static Vector3 GetFixedPosition(this RaycastHit hit, Vector3 tonguePosition)
        {
            return new Vector3(Mathf.Round(hit.point.x), tonguePosition.y, Mathf.Round(hit.point.z));
        }
    }
}