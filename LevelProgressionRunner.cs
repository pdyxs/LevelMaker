using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelProgressionRunnerReference : ObjectReference<LevelProgressionRunner> {}

public class LevelProgressionRunner : 
	MonoBehaviour
{
	
	public LevelActorReference actor;
	
	public LevelProgression progression;
	
	[Header("Level Loading")]
	public string filepath;

	public bool LoadOnAwake;
	
	public enum LoadTarget
	{
		Progression,
		Level
	}

	public LoadTarget loadFrom;
	[HideInInspector]
	public int nextLevel = 0;
	
	[HideInInspector]
	public Level level;

	private void Awake()
	{
		if (LoadOnAwake)
		{
#if UNITY_EDITOR
			if (loadFrom == LoadTarget.Level && level != null)
			{
				actor.Get(this).Load(level);
				return;
			}
#endif
			if (HasNext())
			{
				LoadNext();
			}
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
			actor.Get(this).Load(progression.levels[nextLevel]);
			nextLevel++;
		}
	}
}
