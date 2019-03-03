using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Game.SplineSystem.Editor
{
    /// <summary>Draws the spline data settings in the inspector.</summary>
    public class SplineDebugInspector<T> where T : SplineCreatorBase
    {
        private readonly T _SplineBranchCreator;
        private const int BUTTON_INTERVAL_Y = 3;

        public SplineDebugInspector(T branchCreator)
        {
            _SplineBranchCreator = branchCreator;
        }

        public void DrawSplineSettings()
        {
            GUILayout.BeginVertical("Box");
            GUILayout.Space(BUTTON_INTERVAL_Y);

            GUILayout.Label("BezierSplineData settings", EditorStyles.boldLabel);

            DrawTangentToggle();
            GUILayout.Space(1f);

            DrawBiNormalsToggle();
            GUILayout.Space(1f);

            DrawNormalsToggle();

            GUILayout.Space(BUTTON_INTERVAL_Y);
            GUILayout.EndVertical();
        }

        private void DrawTangentToggle()
        {
            Undo.RecordObject(_SplineBranchCreator, "Show_Tangents");
            bool drawTangents = GUILayout.Toggle(_SplineBranchCreator.DrawTangents, "Show tangents.");
            if (drawTangents == _SplineBranchCreator.DrawTangents) return;

            _SplineBranchCreator.DrawTangents = drawTangents;
            SceneView.RepaintAll();
            EditorUtility.SetDirty(_SplineBranchCreator);
        }

        private void DrawBiNormalsToggle()
        {
            Undo.RecordObject(_SplineBranchCreator, "Show_BiNormals");
            bool drawBiNormals = GUILayout.Toggle(_SplineBranchCreator.DrawBiNormals, "Show bi-normals.");
            if (drawBiNormals == _SplineBranchCreator.DrawBiNormals) return;

            _SplineBranchCreator.DrawBiNormals = drawBiNormals;
            SceneView.RepaintAll();
            EditorUtility.SetDirty(_SplineBranchCreator);
        }

        private void DrawNormalsToggle()
        {
            Undo.RecordObject(_SplineBranchCreator, "Show_Normals");
            bool drawNormals = GUILayout.Toggle(_SplineBranchCreator.DrawNormals, "Show normals.");
            if (drawNormals == _SplineBranchCreator.DrawNormals) return;

            _SplineBranchCreator.DrawNormals = drawNormals;
            SceneView.RepaintAll();
            EditorUtility.SetDirty(_SplineBranchCreator);
        }
    }
}