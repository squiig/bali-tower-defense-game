using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.BackEnd.Utility
{
    /// <summary>
    /// Holds helper methods for bezier calculations.
    /// </summary>
    public static class Bezier
    {
        //Curves.
        public static Vector3 QuadraticCurveVector3(Vector3 a, Vector3 b, Vector3 c, float t)
        {
            t = Mathf.Clamp01(t);

            Vector3 d = Vector3.Lerp(a, b, t);
            Vector3 e = Vector3.Lerp(b, c, t);

            return Vector3.Lerp(d, e, t);
        }
        public static Vector3 CubicCurveVector3(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
        {
            t = Mathf.Clamp01(t);

            Vector3 e = QuadraticCurveVector3(a, b, c, t);
            Vector3 f = QuadraticCurveVector3(b, c, d, t);

            return Vector3.Lerp(e, f, t);
        }

        //Curve lengths.
        public static float Get3DCurveLength(Vector3 startPoint, Vector3 controlStart, Vector3 controlEnd, Vector3 endPoint)
        {
            float anchorPointDistance = Vector3.Distance(startPoint, endPoint);
            float bezierCurveNet = Vector3.Distance(startPoint, controlStart) + Vector3.Distance(controlStart, controlEnd) + Vector3.Distance(controlEnd, endPoint);

            return anchorPointDistance + bezierCurveNet / 2f;
        }

        //Tangents.
        public static Vector3 GetTangent(Vector3 startSplinePoint, Vector3 endSplinePoint)
        {
            return (startSplinePoint - endSplinePoint).normalized;
        }
        public static Vector3 GetTangent(Vector3 start, Vector3 firstControl, Vector3 secondControl, Vector3 end, float t)
        {
            t = Mathf.Clamp01(t);
            float omt = 1f - t;
            float omt2 = omt * omt;
            float t2 = t * t;
            Vector3 tangent =
                start * (-omt2) +
                firstControl * (3 * omt2 - 2 * omt) +
                secondControl * (-3 * t2 + 2 * t) +
                end * (t2);
            return tangent.normalized;
        }

        //Binormals.
        public static Vector3 GetBiNormal(Vector3 tangent, Vector3 up)
        {
            return Vector3.Cross(up, tangent).normalized;
        }
        public static Vector3 GetBiNormal(Vector3 start, Vector3 firstControl, Vector3 secondControl, Vector3 end, float t, Vector3 up)
        {
            Vector3 tangent = GetTangent(start, firstControl, secondControl, end, t);
            Vector3 biNormal = Vector3.Cross(up, tangent).normalized;

            return biNormal;
        }

        //Normals.
        public static Vector3 GetNormal3D(Vector3 tangent, Vector3 biNormal)
        {
            return Vector3.Cross(tangent, biNormal).normalized;
        }
        public static Vector3 GetNormal3D(Vector3 start, Vector3 firstControl, Vector3 secondControl, Vector3 end, float t, Vector3 up)
        {
            Vector3 tangent = GetTangent(start, firstControl, secondControl, end, t);
            Vector3 biNormal = GetBiNormal(start, firstControl, secondControl, end, t, up);

            return Vector3.Cross(tangent, biNormal);
        }
    }
}