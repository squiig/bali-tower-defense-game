using UnityEngine;

namespace Game.BackEnd.SplineSystem
{
    /// <summary>
    /// Adds the functionality to evenly space points across the spline.
    /// </summary>
    public class EvenlySpacedPoints : MonoBehaviour
    {
        [SerializeField] private float _Spacing = 0.1f, _Resolution = 1f;

        private void Start()
        {
            Vector3[] points = FindObjectOfType<SplineCreator>().BezierSplineData.CalculateEvenlySpacedPoints(_Spacing, _Resolution);
            foreach (Vector3 point in points)
            {
                GameObject g = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                g.transform.position = point;
                g.transform.localScale = Vector3.one * _Spacing * 0.5f;
            }
        }
    }
}
