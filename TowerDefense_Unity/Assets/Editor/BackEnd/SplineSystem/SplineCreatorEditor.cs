using System.Linq;
using Game.Editor;
using Game.Utils;
using UnityEngine;
using UnityEditor;
using ArrayUtility = Game.Utils.ArrayUtility;
using Bezier3DUtility = Game.Utils.Bezier3DUtility;

namespace Game.SplineSystem.Editor
{
    /// <summary>Draws the custom inspector of the SplineCreatorEditor and the child branches it has. Also handles multi-spline drawing.</summary>
    [CustomEditor(typeof(SplineCreator)), CanEditMultipleObjects]
    public class SplineCreatorEditor : UnityEditor.Editor
    {
        private SplineCreator _SplineCreator;
        private SplineDebugInspector<SplineCreator> _DebugInspector;
        private const int CURVE_LINE_WIDTH = 4;
        private const float AMOUNT_TANGENTS_PER_CURVE = 10;
	    private const int BUTTON_HEIGHT = 20;
		private readonly string[] _SplineModes = { "Free", "Mirrored" };
	    private int _HoveredConnectionSegmentIndex = -1;

		public void OnEnable()
        {
	        Undo.undoRedoPerformed += UndoCallback;
			_SplineCreator = (SplineCreator)target;
            _SplineCreator.BranchCreators = ArrayUtility.ComponentFilter<SplineBranchCreator>(HierarchyUtility.GetChildrenOfAllParents(Selection.gameObjects));
            _DebugInspector = new SplineDebugInspector<SplineCreator>(_SplineCreator);
        }

	    public void OnDisable()
	    {
		    Undo.undoRedoPerformed -= UndoCallback;
	    }

		private void UndoCallback()
		{
			if (_SplineCreator.SelectedPointIndex.HandleIndex > _SplineCreator.BranchCreators[_SplineCreator.SelectedPointIndex.BranchIndex].BezierSplineData.PointCount - 3)
			{
				_SplineCreator.SelectedPointIndex = new HandlePointIndex()
				{
					HandleIndex = _SplineCreator.SelectedPointIndex.HandleIndex - 3,
					BranchIndex = _SplineCreator.SelectedPointIndex.BranchIndex
				};
				_SplineCreator.ModifierPointIndex = new HandlePointIndex()
				{
					HandleIndex = -1,
					BranchIndex = -1
				};
			}
			if (_SplineCreator.SelectedPointIndex.HandleIndex < 0)
			{
				_SplineCreator.SelectedPointIndex = new HandlePointIndex()
				{
					HandleIndex = 0,
					BranchIndex = _SplineCreator.SelectedPointIndex.BranchIndex
				};
				_SplineCreator.ModifierPointIndex = new HandlePointIndex()
				{
					HandleIndex = -1,
					BranchIndex = -1
				};
			}
	    }

		public override void OnInspectorGUI()
        {
            DrawSplineBranchEditButtons();
            _DebugInspector.DrawSplineSettings();
        }

        private void DrawSplineBranchEditButtons()
        {
            GUILayout.BeginVertical("box");
            GUILayout.Label("Branch edit settings", EditorStyles.boldLabel);

	        DrawBezierSplineModeToolBar();

			if (GUILayout.Button("Combine spline"))
                MergeCurves();
            GUILayout.EndVertical();
        }

	    private void DrawBezierSplineModeToolBar()
	    {
		    Undo.RecordObject(_SplineCreator, "Changed_spline_mode");
		    int splineMode = GUILayout.Toolbar((int)_SplineCreator.DrawMode, _SplineModes, GUILayout.Height(BUTTON_HEIGHT + 5));
		    if ((int)_SplineCreator.DrawMode == splineMode)
			    return;

		    _SplineCreator.DrawMode = (SplineCreatorBase.SplineMode)splineMode;
		    HandleUtility.Repaint();
		    EditorUtility.SetDirty(_SplineCreator);
	    }

		private void MergeCurves()
        {
			if (_SplineCreator.ModifierPointIndex.HandleIndex == -1 || _SplineCreator.ModifierPointIndex.BranchIndex == -1)
				return;
	        Undo.RecordObject(_SplineCreator, "Merge_Branch_Into_Other");
	        _SplineCreator.AddConnectionSegment(
				_SplineCreator.SelectedPointIndex.HandleIndex, 
				_SplineCreator.ModifierPointIndex.HandleIndex,
				_SplineCreator.BranchCreators[_SplineCreator.SelectedPointIndex.BranchIndex].BezierSplineData,
				_SplineCreator.BranchCreators[_SplineCreator.ModifierPointIndex.BranchIndex].BezierSplineData);
	        HandleUtility.Repaint();
	        EditorUtility.SetDirty(_SplineCreator);
        }

