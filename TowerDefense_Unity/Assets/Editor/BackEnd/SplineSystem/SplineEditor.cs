using System.Collections;
using System.Collections.Generic;
using Game.Utils;
using UnityEditor;
using UnityEngine;

namespace Game.SplineSystem.Editor
{
    /// <summary>Contains the base methods for the spline editor.</summary>
    [CustomEditor(typeof(SplineCreator)), CanEditMultipleObjects]
    public class SplineEditor : UnityEditor.Editor
    {

    #region Core

        protected SerializedObject _SerializedSplineCreator;
        protected SerializedProperty _BezierSplineData;
        protected SplineCreator _SplineCreator;
        protected BezierSplineDataObject _SplineDataObject;

        public void OnEnable()
        {
            Undo.undoRedoPerformed += UndoCallback;
            _SplineCreator = target as SplineCreator;
            _SerializedSplineCreator = serializedObject;
            OnSelectedSplineChanged();
        }

        public void OnDisable()
        {
            Undo.undoRedoPerformed -= UndoCallback;
        }

        private void UndoCallback()
        {
            if (_SplineCreator.SelectedPointIndex > _SplineDataObject.PointCount - 3)
                _SplineCreator.SelectedPointIndex -= 3;
            if (_SplineCreator.SelectedPointIndex <= 0)
                _SplineCreator.SelectedPointIndex += 3;
            UpdatePositionHandle();
        }

        #endregion

        #region Inspector methods

        private const int BUTTON_HEIGHT = 20;
        private const int BUTTON_INTERVAL_Y = 3;
        private readonly string[] _SplineModes = { "Free", "Mirrored" };

        #region Inspector Core

        public override void OnInspectorGUI()
        {
            DrawSplineObjectField();
            if (_SplineCreator.BezierSplineData == null) return;
            if (_SplineCreator.BezierSplineData.PointCount <= 0) return;

            DrawCustomInspector();
        }

        private void OnSelectedSplineChanged()
        {
            _BezierSplineData = _SerializedSplineCreator.FindProperty("_BezierSplineData");
            _SplineDataObject = _SplineCreator.BezierSplineData;

            if (_SplineCreator.BezierSplineData == null) return;
            if (_SplineCreator.BezierSplineData.PointCount > 0) return;
            _SplineCreator.ResetSpline();
        }

        private void DrawSplineObjectField()
        {
            _SerializedSplineCreator.Update();

            _BezierSplineData.objectReferenceValue = EditorGUILayout.ObjectField(
                label: "Spline",
                obj: _BezierSplineData.objectReferenceValue, 
                objType: typeof(BezierSplineDataObject), 
                allowSceneObjects: true);

            if (!_SerializedSplineCreator.hasModifiedProperties) return;
            _SerializedSplineCreator.ApplyModifiedProperties();
            OnSelectedSplineChanged();
        }

        private void DrawCustomInspector()
        {
            GUILayout.BeginVertical("Box");
            GUILayout.Space(BUTTON_INTERVAL_Y);

            DrawSplineListCustomizationSettings();
            GUILayout.Space(5f);
            DrawSplineSettings();

            GUILayout.Space(BUTTON_INTERVAL_Y);
            GUILayout.EndVertical();
        }

        #endregion

        #region Spline Edit Settings

        private void DrawSplineListCustomizationSettings()
        {
            GUILayout.Label("BezierSplineData edit settings", EditorStyles.boldLabel);
            DrawBezierSplineModeToolBar();
            GUILayout.Space(BUTTON_INTERVAL_Y);

            if (GUILayout.Button("Add", GUILayout.Height(BUTTON_HEIGHT)) && !_SplineDataObject.IsClosed)
                OnAddButtonPressed();
            GUILayout.Space(1f);
            if (GUILayout.Button("Remove", GUILayout.Height(BUTTON_HEIGHT)))
                OnRemoveButtonPressed();

            GUILayout.Space(1f);

            if (GUILayout.Button("Reset", GUILayout.Height(BUTTON_HEIGHT)))
                OnResetButtonPressed();
            GUILayout.Space(1f);
            if (GUILayout.Button("Insert", GUILayout.Height(BUTTON_HEIGHT)))
                OnInsertButtonPressed();
        }

