using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Game.Turrets.Editor
{
    /// <summary>Handles multi-drawing and selecting of the turret grids in the scene view, gets instantiated on project load.</summary>
    [InitializeOnLoad]
    public class TurretGridSelectionManager : UnityEditor.Editor
    {
        static TurretGridSelectionManager()
        {
            SceneView.onSceneGUIDelegate += OnSceneRender;
        }
        
        static void OnSceneRender(SceneView sceneView)
        {
            GameObject[] selectedGameObjects = Selection.gameObjects;
            foreach (GameObject gameObject in selectedGameObjects)
            {
                if (gameObject.GetComponent<TurretGrid>() != null)
                {
                    gameObject.GetComponent<TurretGrid>().DrawHorizontalGridLines();
                    gameObject.GetComponent<TurretGrid>().DrawVerticalGridLines();
                }
            }
        }
    }
}
