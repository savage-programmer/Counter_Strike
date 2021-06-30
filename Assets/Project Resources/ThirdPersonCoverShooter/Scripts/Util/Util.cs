using UnityEngine;

namespace ThirdPersonCover
{
    /// <summary>
    /// Utility functions.
    /// </summary>
    public sealed class Util
    {
        /// <summary>
        /// Delta of a point on AB line closest to the given point.
        /// </summary>
        public static float FindDeltaPath(Vector3 a, Vector3 b, Vector3 point)
        {
            Vector3 ap = point - a;
            Vector3 ab = b - a;
            float ab2 = ab.x * ab.x + +ab.z * ab.z;
            float ap_ab = ap.x * ab.x + ap.z * ab.z;
            float t = ap_ab / ab2;

            return t;
        }

        /// <summary>
        /// Position of a point on AB line closest to the given .point.
        /// </summary>
        public static Vector3 FindClosestToPath(Vector3 a, Vector3 b, Vector3 point)
        {
            Vector3 ap = point - a;
            Vector3 ab = b - a;
            float ab2 = ab.x * ab.x + +ab.z * ab.z;
            float ap_ab = ap.x * ab.x + ap.z * ab.z;
            float t = ap_ab / ab2;

            return a + ab * Mathf.Clamp01(t);
        }
    }
}