        private void DrawBezierSplineModeToolBar()
        {
            int splineMode = GUILayout.Toolbar((int)_SplineCreator.DrawMode, _SplineModes, GUILayout.Height(BUTTON_HEIGHT + 5));
            if ((int)_SplineCreator.DrawMode == splineMode) return;

            Undo.RecordObject(_SplineCreator, "Changed_spline_mode");
            _SplineCreator.DrawMode = (SplineCreator.SplineMode)splineMode;
            SceneView.RepaintAll();
            EditorUtility.SetDirty(_SplineCreator);
        }

        private void OnAddButtonPressed()
        {
            Vector3 direction = (_SplineDataObject[_SplineDataObject.PointCount - 1] - _SplineDataObject[_SplineDataObject.PointCount - 3]).normalized;
            float deltaLastSegment = (_SplineDataObject[_SplineDataObject.PointCount - 1] - _SplineDataObject[_SplineDataObject.PointCount - 3]).magnitude;

            Undo.RecordObject(_SplineDataObject, "Add_segment");
            _SplineDataObject.AddSegment(_SplineDataObject[_SplineDataObject.PointCount - 1] + direction * deltaLastSegment);
            SceneView.RepaintAll();
            EditorUtility.SetDirty(_SplineCreator);
            UpdatePositionHandle();
        }

        private void OnRemoveButtonPressed()
        {
            Undo.RecordObject(_SplineDataObject, "Remove_segment");
            _SplineDataObject.RemoveSegment(_SplineCreator.SelectedPointIndex);

            if (_SplineCreator.SelectedPointIndex != _SplineDataObject.PointCount - 1 && _SplineCreator.SelectedPointIndex != 0)
                _SplineCreator.SelectedPointIndex -= 3;
            else if (_SplineCreator.SelectedPointIndex == 0 && !_SplineDataObject.IsClosed)
                _SplineCreator.SelectedPointIndex += 3;

            SceneView.RepaintAll();
            EditorUtility.SetDirty(_SplineDataObject);
            UpdatePositionHandle();
        }

        private void OnResetButtonPressed()
        {
            Undo.RecordObject(_SplineDataObject, "Reset_spline");
            _SplineCreator.ResetSpline();
            _SplineDataObject = _SplineCreator.BezierSplineData;
            _SplineCreator.SelectedPointIndex = 0;
            SceneView.RepaintAll();
            EditorUtility.SetDirty(_SplineDataObject);
        }

        private void OnInsertButtonPressed()
        {
            if (_SplineCreator.SelectedPointIndex / 3 == _SplineDataObject.SegmentCount && !_SplineDataObject.IsClosed) //Last point when open
            {
                Undo.RecordObject(_SplineDataObject, "Add_segment");
                _SplineDataObject.AddSegment(_SplineDataObject[_SplineDataObject.PointCount - 1] + (_SplineDataObject[_SplineDataObject.PointCount - 1] - _SplineDataObject[_SplineDataObject.PointCount - 3]).normalized * (_SplineDataObject[_SplineDataObject.PointCount - 1] - _SplineDataObject[_SplineDataObject.PointCount - 3]).magnitude);
            }
            else
            {
                Undo.RecordObject(_SplineDataObject, "Insert_segment");
                if (_SplineDataObject.IsClosed && _SplineCreator.SelectedPointIndex / 3 + 1 == _SplineDataObject.SegmentCount)
                {
                    //Last point when closed
                    _SplineDataObject.InsertSegment(_SplineCreator.SelectedPointIndex,
                        Bezier3DUtility.CubicCurveVector3(
                            _SplineDataObject[_SplineCreator.SelectedPointIndex],
                            _SplineDataObject[_SplineCreator.SelectedPointIndex + 1],
                            _SplineDataObject[_SplineCreator.SelectedPointIndex + 2],
                            _SplineDataObject[0], 0.5f));
                }
                else
                {
                    _SplineDataObject.InsertSegment(_SplineCreator.SelectedPointIndex,
                        Bezier3DUtility.CubicCurveVector3(
                            _SplineDataObject[_SplineCreator.SelectedPointIndex],
                            _SplineDataObject[_SplineCreator.SelectedPointIndex + 1],
                            _SplineDataObject[_SplineCreator.SelectedPointIndex + 2],
                            _SplineDataObject[_SplineCreator.SelectedPointIndex + 3], 0.5f));
                }
            }
            SceneView.RepaintAll();
            EditorUtility.SetDirty(_SplineDataObject);
            UpdatePositionHandle();
        }

