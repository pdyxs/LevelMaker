using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelProgressionRunnerReference : ObjectReference<LevelProgressionRunner> {}

public class LevelProgressionRunner : 
	MonoBehaviour
{
	public LevelProgression progression;

	public LevelActorReference actor;

	public bool LoadOnAwake;

	public int nextLevel = 0;

	private void Awake()
	{
		if (LoadOnAwake && HasNext())
		{
			LoadNext();
		}
	}

	public bool HasNext()
	{
		return progression.levels?.Count > nextLevel;
	}

	public void LoadNext()
	{
		if (progression != null)
		{
			actor.Get(this).currentLevel = progression.levels[nextLevel].name;
			actor.Get(this).Load();
			nextLevel++;
		}
	}
}
