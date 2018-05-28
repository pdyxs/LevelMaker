using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ILevelActor), true)]
public class LevelActorBaseEditor : 
	Editor 
{
	public ILevelActor actor {
		get {
			return target as ILevelActor;
		}
	}
	
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (!Application.isPlaying) {
			return;
		}

		if (actor.hasLevel)
		{
			if (GUILayout.Button("Save"))
			{
				actor.Save();
			}
		}
	}
}
