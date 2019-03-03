using Game.Utils;
using UnityEditor;
using UnityEngine;

namespace Game.SplineSystem.Editor
{
    /// <summary>Contains the base methods for the spline editor.</summary>
    [CustomEditor(typeof(SplineBranchCreator))]
    public class SplineBranchCreatorEditor : UnityEditor.Editor
    {
        private SerializedObject _SerializedSplineCreator;
        private SerializedProperty _BezierSplineData;
        private SplineBranchCreator _SplineBranchCreator;
        private BezierSplineDataObject _SplineDataObject;

        private const int BUTTON_HEIGHT = 20;
        private const int BUTTON_INTERVAL_Y = 3;
        private readonly string[] _SplineModes = { "Free", "Mirrored" };

        public void OnEnable()
        {
            Undo.undoRedoPerformed += UndoCallback;
            _SplineBranchCreator = target as SplineBranchCreator;
            _SerializedSplineCreator = serializedObject;
            OnSplineObjectReferenceChanged();
        }

        public void OnDisable()
        {
            Undo.undoRedoPerformed -= UndoCallback;
        }

        public override void OnInspectorGUI()
        {
            DrawSplineObjectField();

            if (_SplineBranchCreator.BezierSplineData == null) return;
            if (_SplineBranchCreator.BezierSplineData.PointCount <= 0) return;

            DrawSplineCustomizationSettings();
            DrawSplineSettings();
        }

        private void UndoCallback()
        {
            if (_SplineBranchCreator.SelectedPointIndex > _SplineDataObject.PointCount - 3)
                _SplineBranchCreator.SelectedPointIndex -= 3;
            if (_SplineBranchCreator.SelectedPointIndex < 0)
                _SplineBranchCreator.SelectedPointIndex = 0;
        }

        private void OnSplineObjectReferenceChanged()
        {
            _BezierSplineData = _SerializedSplineCreator.FindProperty("_BezierSplineData");
            _SplineDataObject = _SplineBranchCreator.BezierSplineData;
            _SplineBranchCreator.SelectedPointIndex = 0;

            if (_SplineBranchCreator.BezierSplineData == null) return;
            if (_SplineBranchCreator.BezierSplineData.SegmentCount > 0) return;
            _SplineBranchCreator.ResetSpline();
        }

        private void DrawSplineObjectField()
        {
            _SerializedSplineCreator?.Update();

            GUILayout.BeginVertical("box");
            GUILayout.Label("Current spline", EditorStyles.boldLabel);
            _BezierSplineData.objectReferenceValue = EditorGUILayout.ObjectField(
                label: "Spline",
                obj: _BezierSplineData.objectReferenceValue, 
                objType: typeof(BezierSplineDataObject), 
                allowSceneObjects: true);
            GUILayout.EndHorizontal();

            if (_SerializedSplineCreator == null || !_SerializedSplineCreator.hasModifiedProperties) return;
            _SerializedSplineCreator.ApplyModifiedProperties();
            OnSplineObjectReferenceChanged();

            if (_BezierSplineData.objectReferenceValue == null) return;
            SceneView.RepaintAll();
            EditorUtility.SetDirty(_SplineBranchCreator);
        }

        private void DrawSplineCustomizationSettings()
        {
            GUILayout.BeginVertical("Box");
            GUILayout.Space(BUTTON_INTERVAL_Y);

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

            GUILayout.Space(BUTTON_INTERVAL_Y);
            GUILayout.EndVertical();
        }

        private void DrawBezierSplineModeToolBar()
        {
            Undo.RecordObject(_SplineBranchCreator, "Changed_spline_mode");
            int splineMode = GUILayout.Toolbar((int)_SplineBranchCreator.DrawMode, _SplineModes, GUILayout.Height(BUTTON_HEIGHT + 5));
            if ((int)_SplineBranchCreator.DrawMode == splineMode) return;

            _SplineBranchCreator.DrawMode = (SplineBranchCreator.SplineMode)splineMode;
            SceneView.RepaintAll();
            EditorUtility.SetDirty(_SplineBranchCreator);
        }

