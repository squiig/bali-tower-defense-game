using Game.Utils;
using UnityEditor;
using UnityEngine;

namespace Game.SplineSystem.Editor
{
    /// <summary>Contains the inspector and scene methods needed to draw a single spline branch, it does not support multi-object editing..</summary>
    [CustomEditor(typeof(SplineBranchCreator))]
    public class SplineBranchCreatorEditor : UnityEditor.Editor
    {
        private SerializedObject _SerializedSplineCreator;
        private SerializedProperty _BezierSplineData;
        private SplineBranchCreator _SplineBranchCreator;
        private BezierSplineDataObject _SplineDataObject;
        private SplineDebugInspector<SplineBranchCreator> _SplineDebugInspector;

        private const int BUTTON_HEIGHT = 20;
        private const int BUTTON_INTERVAL_Y = 3;
        private readonly string[] _SplineModes = { "Free", "Mirrored" };
        private const int CURVE_LINE_WIDTH = 3;
        private const float AMOUNT_TANGENTS_PER_CURVE = 10;

        public void OnEnable()
        {
            Undo.undoRedoPerformed += UndoCallback;
            _SplineBranchCreator = target as SplineBranchCreator;
            _SerializedSplineCreator = serializedObject;
            _SplineDebugInspector = new SplineDebugInspector<SplineBranchCreator>(_SplineBranchCreator);
            OnSplineObjectReferenceChanged();
        }

        public void OnDisable()
        {
            Undo.undoRedoPerformed -= UndoCallback;
        }

	    private void UndoCallback()
	    {
		    if (_SplineBranchCreator.SelectedPointIndex > _SplineDataObject.PointCount - 3)
			    _SplineBranchCreator.SelectedPointIndex -= 3;
		    if (_SplineBranchCreator.SelectedPointIndex < 0)
			    _SplineBranchCreator.SelectedPointIndex = 0;
	    }

		public override void OnInspectorGUI()
        {
            DrawSplineObjectField();

            if (_SplineBranchCreator.BezierSplineData == null) return;
            if (_SplineBranchCreator.BezierSplineData.PointCount <= 0) return;

            DrawSplineCustomizationSettings();
            _SplineDebugInspector.DrawSplineSettings();
        }

