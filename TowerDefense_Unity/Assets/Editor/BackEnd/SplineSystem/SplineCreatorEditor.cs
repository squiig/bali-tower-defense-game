using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Game.SplineSystem.Editor
{
    /// <summary>Draws the custom inspector of the SplineCreatorEditor and handles multi-object drawing.</summary>
    [CustomEditor(typeof(SplineCreator)), CanEditMultipleObjects]
    public class SplineCreatorEditor : UnityEditor.Editor
    {
        private SerializedObject _SplineCreator;


        public void OnEnable()
        {
            _SplineCreator = serializedObject;

        }

        public override void OnInspectorGUI()
        {
            DrawSplineBranchEditButtons();
        }

        private void DrawSplineBranchEditButtons()
        {
            //GUILayout.BeginVertical("box");
            //if (GUILayout.Button("Combine spline"))
            //    OnSplineSplitButtonPressed();
            //GUILayout.EndVertical();
        }
    }
}
