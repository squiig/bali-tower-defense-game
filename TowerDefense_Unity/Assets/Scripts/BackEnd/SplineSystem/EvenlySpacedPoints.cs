using UnityEngine;

namespace Game.SplineSystem
{
    /// <summary>
    /// Adds the functionality to evenly space points across the spline.
    /// </summary>
    [RequireComponent(typeof(SplineBranchCreator))]
    public class EvenlySpacedPoints : MonoBehaviour
    {
        [SerializeField] private float _Spacing = 0.1f, _Resolution = 1f;

        private void Start()
        {
            Vector3[] points = GetComponent<SplineBranchCreator>().BezierSplineData.CalculateEvenlySpacedPoints(_Spacing, _Resolution);
            foreach (Vector3 point in points)
            {
                GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sphere.transform.position = point;
                sphere.transform.localScale = Vector3.one * _Spacing * 0.5f;
                sphere.transform.parent = transform;
            }
        }
    }
}
