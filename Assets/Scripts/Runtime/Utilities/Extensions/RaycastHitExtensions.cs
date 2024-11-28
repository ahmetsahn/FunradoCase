using Runtime.Gameplay.Frog.Model;
using UnityEngine;

namespace Runtime.Utilities.Extensions
{
    public static class RaycastHitExtensions
    {
        public static Vector3 GetFixedPosition(this RaycastHit hit)
        {
            return new Vector3(Mathf.Round(hit.point.x), Constants.GRAPE_FIXED_Y_POSITION, Mathf.Round(hit.point.z));
        }
    }
}