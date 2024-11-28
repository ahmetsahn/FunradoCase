using UnityEngine.Splines;

namespace Runtime.Utilities.Extensions
{
    public static class SplineExtensions
    {
        public static void SetLastKnotLinear(this Spline spline)
        {
            int knotIndex = spline.Count - 1;

            if (knotIndex <= 0)
            {
                return;
            }
            
            spline.SetTangentMode(knotIndex - 1, TangentMode.Linear);
            spline.SetTangentMode(knotIndex, TangentMode.Linear);
        }

    }
}