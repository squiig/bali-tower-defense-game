using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Game.Utils;
using UnityEngine.Experimental.UIElements;
using ArrayUtility = Game.Utils.ArrayUtility;

namespace Game.SplineSystem.Editor
{
    /// <summary>Draws spline when added to the selection list.</summary>
    [InitializeOnLoad]
    public class SplineDrawer
    {
        private static SplineBranchCreator[] s_Branches = new SplineBranchCreator[0];
        private static int s_SelectedHandleIndex, s_SelectedBranchIndex;
        private const int CURVE_LINE_WIDTH = 3;
        private const float AMOUNT_TANGENTS_PER_CURVE = 10;

        static SplineDrawer()
        {
            s_SelectedBranchIndex = 0;
            s_SelectedHandleIndex = 0;
            Selection.selectionChanged += OnNewSelection;
            SceneView.onSceneGUIDelegate += OnSceneRender;
        }

        private static void OnNewSelection()
        {
            s_Branches = ArrayUtility.ComponentFilter<SplineBranchCreator>(HierarchyUtility.GetChildrenOfAllParents(Selection.gameObjects));
        }

        private static void OnSceneRender(SceneView sceneView)
        {
            if (s_Branches.Length <= 0) return;
            for (int i = 0; i < s_Branches.Length; i++)
            {
                if (s_Branches[i].BezierSplineData == null) return;

                DrawSegments(i);
                DrawSplineHandles(i);

                if (!s_Branches[i].transform.hasChanged) continue;
                UpdateSplinePosition(i);
                s_Branches[i].transform.hasChanged = false;
            }

            DrawSelectedHandles();
        }

        private static void DrawSegments(int branchIndex)
        {
            for (int i = 0; i < s_Branches[branchIndex].BezierSplineData.SegmentCount; i++)
            {
                Vector3[] segment = s_Branches[branchIndex].BezierSplineData.GetSegmentPoints(i);
                Handles.DrawBezier(segment[0], segment[3], segment[1], segment[2], Color.magenta, null, CURVE_LINE_WIDTH);

                if (!s_Branches[branchIndex].DrawTangents && !s_Branches[branchIndex].DrawNormals && !s_Branches[branchIndex].DrawBiNormals) continue;
                for (float t = 0; t < 1; t += 1 / AMOUNT_TANGENTS_PER_CURVE)
                {
                    if (s_Branches[branchIndex].DrawTangents) DrawTangents(segment, t);
                    if (s_Branches[branchIndex].DrawBiNormals) DrawBiNormals(segment, t);
                    if (s_Branches[branchIndex].DrawNormals) DrawNormals(segment, t);
                }
            }
        }

        private static void DrawTangents(Vector3[] segment, float t)
        {
            Vector3 startPoint = Bezier3DUtility.CubicCurveVector3(segment[0], segment[1], segment[2], segment[3], t + 0.1f);
            Vector3 tangent = Bezier3DUtility.GetTangent(startPoint, Bezier3DUtility.CubicCurveVector3(segment[0], segment[1], segment[2], segment[3], t));
            Handles.color = Color.green;
            Handles.DrawLine(startPoint, startPoint + tangent);
        }

        private static void DrawBiNormals(Vector3[] segment, float t)
        {
            Vector3 startPoint = Bezier3DUtility.CubicCurveVector3(segment[0], segment[1], segment[2], segment[3], t + 0.1f);
            Vector3 tangent = Bezier3DUtility.GetTangent(startPoint, Bezier3DUtility.CubicCurveVector3(segment[0], segment[1], segment[2], segment[3], t));
            Vector3 biNormal = Vector3.Cross(Vector3.up, tangent).normalized;
            Handles.color = Color.red;
            Handles.DrawLine(startPoint, startPoint + biNormal);
        }

        private static void DrawNormals(Vector3[] segment, float t)
        {
            Vector3 startPoint = Bezier3DUtility.CubicCurveVector3(segment[0], segment[1], segment[2], segment[3], t + 0.1f);
            Vector3 tangent = Bezier3DUtility.GetTangent(startPoint, Bezier3DUtility.CubicCurveVector3(segment[0], segment[1], segment[2], segment[3], t));
            Vector3 biNormal = Vector3.Cross(Vector3.up, tangent).normalized;
            Vector3 normal = Vector3.Cross(tangent, biNormal).normalized;
            Handles.color = Color.cyan;
            Handles.DrawLine(startPoint, startPoint + normal);
        }

