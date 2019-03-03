using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Game.SplineSystem.Editor
{
    /// <summary>Renders a spline on creation.</summary>
    public class SplineRenderer
    {
        private BezierSplineDataObject _Branch;

        public SplineRenderer(BezierSplineDataObject branch)
        {
            _Branch = branch;
        }


    }
}
