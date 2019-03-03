using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Game.SplineSystem.Editor
{
    /// <summary>Draws the spline data settings in the inspector.</summary>
    public class SplineSettings
    {
        private readonly BezierSplineDataObject _SplineDataObject;
        private readonly SplineBranchCreator _SplineBranchCreator;
        private const int BUTTON_INTERVAL_Y = 3;
        private const int BUTTON_HEIGHT = 20;

        public SplineSettings(SplineBranchCreator branchCreator)
        {
            _SplineBranchCreator = branchCreator;
            _SplineDataObject = branchCreator.BezierSplineData;
        }

        public void DrawSplineSettings()
        {
            GUILayout.BeginVertical("Box");
            GUILayout.Space(BUTTON_INTERVAL_Y);

            GUILayout.Label("BezierSplineData settings", EditorStyles.boldLabel);

            DrawIsClosedToggle();
            GUILayout.Space(1f);

            DrawTangentToggle();
            GUILayout.Space(1f);

            DrawBiNormalsToggle();
            GUILayout.Space(1f);

            DrawNormalsToggle();
            GUILayout.Space(1f);

            if (GUILayout.Button("Center Spline Pivot"))
                OnCenterPositionHandleButtonPressed();

            GUILayout.Space(BUTTON_INTERVAL_Y);
            GUILayout.EndVertical();
        }

        private void DrawIsClosedToggle()
        {
            Undo.RecordObject(_SplineDataObject, "Toggle_Closed");
            bool isClosed = GUILayout.Toggle(_SplineDataObject.IsClosed, "Is closed", GUILayout.Height(BUTTON_HEIGHT));
            if (isClosed == _SplineDataObject.IsClosed) return;

            _SplineDataObject.ToggleClosed(!_SplineDataObject.IsClosed);
            SceneView.RepaintAll();
            EditorUtility.SetDirty(_SplineBranchCreator);
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

        private void OnCenterPositionHandleButtonPressed()
        {
            Undo.RecordObject(_SplineBranchCreator, "Center_Spline_Position_Handle");
            _SplineBranchCreator.transform.position = _SplineDataObject.GetCenterPoint();
            SceneView.RepaintAll();
            EditorUtility.SetDirty(_SplineBranchCreator);
        }
    }
}