        private void OnSceneGUI()
        {
			if (!DrawSplines())
				return;

            for (int i = 0; i < _SplineCreator.BranchCount; i++)
            {
                if (_SplineCreator[i].BezierSplineData == null)
	                continue;

	            HandleInput(i);
	            DrawConnectionSegments(i);
                DrawSegments(i);
                DrawSplineHandles(i);

				if (!_SplineCreator[i].transform.hasChanged)
					continue;
                UpdateSplinePosition(i);
                _SplineCreator[i].transform.hasChanged = false;
            }

            DrawSelectedHandles();
        }

	    private void HandleInput(int branchIndex)
	    {
		    HandleShiftCommands(branchIndex);
		    HandleConnectionSegmentDeletion(branchIndex);
	    }

	    private bool DrawSplines()
	    {
		    if (_SplineCreator.BranchCount <= 0)
			    return false;
		    return _SplineCreator.BranchCreators.Count(splineCreator => splineCreator.BezierSplineData != null) > 0;
	    }

        private void DrawSegments(int branchIndex)
        {
            for (int i = 0; i < _SplineCreator[branchIndex].BezierSplineData.SegmentCount; i++)
            {
				Vector3[] segment = _SplineCreator[branchIndex].BezierSplineData.GetSegmentPoints(i);
                Handles.DrawBezier(segment[0], segment[3], segment[1], segment[2], Color.magenta, null, CURVE_LINE_WIDTH);

                if (!_SplineCreator.DrawTangents && !_SplineCreator.DrawNormals && !_SplineCreator.DrawBiNormals) continue;
                for (float t = 0; t < 1; t += 1 / AMOUNT_TANGENTS_PER_CURVE)
                {
                    if (_SplineCreator.DrawTangents)
	                    DrawTangents(segment, t);
                    if (_SplineCreator.DrawBiNormals)
	                    DrawBiNormals(segment, t);
                    if (_SplineCreator.DrawNormals)
	                    DrawNormals(segment, t);
                }
            }
        }

	    private void DrawConnectionSegments(int branchIndex)
	    {
		    if (_SplineCreator.ConnectionSegmentCount <= 0) return;
		    for (int i = 0; i < _SplineCreator.ConnectionSegmentCount; i++)
		    {
			    Color segmentDrawColor = _HoveredConnectionSegmentIndex == i && InputManagerEditor.IsKeyDown(KeyCode.LeftShift)
				    ? Color.yellow
				    : Color.cyan;
			    ConnectionSegment segment = _SplineCreator.GetConnectionSegment(i);
			    segment.Update();
			    DrawConnectionSegmentHandles(segment);
			    Handles.DrawBezier(segment[0], segment[3], segment[1], segment[2], segmentDrawColor, null, 4f);
			}
		}

