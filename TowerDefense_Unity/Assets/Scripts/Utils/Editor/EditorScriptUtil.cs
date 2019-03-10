using UnityEditor;
using UnityEngine;

namespace Game.Utils.Editor
{
	/// <summary>
	/// Some extra functionality, shortcuts, and other things.
	/// </summary>
	public static class EditorScriptUtil
	{
		public static void RangeSlider(SerializedProperty minFloat, SerializedProperty maxFloat, float minLimit, float maxLimit)
		{
			float floatMin = minFloat.floatValue;
			float floatMax = maxFloat.floatValue;

			GUILayout.BeginVertical(GUI.skin.box);
			EditorGUILayout.LabelField("Pitch Range");
			EditorGUILayout.BeginHorizontal();
			floatMin = Mathf.Clamp(EditorGUILayout.FloatField(floatMin), minLimit, maxLimit);

			if (floatMin > floatMax)
				floatMax = floatMin;

			floatMax = Mathf.Clamp(EditorGUILayout.FloatField(floatMax), minLimit, maxLimit);

			if (floatMax < floatMin)
				floatMin = floatMax;


			EditorGUILayout.EndHorizontal();
			EditorGUILayout.MinMaxSlider(ref floatMin, ref floatMax, minLimit, maxLimit);
			GUILayout.EndVertical();


			minFloat.floatValue = floatMin;
			maxFloat.floatValue = floatMax;
		}

		/// <summary>
		/// Fetches the property relative to an array element. 
		/// </summary>
		public static SerializedProperty FromPropertyRelativeFromIndex(SerializedProperty array, int index, string relative)
		{
			if (!array.isArray)
			{
				Debug.LogError("Tried getting property value from index, but the provided property isn't an enumerable.");
				return null;
			}

			SerializedProperty indexedProperty = array.GetArrayElementAtIndex(index);

			return indexedProperty.FindPropertyRelative(relative);
		}

		/// <summary>
		/// Gets the object guid from an element in an array
		/// </summary>
		/// <param name="property">The property to fetch from, this needs to be an array with object references</param>
		public static bool TryGetGuidFromPropertyIndex(SerializedProperty property, int index, out string guid)
		{
			guid = string.Empty;
			if (!property.isArray)
			{
				Debug.LogError("Tried getting property value from index, but the provided property isn't an enumerable.");
				return false;
			}

			SerializedProperty indexedProperty = property.GetArrayElementAtIndex(index);

			return TryFetchPropertyGuid(indexedProperty, out guid);
		}

		/// <summary>
		/// attempts to fetch the guid of a objectreference value
		/// </summary>
		/// <param name="property"></param>
		/// <param name="guid"></param>
		/// <returns></returns>
		public static bool TryFetchPropertyGuid(SerializedProperty property, out string guid)
		{
			guid = string.Empty;
			if (property.objectReferenceValue == null)
			{
				return false;
			}

			if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(property.objectReferenceValue, out guid, out long _))
			{
				return true;
			}

			Debug.LogError($"Couldn't fetch GUID of {property.objectReferenceValue.name}, please make sure it is an scriptable object.");
			return false;
		}
	}
}
