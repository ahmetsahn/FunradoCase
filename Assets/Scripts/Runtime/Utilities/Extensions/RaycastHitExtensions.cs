using UnityEngine;

namespace Runtime.Utilities.Extensions
{
    public static class RaycastHitExtensions
    {
        public static Vector3 GetFixedPosition(this RaycastHit hit, Vector3 frogTransform)
        {
            return new Vector3(Mathf.Round(hit.point.x), frogTransform.y + Constants.TONGUE_START_HEIGHT, Mathf.Round(hit.point.z));
        }
    }
}