        #endregion

        #region Spline Data Settings

        private void DrawSplineSettings()
        {
            GUILayout.Label("BezierSplineData settings", EditorStyles.boldLabel);
            DrawIsClosedToggle();
            GUILayout.Space(1f);
            DrawTangentToggle();
            GUILayout.Space(1f);
            DrawBiNormalsToggle();
            GUILayout.Space(1f);
            DrawNormalsToggle();
            GUILayout.Space(1f);
        }

        private void DrawIsClosedToggle()
        {
            bool isClosed = GUILayout.Toggle(_SplineDataObject.IsClosed, "Is closed", GUILayout.Height(BUTTON_HEIGHT));
            if (isClosed == _SplineDataObject.IsClosed) return;

            Undo.RecordObject(_SplineDataObject, "Toggle_Closed");
            _SplineDataObject.ToggleClosed(!_SplineDataObject.IsClosed);
            SceneView.RepaintAll();
            EditorUtility.SetDirty(_SplineCreator);
        }

        private void DrawTangentToggle()
        {
            bool drawTangents = GUILayout.Toggle(_SplineCreator.DrawTangents, "Show tangents.");
            if (drawTangents == _SplineCreator.DrawTangents) return;

            Undo.RecordObject(_SplineCreator, "Show_Tangents");
            _SplineCreator.DrawTangents = drawTangents;
            SceneView.RepaintAll();
            EditorUtility.SetDirty(_SplineCreator);
        }

        private void DrawBiNormalsToggle()
        {
            bool drawBiNormals = GUILayout.Toggle(_SplineCreator.DrawBiNormals, "Show bi-normals.");
            if (drawBiNormals == _SplineCreator.DrawBiNormals) return;

            Undo.RecordObject(_SplineCreator, "Show_BiNormals");
            _SplineCreator.DrawBiNormals = drawBiNormals;
            SceneView.RepaintAll();
            EditorUtility.SetDirty(_SplineCreator);
        }

        private void DrawNormalsToggle()
        {
            bool drawNormals = GUILayout.Toggle(_SplineCreator.DrawNormals, "Show normals.");
            if (drawNormals == _SplineCreator.DrawNormals) return;

            Undo.RecordObject(_SplineCreator, "Show_Normals");
            _SplineCreator.DrawNormals = drawNormals;
            SceneView.RepaintAll();
            EditorUtility.SetDirty(_SplineCreator);
        }

        #endregion

        #endregion

        #region Scene Draw methods

        private const int CURVE_LINE_WIDTH = 3;
        private const float AMOUNT_TANGENTS_PER_CURVE = 10;

        protected void OnSceneGUI()
        {
            if (_SplineCreator.BezierSplineData == null) return;
            DrawSegments();
            DrawSplineHandles();
        }

        private void DrawSegments()
        {
            for (int i = 0; i < _SplineDataObject.SegmentCount; i++)
            {
                Vector3[] segment = _SplineDataObject.GetSegmentPoints(i);

                Handles.color = Color.yellow;
                if (segment[0] == _SplineDataObject[_SplineCreator.SelectedPointIndex]) Handles.DrawLine(segment[0], segment[1]);
                if (segment[3] == _SplineDataObject[_SplineCreator.SelectedPointIndex]) Handles.DrawLine(segment[2], segment[3]);

                Handles.DrawBezier(segment[0], segment[3], segment[1], segment[2], Color.magenta, null, CURVE_LINE_WIDTH);

                if (!_SplineCreator.DrawTangents && !_SplineCreator.DrawNormals && !_SplineCreator.DrawBiNormals) continue;
                for (float t = 0; t < 1; t += 1 / AMOUNT_TANGENTS_PER_CURVE)
                {
                    if (_SplineCreator.DrawTangents) DrawTangents(segment, t);
                    if (_SplineCreator.DrawBiNormals) DrawBiNormals(segment, t);
                    if (_SplineCreator.DrawNormals) DrawNormals(segment, t);
                }
            }
        }

