using System.Collections;
using System.Collections.Generic;
using PDYXS.ThingSpawner;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class LevelActorReference: ObjectReference<ILevelActor> {}

public interface ILevelActor
{
	bool hasLevel { get; }

	Level Create(string levelName, string filepath);
	void Load(Level l);
	void Clear();
	
#if UNITY_EDITOR
	void Save();
#endif
}

public abstract class LevelActorBase<TLevel> :
	MonoBehaviour, ILevelActor
	where TLevel : Level
{
	[SerializeField] private PrefabSaver prefabSaver;
	
	public TLevel level { get; private set; }

	public bool hasLevel => level != null;

#if UNITY_EDITOR
	public void Save() {
		level.Save();
	}
#endif

	public Level Create(string levelName, string filepath) {
		Clear();
		level = ScriptableObject.CreateInstance<TLevel>();
		level.Create(this);
#if UNITY_EDITOR
		UnityEditor.AssetDatabase.CreateAsset(level, $"{filepath}{levelName}{Level.extension}");
#endif
		return level;
	}
	
	public UnityEvent OnLevelLoaded = new UnityEvent();

	public void Load(Level level)
	{
		
		var l = level as TLevel;
		if (l != null)
		{
			StartCoroutine(WaitToLoad(l));
		}
	}

	public IEnumerator WaitToLoad(TLevel level)
	{
		Clear();
		yield return new WaitForEndOfFrame();
		this.level = level;
		level.Load(this);

		if (Application.isPlaying)
		{
			doInitialise();
		}
		OnLevelLoaded.Invoke();
	}

	public void Clear()
	{
		if (level != null)
		{
			level.Clear();
			level = null;
		}

		doClear();
	}

	protected abstract void doInitialise();

	protected abstract void doClear();
}
