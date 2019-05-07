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

			if (target == null)
				return;

			SerializedProperty useProperty =  serializedObject.FindProperty("_Use");

			EditorGUILayout.LabelField("Used by spline manager");
			useProperty.boolValue = EditorGUILayout.Toggle(useProperty.boolValue);

			if (serializedObject.hasModifiedProperties)
				serializedObject.ApplyModifiedProperties();


			EditorGUI.BeginDisabledGroup(true);
            base.OnInspectorGUI();
            EditorGUI.EndDisabledGroup();
        }
    }
}
