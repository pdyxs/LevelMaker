using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(LevelObject), true)]
public class LevelObjectDrawer : PropertyDrawer {
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		var listObjects = property.FindPropertyRelative("levelObjects");
		if (listObjects != null)
		{
			return EditorGUI.GetPropertyHeight(listObjects, label);
		}
		return EditorGUI.GetPropertyHeight(property, label);
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		var listObjects = property.FindPropertyRelative("levelObjects");
		if (listObjects != null)
		{
			EditorGUI.PropertyField(position, listObjects, label, true);
			return;
		}
		EditorGUI.PropertyField(position, property, label, true);
	}
}
