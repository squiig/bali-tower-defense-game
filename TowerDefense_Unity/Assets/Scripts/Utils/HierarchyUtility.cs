using System.Collections.Generic;
using UnityEngine;

namespace Game.Utils
{
    /// <summary>Contains unprovided methods for searching different game objects in the hierarchy.</summary>
    public static class HierarchyUtility
    {
        public static GameObject[] GetChildrenOfAllParents(GameObject[] parents)
        {
            List<GameObject> allObjects = new List<GameObject>();
            foreach (GameObject parent in parents)
            {
                allObjects.Add(parent);
                for (int childIndex = 0; childIndex < parent.transform.childCount; childIndex++)
                    allObjects.Add(parent.transform.GetChild(childIndex).gameObject);
            }
            return allObjects.ToArray();
        }
    }
}
