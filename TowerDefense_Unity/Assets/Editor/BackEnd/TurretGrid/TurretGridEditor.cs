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
    }
}
