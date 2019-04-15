using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace Game.Turrets.Editor
{
    /// <summary>Draws the grid in the scene view.</summary>
    [CustomEditor(typeof(TurretGrid))]
    public class TurretGridEditor : UnityEditor.Editor
    {
        #region Core

        private SerializedObject _SerializedTurretGrid;
        private SerializedProperty _CellSize, _GridResolution, _GridCellPrefab;

        public void OnEnable()
        {
            _SerializedTurretGrid = serializedObject;
            _CellSize = _SerializedTurretGrid.FindProperty("_CellSize");
            _GridResolution = _SerializedTurretGrid.FindProperty("_GridResolution");
            _GridCellPrefab = _SerializedTurretGrid.FindProperty("_TurretGridCellPrefab");
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

            GUILayout.Label("Grid Edit Settings", EditorStyles.boldLabel);
            _CellSize.vector2Value = EditorGUILayout.Vector2Field("Cell Size", _CellSize.vector2Value);
            _GridResolution.vector2IntValue = EditorGUILayout.Vector2IntField("Grid Resolution", _GridResolution.vector2IntValue);
            _GridResolution.vector2IntValue = new Vector2Int(Mathf.Clamp(_GridResolution.vector2IntValue.x, 0, 40), Mathf.Clamp(_GridResolution.vector2IntValue.y, 0, 40));
            _GridCellPrefab.objectReferenceValue = EditorGUILayout.ObjectField("GridCell Prefab", _GridCellPrefab.objectReferenceValue, typeof(TurretGridCell), false);

            GUILayout.Space(5f);
            GUILayout.EndVertical();

            if (!_SerializedTurretGrid.hasModifiedProperties) return;
            _SerializedTurretGrid.ApplyModifiedProperties();
        }

        #endregion
    }
}