        private void OnAddButtonPressed()
        {
            Undo.RecordObject(_SplineDataObject, "Add_segment");
            Vector3 direction = (_SplineDataObject[_SplineDataObject.PointCount - 1] - _SplineDataObject[_SplineDataObject.PointCount - 3]).normalized;
            float deltaLastSegment = (_SplineDataObject[_SplineDataObject.PointCount - 1] - _SplineDataObject[_SplineDataObject.PointCount - 3]).magnitude;

            _SplineDataObject.AddSegment(_SplineDataObject[_SplineDataObject.PointCount - 1] + direction * deltaLastSegment);
            SceneView.RepaintAll();
            EditorUtility.SetDirty(_SplineBranchCreator);
        }

        private void OnRemoveButtonPressed()
        {
            Undo.RecordObject(_SplineDataObject, "Remove_segment");
            _SplineDataObject.RemoveSegment(_SplineBranchCreator.SelectedPointIndex);

            if (_SplineBranchCreator.SelectedPointIndex != _SplineDataObject.PointCount - 1 && _SplineBranchCreator.SelectedPointIndex != 0)
                _SplineBranchCreator.SelectedPointIndex -= 3;
            else if (_SplineBranchCreator.SelectedPointIndex == 0 && !_SplineDataObject.IsClosed)
                _SplineBranchCreator.SelectedPointIndex += 3;

            SceneView.RepaintAll();
            EditorUtility.SetDirty(_SplineDataObject);
        }

        private void OnResetButtonPressed()
        {
            Undo.RecordObject(_SplineDataObject, "Reset_spline");
            _SplineBranchCreator.ResetSpline();
            _SplineBranchCreator.SelectedPointIndex = 0;
            SceneView.RepaintAll();
            EditorUtility.SetDirty(_SplineDataObject);
        }

        private void OnInsertButtonPressed()
        {
            //Last point when open
            if (_SplineBranchCreator.SelectedPointIndex / 3 == _SplineDataObject.SegmentCount && !_SplineDataObject.IsClosed) 
            {
                Undo.RecordObject(_SplineDataObject, "Add_segment");
                _SplineDataObject.AddSegment(
                    _SplineDataObject[_SplineDataObject.PointCount - 1] + 
                    (_SplineDataObject[_SplineDataObject.PointCount - 1] - _SplineDataObject[_SplineDataObject.PointCount - 3]).normalized * 
                    (_SplineDataObject[_SplineDataObject.PointCount - 1] - _SplineDataObject[_SplineDataObject.PointCount - 3]).magnitude);
            }
            else
            {
                Undo.RecordObject(_SplineDataObject, "Insert_segment");
                if (_SplineDataObject.IsClosed && _SplineBranchCreator.SelectedPointIndex / 3 + 1 == _SplineDataObject.SegmentCount)
                {
                    //Last point when closed
                    _SplineDataObject.InsertSegment(_SplineBranchCreator.SelectedPointIndex,
                        Bezier3DUtility.CubicCurveVector3(
                            a: _SplineDataObject[_SplineBranchCreator.SelectedPointIndex],
                            b: _SplineDataObject[_SplineBranchCreator.SelectedPointIndex + 1],
                            c: _SplineDataObject[_SplineBranchCreator.SelectedPointIndex + 2],
                            d: _SplineDataObject[0], 0.5f));
                }
                else
                {
                    _SplineDataObject.InsertSegment(_SplineBranchCreator.SelectedPointIndex,
                        Bezier3DUtility.CubicCurveVector3(
                            a: _SplineDataObject[_SplineBranchCreator.SelectedPointIndex],
                            b: _SplineDataObject[_SplineBranchCreator.SelectedPointIndex + 1],
                            c: _SplineDataObject[_SplineBranchCreator.SelectedPointIndex + 2],
                            d: _SplineDataObject[_SplineBranchCreator.SelectedPointIndex + 3], 0.5f));
                }
            }
            SceneView.RepaintAll();
            EditorUtility.SetDirty(_SplineDataObject);
        }

        private void DrawSplineSettings()
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

            if(GUILayout.Button("Center Spline Pivot"))
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