        private static void DrawSplineHandles(int branchIndex)
        {
            for (int i = 0; i < s_Branches[branchIndex].BezierSplineData.PointCount; i += 3)
            {
                Handles.color = Color.red;
                float handleSize = HandleUtility.GetHandleSize(s_Branches[branchIndex].BezierSplineData[i]);
                if (!Handles.Button(s_Branches[branchIndex].BezierSplineData[i], Quaternion.identity, handleSize * 0.12f, handleSize * 0.16f, Handles.SphereHandleCap))
                    continue;

                s_SelectedHandleIndex = i;
                s_SelectedBranchIndex = branchIndex;
            }
        }

        private static void DrawSelectedHandles()
        {
            Vector3[] segment = s_Branches[s_SelectedBranchIndex].BezierSplineData.GetSegmentPoints(s_SelectedHandleIndex > 0 ? s_SelectedHandleIndex / 3 - 1 : s_SelectedHandleIndex / 3);
            Handles.color = Color.yellow;
            if (segment[0] == s_Branches[s_SelectedBranchIndex].BezierSplineData[s_SelectedHandleIndex]) Handles.DrawLine(segment[0], segment[1]);
            if (segment[3] == s_Branches[s_SelectedBranchIndex].BezierSplineData[s_SelectedHandleIndex]) Handles.DrawLine(segment[2], segment[3]);

            DrawSplinePositionHandle(s_SelectedBranchIndex, s_SelectedHandleIndex);
            if (s_SelectedHandleIndex + 1 < s_Branches[s_SelectedBranchIndex].BezierSplineData.PointCount)
                DrawSplinePositionHandle(s_SelectedBranchIndex, s_SelectedHandleIndex + 1);
            if (s_SelectedHandleIndex - 1 >= 0)
                DrawSplinePositionHandle(s_SelectedBranchIndex, s_SelectedHandleIndex - 1);
            if (s_Branches[s_SelectedBranchIndex].BezierSplineData.IsClosed && s_SelectedHandleIndex == 0)
                DrawSplinePositionHandle(s_SelectedBranchIndex, s_Branches[s_SelectedBranchIndex].BezierSplineData.PointCount - 1);
        }

        private static void DrawSplinePositionHandle(int branchIndex, int index)
        {
            Undo.RecordObject(s_Branches[branchIndex].BezierSplineData, "Move_point");
            Vector3 point = Handles.DoPositionHandle(s_Branches[branchIndex].BezierSplineData[index], Quaternion.identity);
            if (point == s_Branches[branchIndex].BezierSplineData[index]) return;

            EditorUtility.SetDirty(s_Branches[branchIndex].BezierSplineData);
            s_Branches[branchIndex].BezierSplineData.MovePoint(index, point, s_Branches[branchIndex].DrawMode);
        }

        private static void UpdateSplinePosition(int branchIndex)
        {
            Undo.RecordObject(s_Branches[branchIndex], "Move_Spline");
            Undo.RecordObject(s_Branches[branchIndex].BezierSplineData, "Move_Spline");

            float moveDistance = Mathf.Sign(Vector3.Distance(s_Branches[branchIndex].BezierSplineData.Position, s_Branches[branchIndex].transform.position));
            Vector3 distanceVector = moveDistance >= 0
                ? s_Branches[branchIndex].transform.position - s_Branches[branchIndex].BezierSplineData.Position
                : s_Branches[branchIndex].BezierSplineData.Position - s_Branches[branchIndex].transform.position;

            s_Branches[branchIndex].BezierSplineData.Position = s_Branches[branchIndex].transform.position;
            for (int i = 0; i < s_Branches[branchIndex].BezierSplineData.PointCount; i++)
                s_Branches[branchIndex].BezierSplineData[i] += distanceVector;

            SceneView.RepaintAll();
            EditorUtility.SetDirty(s_Branches[branchIndex]);
            EditorUtility.SetDirty(s_Branches[branchIndex].BezierSplineData);
        }
    }
}