        private void DrawTangents(Vector3[] segment, float t)
        {
            Vector3 startPoint = Bezier3DUtility.CubicCurveVector3(segment[0], segment[1], segment[2], segment[3], t + 0.1f);
            Vector3 tangent = Bezier3DUtility.GetTangent(startPoint, Bezier3DUtility.CubicCurveVector3(segment[0], segment[1], segment[2], segment[3], t));
            Handles.color = Color.green;
            Handles.DrawLine(startPoint, startPoint + tangent);
        }

        private void DrawBiNormals(Vector3[] segment, float t)
        {
            Vector3 startPoint = Bezier3DUtility.CubicCurveVector3(segment[0], segment[1], segment[2], segment[3], t + 0.1f);
            Vector3 tangent = Bezier3DUtility.GetTangent(startPoint, Bezier3DUtility.CubicCurveVector3(segment[0], segment[1], segment[2], segment[3], t));
            Vector3 biNormal = Vector3.Cross(Vector3.up, tangent).normalized;
            Handles.color = Color.red;
            Handles.DrawLine(startPoint, startPoint + biNormal);
        }

        private void DrawNormals(Vector3[] segment, float t)
        {
            Vector3 startPoint = Bezier3DUtility.CubicCurveVector3(segment[0], segment[1], segment[2], segment[3], t + 0.1f);
            Vector3 tangent = Bezier3DUtility.GetTangent(startPoint, Bezier3DUtility.CubicCurveVector3(segment[0], segment[1], segment[2], segment[3], t));
            Vector3 biNormal = Vector3.Cross(Vector3.up, tangent).normalized;
            Vector3 normal = Vector3.Cross(tangent, biNormal).normalized;
            Handles.color = Color.cyan;
            Handles.DrawLine(startPoint, startPoint + normal);
        }

        private void DrawSplineHandles()
        {
            for (int i = 0; i < _SplineDataObject.PointCount; i += 3)
            {
                Handles.color = Color.red;
                float handleSize = HandleUtility.GetHandleSize(_SplineDataObject[i]);
                if (Handles.Button(_SplineDataObject[i], Quaternion.identity, handleSize * 0.12f, handleSize * 0.16f, Handles.SphereHandleCap))
                    _SplineCreator.SelectedPointIndex = i;
            }

            DrawSelectedHandle(_SplineCreator.SelectedPointIndex);
            if (_SplineCreator.SelectedPointIndex + 1 < _SplineDataObject.PointCount) DrawSelectedHandle(_SplineCreator.SelectedPointIndex + 1);
            if (_SplineCreator.SelectedPointIndex - 1 >= 0) DrawSelectedHandle(_SplineCreator.SelectedPointIndex - 1);
            if (_SplineDataObject.IsClosed && _SplineCreator.SelectedPointIndex == 0) DrawSelectedHandle(_SplineDataObject.PointCount - 1);
        }

        private void DrawSelectedHandle(int index, bool recordAction = true)
        {
            Vector3 point = Handles.DoPositionHandle(_SplineDataObject[index], Quaternion.identity);
            if (point == _SplineDataObject[index]) return;

            if (recordAction) Undo.RecordObject(_SplineDataObject, "Move_point");
            EditorUtility.SetDirty(_SplineDataObject);
            _SplineDataObject.MovePoint(index, point, _SplineCreator.DrawMode);
            UpdatePositionHandle();
        }

        private void UpdatePositionHandle()
        {
            Vector3 totalPos = Vector3.zero;
            for (int i = 0; i < _SplineDataObject.PointCount; i += 3)
                totalPos += _SplineDataObject[i];

            _SplineCreator.transform.position = totalPos / _SplineDataObject.SegmentCount;
        }

    #endregion

    }
}