	    private void DrawConnectionSegmentHandles(ConnectionSegment segment)
	    {
		    Handles.color = Color.yellow;

			//Draw first control point.
		    if (_SplineCreator.SelectedPointIndex.HandleIndex == segment.SenderHandleIndex
		        && Vector3.Distance(segment[0], _SplineCreator[_SplineCreator.SelectedPointIndex.BranchIndex]
			        .BezierSplineData[_SplineCreator.SelectedPointIndex.HandleIndex]) < 0.1f
		        && (segment.SenderHandleIndex <= 0 || segment.SenderHandleIndex >= segment.SenderSpline.PointCount - 1))
		    {
			    Vector3 firstControlPoint = segment.GetFirstControlPoint();
			    Handles.DrawLine(segment.SenderSpline[segment.SenderHandleIndex], firstControlPoint);

			    Undo.RecordObject(_SplineCreator[_SplineCreator.SelectedPointIndex.BranchIndex].BezierSplineData, "Move_connectionPoint");
			    Vector3 point = Handles.DoPositionHandle(segment.GetFirstControlPoint(), Quaternion.identity);
			    if (point != segment.GetFirstControlPoint())
			    {
				    float delta = (_SplineCreator[_SplineCreator.SelectedPointIndex.BranchIndex].BezierSplineData[segment.SenderHandleIndex] - point).magnitude * 2;
				    Vector3 direction = (_SplineCreator[_SplineCreator.SelectedPointIndex.BranchIndex].BezierSplineData[segment.SenderHandleIndex] - point).normalized;
				    EditorUtility.SetDirty(_SplineCreator[_SplineCreator.SelectedPointIndex.BranchIndex].BezierSplineData);

					if (segment.SenderHandleIndex <= 0)
					    _SplineCreator[_SplineCreator.SelectedPointIndex.BranchIndex].BezierSplineData.MovePoint(segment.SenderHandleIndex + 1, point + direction * delta, _SplineCreator.DrawMode);
				    else if (segment.SenderHandleIndex >= segment.SenderSpline.PointCount - 1)
					    _SplineCreator[_SplineCreator.SelectedPointIndex.BranchIndex].BezierSplineData.MovePoint(segment.SenderHandleIndex - 1, point + direction * delta, _SplineCreator.DrawMode);
			    }
		    }

			//Draw second control point.
		    if (_SplineCreator.SelectedPointIndex.HandleIndex == segment.ReceiverHandleIndex
		        && Vector3.Distance(segment[3], _SplineCreator[_SplineCreator.SelectedPointIndex.BranchIndex]
			        .BezierSplineData[_SplineCreator.SelectedPointIndex.HandleIndex]) < 0.1f 
		        && (segment.ReceiverHandleIndex <= 0 || segment.ReceiverHandleIndex >= segment.SenderSpline.PointCount - 1))
		    {
			    Vector3 secondControlPoint = segment.GetSecondControlPoint();
			    Handles.DrawLine(segment.ReceiverSpline[segment.ReceiverHandleIndex], secondControlPoint);

			    Undo.RecordObject(_SplineCreator[_SplineCreator.SelectedPointIndex.BranchIndex].BezierSplineData, "Move_connectionPoint");
			    Vector3 point = Handles.DoPositionHandle(segment.GetSecondControlPoint(), Quaternion.identity);
			    if (point != segment.GetSecondControlPoint())
			    {
				    float delta = (_SplineCreator[_SplineCreator.SelectedPointIndex.BranchIndex].BezierSplineData[segment.ReceiverHandleIndex] - point).magnitude * 2;
				    Vector3 direction = (_SplineCreator[_SplineCreator.SelectedPointIndex.BranchIndex].BezierSplineData[segment.ReceiverHandleIndex] - point).normalized;
				    EditorUtility.SetDirty(_SplineCreator[_SplineCreator.SelectedPointIndex.BranchIndex].BezierSplineData);

				    if (segment.ReceiverHandleIndex <= 0)
					    _SplineCreator[_SplineCreator.SelectedPointIndex.BranchIndex].BezierSplineData.MovePoint(segment.ReceiverHandleIndex + 1, point + direction * delta, _SplineCreator.DrawMode);
				    else if (segment.ReceiverHandleIndex >= segment.SenderSpline.PointCount - 1)
					    _SplineCreator[_SplineCreator.SelectedPointIndex.BranchIndex].BezierSplineData.MovePoint(segment.ReceiverHandleIndex - 1, point + direction * delta, _SplineCreator.DrawMode);
			    }
			}
		}

        private void DrawTangents(Vector3[] segment, float t)
        {
            float tangentLength = HandleUtility.GetHandleSize(_SplineCreator[_SplineCreator.SelectedPointIndex.BranchIndex].BezierSplineData[_SplineCreator.SelectedPointIndex.HandleIndex]);
            Vector3 startPoint = Bezier3DUtility.CubicCurveVector3(segment[0], segment[1], segment[2], segment[3], t + 0.1f);
            Vector3 tangent = Bezier3DUtility.GetTangent(startPoint, Bezier3DUtility.CubicCurveVector3(segment[0], segment[1], segment[2], segment[3], t));
            Handles.color = Color.green;
            Handles.DrawLine(startPoint, startPoint + tangent * tangentLength / 3);
        }

        private void DrawBiNormals(Vector3[] segment, float t)
        {
            float biNormalLength = HandleUtility.GetHandleSize(_SplineCreator[_SplineCreator.SelectedPointIndex.BranchIndex].BezierSplineData[_SplineCreator.SelectedPointIndex.HandleIndex]);
            Vector3 startPoint = Bezier3DUtility.CubicCurveVector3(segment[0], segment[1], segment[2], segment[3], t + 0.1f);
            Vector3 tangent = Bezier3DUtility.GetTangent(startPoint, Bezier3DUtility.CubicCurveVector3(segment[0], segment[1], segment[2], segment[3], t));
            Vector3 biNormal = Vector3.Cross(Vector3.up, tangent).normalized;
            Handles.color = Color.red;
            Handles.DrawLine(startPoint, startPoint + biNormal * biNormalLength / 3);
        }

