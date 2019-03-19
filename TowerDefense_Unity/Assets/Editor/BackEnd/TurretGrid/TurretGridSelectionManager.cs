using UnityEditor;
using UnityEngine;
using ArrayUtility = Game.Utils.ArrayUtility;

namespace Game.Turrets.Editor
{
    /// <summary>Handles multi-drawing and selecting of the turret grids in the scene view, gets instantiated on project load.</summary>
    [InitializeOnLoad]
    public class TurretGridSelectionManager : UnityEditor.Editor
    {
        private static TurretGrid[] _TurretGrids = new TurretGrid[0];

        static TurretGridSelectionManager()
        {
            Selection.selectionChanged += OnNewSelection;
            SceneView.onSceneGUIDelegate += OnSceneRender;
        }

        static void OnNewSelection()
        {
            _TurretGrids = ArrayUtility.ComponentFilter<TurretGrid>(Selection.gameObjects);
        }

        static void OnSceneRender(SceneView sceneView)
        {
            for (int i = 0; i < _TurretGrids.Length; i++)
            {
                _TurretGrids[i].DrawVerticalGridLines();
                _TurretGrids[i].DrawHorizontalGridLines();
            }
        }
    }
}
