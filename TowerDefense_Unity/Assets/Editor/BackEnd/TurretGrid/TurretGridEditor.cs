using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Game.BackEnd.Turrets.Editor
{
    /// <summary>Draws the grid in the scene view.</summary>
    [CustomEditor(typeof(TurretGrid))]
    public class TurretGridEditor : UnityEditor.Editor
    {

    #region Core

        private SerializedObject _SerializedTurretGrid;

        public void OnEnable()
        {
            _SerializedTurretGrid = new SerializedObject((TurretGrid)target);
        }

        public void OnDisable()
        {
            
        }

        #endregion

        #region InspectorDrawMethods

        public override void OnInspectorGUI()
        {
            DrawGridSizeOptions();
        }

        private void DrawGridSizeOptions()
        {
            base.OnInspectorGUI();
        }

        #endregion

        #region SceneDrawMethods

        private void OnSceneGUI()
    {
        SerializedProperty cellSize = _SerializedTurretGrid.FindProperty("_CellSize");
        SerializedProperty gridResolution = _SerializedTurretGrid.FindProperty("_GridResolution");

        DrawHorizontalGridLines(cellSize.vector2IntValue, gridResolution.vector2IntValue);
        DrawVerticalGridLines(cellSize.vector2IntValue, gridResolution.vector2IntValue);   
    }

    private void DrawHorizontalGridLines(Vector2Int cellSize, Vector2Int gridResolution)
    {
        for (int z = 0; z < gridResolution.y + 1; z++)
        {
            Vector3 startPoint = new Vector3(0, 0, z * cellSize.y);
            Vector3 endPoint = new Vector3(gridResolution.x * cellSize.x, 0, z * cellSize.y);

            Handles.DrawLine(startPoint, endPoint);
        }
    }

    private void DrawVerticalGridLines(Vector2Int cellSize, Vector2Int gridResolution)
    {
        for (int x = 0; x < gridResolution.x + 1; x++)
        {
            Vector3 startPoint = new Vector3(x * cellSize.x, 0, 0);
            Vector3 endPoint = new Vector3(x * cellSize.x, 0, gridResolution.y * cellSize.y);

            Handles.DrawLine(startPoint, endPoint);
        }
    }
        
    #endregion
    }
}