        private void DrawNormals(Vector3[] segment, float t)
        {
            float normalLength = HandleUtility.GetHandleSize(_SplineCreator[_SplineCreator.SelectedPointIndex.BranchIndex].BezierSplineData[_SplineCreator.SelectedPointIndex.HandleIndex]);
            Vector3 startPoint = Bezier3DUtility.CubicCurveVector3(segment[0], segment[1], segment[2], segment[3], t + 0.1f);
            Vector3 tangent = Bezier3DUtility.GetTangent(startPoint, Bezier3DUtility.CubicCurveVector3(segment[0], segment[1], segment[2], segment[3], t));
            Vector3 biNormal = Vector3.Cross(Vector3.up, tangent).normalized;
            Vector3 normal = Vector3.Cross(tangent, biNormal).normalized;
            Handles.color = Color.cyan;
            Handles.DrawLine(startPoint, startPoint + normal * normalLength / 3);
        }

        private void DrawSplineHandles(int branchIndex)
        {
            for (int i = 0; i < _SplineCreator[branchIndex].BezierSplineData.PointCount; i += 3)
            {
	            Handles.color = _SplineCreator.ModifierPointIndex.HandleIndex == i && _SplineCreator.ModifierPointIndex.BranchIndex == branchIndex 
		            ? Color.yellow 
		            : Color.red;
				float handleSize = HandleUtility.GetHandleSize(_SplineCreator[branchIndex].BezierSplineData[i]);

                bool leftShift = InputManagerEditor.IsKeyDown(KeyCode.LeftShift);
                if (Handles.Button(_SplineCreator[branchIndex].BezierSplineData[i], Quaternion.identity, 
	                handleSize * 0.12f, handleSize * 0.16f, Handles.SphereHandleCap))
                {
                    if (leftShift)
                    {
                        _SplineCreator.ModifierPointIndex = new HandlePointIndex()
                        {
                            HandleIndex = i,
                            BranchIndex = branchIndex
                        };
                        continue;
                    }

                    _SplineCreator.ModifierPointIndex = new HandlePointIndex()
                    {
                        HandleIndex = -1,
                        BranchIndex = -1
                    };
                    _SplineCreator.SelectedPointIndex = new HandlePointIndex()
                    {
                        HandleIndex = i,
                        BranchIndex = branchIndex
                    };
                }
            }
        }

        private void DrawSelectedHandles()
        {
	        if (_SplineCreator[_SplineCreator.SelectedPointIndex.BranchIndex].BezierSplineData == null)
	        {
		        _SplineCreator.SelectedPointIndex = new HandlePointIndex()
		        {
					HandleIndex = -1,
					BranchIndex = -1
		        };
		        _SplineCreator.ModifierPointIndex = new HandlePointIndex()
		        {
			        HandleIndex = -1,
			        BranchIndex = -1
		        };
				return;
			}

			Handles.color = Color.yellow;
	        if (_SplineCreator.SelectedPointIndex.HandleIndex / 3 < _SplineCreator[_SplineCreator.SelectedPointIndex.BranchIndex].BezierSplineData.PointCount / 3)
	        {
		        Vector3[] segment = _SplineCreator[_SplineCreator.SelectedPointIndex.BranchIndex].BezierSplineData.GetSegmentPoints(_SplineCreator.SelectedPointIndex.HandleIndex / 3);
				if (segment[0] == _SplineCreator[_SplineCreator.SelectedPointIndex.BranchIndex].BezierSplineData[_SplineCreator.SelectedPointIndex.HandleIndex])
					Handles.DrawLine(segment[0], segment[1]);
			}

	        if (_SplineCreator.SelectedPointIndex.HandleIndex / 3 > 0)
	        {
				Vector3[] lastSegment = _SplineCreator[_SplineCreator.SelectedPointIndex.BranchIndex].BezierSplineData.GetSegmentPoints(_SplineCreator.SelectedPointIndex.HandleIndex / 3 - 1);
				if (lastSegment[3] == _SplineCreator[_SplineCreator.SelectedPointIndex.BranchIndex].BezierSplineData[_SplineCreator.SelectedPointIndex.HandleIndex])
					Handles.DrawLine(lastSegment[2], lastSegment[3]);
	        }

			DrawSplinePositionHandle(_SplineCreator.SelectedPointIndex.BranchIndex, _SplineCreator.SelectedPointIndex.HandleIndex);
            if (_SplineCreator.SelectedPointIndex.HandleIndex + 1 < _SplineCreator[_SplineCreator.SelectedPointIndex.BranchIndex].BezierSplineData.PointCount)
                DrawSplinePositionHandle(_SplineCreator.SelectedPointIndex.BranchIndex, _SplineCreator.SelectedPointIndex.HandleIndex + 1);
            if (_SplineCreator.SelectedPointIndex.HandleIndex - 1 >= 0)
                DrawSplinePositionHandle(_SplineCreator.SelectedPointIndex.BranchIndex, _SplineCreator.SelectedPointIndex.HandleIndex - 1);
            if (_SplineCreator[_SplineCreator.SelectedPointIndex.BranchIndex].BezierSplineData.IsClosed && _SplineCreator.SelectedPointIndex.HandleIndex == 0)
                DrawSplinePositionHandle(_SplineCreator.SelectedPointIndex.BranchIndex, _SplineCreator[_SplineCreator.SelectedPointIndex.BranchIndex].BezierSplineData.PointCount - 1);
        }

