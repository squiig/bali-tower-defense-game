using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Game.SplineSystem
{
    /// <summary>Draws the default inspector list of the bezier spline data object, but will make sure the user can't interact with it.</summary>
    [CustomEditor(typeof(BezierSplineDataObject))]
    public class BezierSplineDataEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginDisabledGroup(true);
            base.OnInspectorGUI();
            EditorGUI.EndDisabledGroup();
        }
    }
}
