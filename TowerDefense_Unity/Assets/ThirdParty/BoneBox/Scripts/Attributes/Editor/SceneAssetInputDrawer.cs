using System.Collections;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

namespace BoneBox.Attributes.Drawers
{
	[CustomPropertyDrawer(typeof(SceneAssetInput))]
	public class SceneAssetInputDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			// Using BeginProperty / EndProperty on the parent property means that
			// prefab override logic works on the entire property.
			EditorGUI.BeginProperty(position, label, property);
			
			SceneAsset oldScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(property.stringValue);

			EditorGUI.BeginChangeCheck();
			
			SceneAsset newScene = EditorGUILayout.ObjectField("Scene", oldScene, typeof(Object), false) as SceneAsset; // wrong?

			if (EditorGUI.EndChangeCheck())
			{
				string newPath = AssetDatabase.GetAssetPath(newScene);
				property.stringValue = newPath;
			}
			
			EditorGUI.EndProperty();
		}
	}
}