        private void DrawSplinePositionHandle(int branchIndex, int index)
        {
            Undo.RecordObject(_SplineCreator[branchIndex].BezierSplineData, "Move_point");
            Vector3 point = Handles.DoPositionHandle(_SplineCreator[branchIndex].BezierSplineData[index], Quaternion.identity);
            if (point == _SplineCreator[branchIndex].BezierSplineData[index])
	            return;

            EditorUtility.SetDirty(_SplineCreator[branchIndex].BezierSplineData);
            _SplineCreator[branchIndex].BezierSplineData.MovePoint(index, point, _SplineCreator.DrawMode);
        }

        private void UpdateSplinePosition(int branchIndex)
        {
            Undo.RecordObject(_SplineCreator[branchIndex], "Move_Spline");
            Undo.RecordObject(_SplineCreator[branchIndex].BezierSplineData, "Move_Spline");

            float moveDistance = Mathf.Sign(Vector3.Distance(_SplineCreator[branchIndex].BezierSplineData.Position, _SplineCreator[branchIndex].transform.position));
            Vector3 distanceVector = moveDistance >= 0
                ? _SplineCreator[branchIndex].transform.position - _SplineCreator[branchIndex].BezierSplineData.Position
                : _SplineCreator[branchIndex].BezierSplineData.Position - _SplineCreator[branchIndex].transform.position;

            _SplineCreator[branchIndex].BezierSplineData.Position = _SplineCreator[branchIndex].transform.position;
            for (int i = 0; i < _SplineCreator[branchIndex].BezierSplineData.PointCount; i++)
                _SplineCreator[branchIndex].BezierSplineData[i] += distanceVector;

            HandleUtility.Repaint();
            EditorUtility.SetDirty(_SplineCreator[branchIndex]);
            EditorUtility.SetDirty(_SplineCreator[branchIndex].BezierSplineData);
        }

	    private void HandleShiftCommands(int branchIndex)
	    {
			Event e = Event.current;
		    if (e.type == EventType.MouseDown && e.button == 0 && e.shift && _HoveredConnectionSegmentIndex > -1)
		    {
			    Undo.RecordObject(_SplineCreator, "RemoveConnectionPoint");
			    _SplineCreator.RemoveConnectionSegment(_SplineCreator.GetConnectionSegment(_HoveredConnectionSegmentIndex));
			    _HoveredConnectionSegmentIndex = -1;
			    EditorUtility.SetDirty(_SplineCreator);
		    }
	    }

	    private void HandleConnectionSegmentDeletion(int branchIndex)
	    {
		    Event e = Event.current;
			if (e.type != EventType.MouseMove) return;

		    float selectionThreshHold = 5f;
		    int newSelectedIndex = -1;

		    for (int i = 0; i < _SplineCreator.ConnectionSegmentCount; i++)
		    {
				ConnectionSegment segment = _SplineCreator.GetConnectionSegment(i);
				float distance = HandleUtility.DistancePointBezier(
					e.mousePosition, 
					HandleUtility.WorldToGUIPoint(segment[0]), 
					HandleUtility.WorldToGUIPoint(segment[3]), 
					HandleUtility.WorldToGUIPoint(segment[1]), 
					HandleUtility.WorldToGUIPoint(segment[2]));

			    if (distance < selectionThreshHold)
			    {
					selectionThreshHold = distance;
					newSelectedIndex = i;
			    }

			    if (newSelectedIndex == _HoveredConnectionSegmentIndex)
				    continue;
			    _HoveredConnectionSegmentIndex = newSelectedIndex;
			    HandleUtility.Repaint();
		    }
	    }
    }
}
