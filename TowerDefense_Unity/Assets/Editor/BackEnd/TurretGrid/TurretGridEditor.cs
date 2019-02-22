using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace Game.BackEnd.Turrets.Editor
{
    /// <summary>Draws the grid in the scene view.</summary>
    [CustomEditor(typeof(TurretGrid))]
    public class TurretGridEditor : UnityEditor.Editor
    {
        #region Core

        private TurretGrid _TurretGrid;
        private SerializedObject _SerializedTurretGrid;
        private SerializedProperty _CellSize, _GridResolution;

        public void OnEnable()
        {
            _TurretGrid = (TurretGrid)target;
            _SerializedTurretGrid = new SerializedObject(_TurretGrid);
            _CellSize = _SerializedTurretGrid.FindProperty("_CellSize");
            _GridResolution = _SerializedTurretGrid.FindProperty("_GridResolution");
        }

        #endregion

        #region InspectorDrawMethods

        public override void OnInspectorGUI()
        {
            DrawGridSizeOptions();
        }

        private void DrawGridSizeOptions()
        {
            _SerializedTurretGrid.Update();

            GUILayout.BeginVertical("Box");

            _CellSize = _SerializedTurretGrid.FindProperty("_CellSize");
            _GridResolution = _SerializedTurretGrid.FindProperty("_GridResolution");

            GUILayout.Label("Grid Edit Settings", EditorStyles.boldLabel);
            _CellSize.vector2IntValue = EditorGUILayout.Vector2IntField("Cell Size", _CellSize.vector2IntValue);
            _GridResolution.vector2IntValue = EditorGUILayout.Vector2IntField("Grid Resolution", _GridResolution.vector2IntValue);

            GUILayout.Space(5f);

            GUILayout.EndVertical();

            if (!_SerializedTurretGrid.hasModifiedProperties) return;
            _SerializedTurretGrid.ApplyModifiedProperties();
        }

        #endregion

        #region SceneDrawMethods

        private void OnSceneGUI()
        {
            if (Selection.activeObject == null) return;
            DrawHorizontalGridLines(_CellSize.vector2IntValue, _GridResolution.vector2IntValue);
            DrawVerticalGridLines(_CellSize.vector2IntValue, _GridResolution.vector2IntValue);   
        }

        private void DrawHorizontalGridLines(Vector2Int cellSize, Vector2Int gridResolution)
        {
            Vector3 position = Selection.activeTransform.position;
            Vector3 rotation = Selection.activeTransform.eulerAngles;
            Vector2 gridSize = cellSize * gridResolution;

            for (int z = 0; z < gridResolution.y + 1; z++)
            {
                GridPoint startPoint = new GridPoint(new Vector3(-gridSize.x / 2, 0, z * cellSize.y - gridSize.y / 2), rotation);
                GridPoint endPoint = new GridPoint(new Vector3(gridResolution.x * cellSize.x - gridSize.x / 2, 0, z * cellSize.y - gridSize.y / 2), rotation);
                Handles.DrawLine(position + startPoint.GetPointPosition(), position + endPoint.GetPointPosition());
            }
        }

        private void DrawVerticalGridLines(Vector2Int cellSize, Vector2Int gridResolution)
        {
            Vector3 position = Selection.activeTransform.position;
            Vector3 rotation = Selection.activeTransform.eulerAngles;
            Vector2 gridSize = cellSize * gridResolution;

            for (int x = 0; x < gridResolution.x + 1; x++)
            {
                GridPoint startPoint = new GridPoint(new Vector3(x * cellSize.x - gridSize.x / 2, 0, -gridSize.y / 2), rotation);
                GridPoint endPoint = new GridPoint(new Vector3(x * cellSize.x - gridSize.x / 2, 0, gridResolution.y * cellSize.y - gridSize.y / 2), rotation);
                Handles.DrawLine(position + startPoint.GetPointPosition(), position + endPoint.GetPointPosition());
            }
        }

        #endregion
    }
}
