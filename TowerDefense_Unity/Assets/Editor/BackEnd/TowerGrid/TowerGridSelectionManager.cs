using UnityEditor;
using UnityEngine;
using ArrayUtility = Game.Utils.ArrayUtility;

namespace Game.Turrets.Editor
{
    /// <summary>Handles multi-drawing and selecting of the turret grids in the scene view, gets instantiated on project load.</summary>
    [InitializeOnLoad]
    public class TowerGridSelectionManager : UnityEditor.Editor
    {
        private static TowerGrid[] s_TowerGrids = new TowerGrid[0];

        static TowerGridSelectionManager()
        {
            Selection.selectionChanged += OnNewSelection;
            SceneView.onSceneGUIDelegate += OnSceneRender;
        }

        static void OnNewSelection()
        {
            s_TowerGrids = ArrayUtility.ComponentFilter<TowerGrid>(Selection.gameObjects);
        }

        static void OnSceneRender(SceneView sceneView)
        {
	        foreach (TowerGrid grid in s_TowerGrids)
	        {
		        if (grid == null)
			        continue;
		        grid.DrawVerticalGridLines();
		        grid.DrawHorizontalGridLines();
	        }
        }
    }
}