        private void OnSplineObjectReferenceChanged()
        {
            _BezierSplineData = _SerializedSplineCreator.FindProperty("_BezierSplineData");
            _SplineDataObject = _SplineBranchCreator.BezierSplineData;
            _SplineBranchCreator.SelectedPointIndex = 0;

            if (_SplineBranchCreator.BezierSplineData == null) return;
            if (_SplineBranchCreator.BezierSplineData.SegmentCount > 0) return;
            _SplineBranchCreator.Reset();
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

            GUILayout.Space(1f);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            DrawIsClosedToggle();
            GUILayout.EndHorizontal();

            if (GUILayout.Button("Center Spline Pivot"))
                OnCenterPositionHandleButtonPressed();

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
            _SplineBranchCreator.Reset();
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

        private void DrawIsClosedToggle()
        {
            Undo.RecordObject(_SplineDataObject, "Toggle_Closed");
            bool isClosed = GUILayout.Toggle(_SplineDataObject.IsClosed, "Is closed", GUILayout.Height(BUTTON_HEIGHT), GUILayout.Width(Screen.width / 2 + 25));
            if (isClosed == _SplineDataObject.IsClosed) return;

            _SplineDataObject.ToggleClosed(!_SplineDataObject.IsClosed);
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

        protected void OnSceneGUI()
        {
            if (_SplineBranchCreator.BezierSplineData == null) return;
            DrawSegments();
            DrawCurveHandles();

            if (!_SplineBranchCreator.transform.hasChanged) return;
            UpdateSplinePosition();
            _SplineBranchCreator.transform.hasChanged = false;
        }

        private void DrawSegments()
        {
            for (int i = 0; i < _SplineDataObject.SegmentCount; i++)
            {
                Vector3[] segment = _SplineDataObject.GetSegmentPoints(i);

                Handles.color = Color.yellow;
                if (segment[0] == _SplineDataObject[_SplineBranchCreator.SelectedPointIndex]) Handles.DrawLine(segment[0], segment[1]);
                if (segment[3] == _SplineDataObject[_SplineBranchCreator.SelectedPointIndex]) Handles.DrawLine(segment[2], segment[3]);

                Handles.DrawBezier(segment[0], segment[3], segment[1], segment[2], Color.magenta, null, CURVE_LINE_WIDTH);

                if (!_SplineBranchCreator.DrawTangents && !_SplineBranchCreator.DrawNormals && !_SplineBranchCreator.DrawBiNormals) continue;
                for (float t = 0; t < 1; t += 1 / AMOUNT_TANGENTS_PER_CURVE)
                {
                    if (_SplineBranchCreator.DrawTangents) DrawTangents(segment, t);
                    if (_SplineBranchCreator.DrawBiNormals) DrawBiNormals(segment, t);
                    if (_SplineBranchCreator.DrawNormals) DrawNormals(segment, t);
                }
            }
        }

        private void DrawTangents(Vector3[] segment, float t)
        {
            float tangentLength = HandleUtility.GetHandleSize(_SplineDataObject[_SplineBranchCreator.SelectedPointIndex]);
            Vector3 startPoint = Bezier3DUtility.CubicCurveVector3(segment[0], segment[1], segment[2], segment[3], t + 0.1f);
            Vector3 tangent = Bezier3DUtility.GetTangent(startPoint, Bezier3DUtility.CubicCurveVector3(segment[0], segment[1], segment[2], segment[3], t));
            Handles.color = Color.green;
            Handles.DrawLine(startPoint, startPoint + tangent * tangentLength);
        }

        private void DrawBiNormals(Vector3[] segment, float t)
        {
            float biNormalLength = HandleUtility.GetHandleSize(_SplineDataObject[_SplineBranchCreator.SelectedPointIndex]);
            Vector3 startPoint = Bezier3DUtility.CubicCurveVector3(segment[0], segment[1], segment[2], segment[3], t + 0.1f);
            Vector3 tangent = Bezier3DUtility.GetTangent(startPoint, Bezier3DUtility.CubicCurveVector3(segment[0], segment[1], segment[2], segment[3], t));
            Vector3 biNormal = Vector3.Cross(Vector3.up, tangent).normalized;
            Handles.color = Color.red;
            Handles.DrawLine(startPoint, startPoint + biNormal * biNormalLength);
        }

        private void DrawNormals(Vector3[] segment, float t)
        {
            float normalLength = HandleUtility.GetHandleSize(_SplineDataObject[_SplineBranchCreator.SelectedPointIndex]);
            Vector3 startPoint = Bezier3DUtility.CubicCurveVector3(segment[0], segment[1], segment[2], segment[3], t + 0.1f);
            Vector3 tangent = Bezier3DUtility.GetTangent(startPoint, Bezier3DUtility.CubicCurveVector3(segment[0], segment[1], segment[2], segment[3], t));
            Vector3 biNormal = Vector3.Cross(Vector3.up, tangent).normalized;
            Vector3 normal = Vector3.Cross(tangent, biNormal).normalized;
            Handles.color = Color.cyan;
            Handles.DrawLine(startPoint, startPoint + normal * normalLength);
        }

        private void DrawCurveHandles()
        {
            for (int i = 0; i < _SplineDataObject.PointCount; i += 3)
            {
                Handles.color = Color.red;
                float handleSize = HandleUtility.GetHandleSize(_SplineDataObject[i]);

                if (!Handles.Button(_SplineDataObject[i], Quaternion.identity,
                    handleSize * 0.12f, handleSize * 0.16f, Handles.SphereHandleCap))
                    continue;
                _SplineBranchCreator.SelectedPointIndex = i;
            }

            DrawSelectedHandle(_SplineBranchCreator.SelectedPointIndex);
            if (_SplineBranchCreator.SelectedPointIndex + 1 < _SplineDataObject.PointCount)
                DrawSelectedHandle(_SplineBranchCreator.SelectedPointIndex + 1);
            if (_SplineBranchCreator.SelectedPointIndex - 1 >= 0)
                DrawSelectedHandle(_SplineBranchCreator.SelectedPointIndex - 1);
            if (_SplineDataObject.IsClosed && _SplineBranchCreator.SelectedPointIndex == 0)
                DrawSelectedHandle(_SplineDataObject.PointCount - 1);
        }

        private void DrawSelectedHandle(int index, bool recordAction = true)
        {
            Vector3 point = Handles.DoPositionHandle(_SplineDataObject[index], Quaternion.identity);
            if (point == _SplineDataObject[index]) return;

            if (recordAction) Undo.RecordObject(_SplineBranchCreator, "Move_point");
            _SplineDataObject.MovePoint(index, point, _SplineBranchCreator.DrawMode);
        }

        private void UpdateSplinePosition()
        {
            Undo.RecordObject(_SplineBranchCreator, "Move_Spline");
            Undo.RecordObject(_SplineBranchCreator.BezierSplineData, "Move_Spline");

            float moveDistance = Mathf.Sign(Vector3.Distance(_SplineBranchCreator.BezierSplineData.Position, _SplineBranchCreator.transform.position));
            Vector3 distanceVector = moveDistance >= 0
                ? _SplineBranchCreator.transform.position - _SplineBranchCreator.BezierSplineData.Position
                : _SplineBranchCreator.BezierSplineData.Position - _SplineBranchCreator.transform.position;

            _SplineBranchCreator.BezierSplineData.Position = _SplineBranchCreator.transform.position;
            for (int i = 0; i < _SplineBranchCreator.BezierSplineData.PointCount; i++)
                _SplineBranchCreator.BezierSplineData[i] += distanceVector;

            SceneView.RepaintAll();
            EditorUtility.SetDirty(_SplineBranchCreator);
            EditorUtility.SetDirty(_SplineBranchCreator.BezierSplineData);
        }
    }
}
