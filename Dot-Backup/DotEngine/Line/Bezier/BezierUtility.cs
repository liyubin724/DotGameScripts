using UnityEngine;

namespace Dot.Line.Bezier
{
    public static class BezierUtility
    {
        public static Vector3 GetPosition(Vector3 p0,Vector3 p1,float t)
        {
            t = Mathf.Clamp01(t);

            return (1 - t) * p0 + t * p1;
        }

        public static Vector3 GetPosition(Vector3 p0, Vector3 p1, Vector3 p2,float t)
        {
            t = Mathf.Clamp01(t);
            return (1 - t) * (1 - t) * p0 + 2 * t * (1 - t) * p1 + t * t * p2;
        }

        public static Vector3 GetPosition(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            t = Mathf.Clamp01(t);
            return (1 - t) * (1 - t) * (1 - t) * p0 + 3 * t * (1 - t) * (1 - t) * p1 + 3 * t * t * (1 - t) * p2 + t * t * t * p3;
        }
    }
}
