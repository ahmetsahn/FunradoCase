using System;
using Runtime.Enums;
using UnityEngine;

namespace Runtime.Utilities.Extensions
{
    public static class DirectionTypeExtensions
    {
        public static Vector3 GetNewRayDirection(this DirectionType directionType)
        {
            return directionType switch
            {
                DirectionType.Forward => Vector3.forward,
                DirectionType.Back => Vector3.back,
                DirectionType.Left => Vector3.left,
                DirectionType.Right => Vector3.right,
                _ => throw new ArgumentOutOfRangeException(nameof(directionType), directionType, null)
            };
        }
    }
}