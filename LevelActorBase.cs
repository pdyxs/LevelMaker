using System.Collections;
using System.Collections.Generic;
using PDYXS.ThingSpawner;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class LevelActorReference: ObjectReference<LevelActorBase> {}

public abstract class LevelActorBase :
	MonoBehaviour
{
	public abstract string filepath { get; }
	public const string extension = ".asset";

	[SerializeField] private PrefabSaver prefabSaver;

	[HideInInspector] public string currentLevel;

	public abstract void Load();
	public abstract void Clear();
}

public abstract class LevelActorBase<TLevel> :
	LevelActorBase
	where TLevel : Level
{
	public TLevel level { get; private set; }

#if UNITY_EDITOR
	public void Save() {
		level.Save();
	}
#endif

	public void Create(string levelName) {
		Clear();
		level = ScriptableObject.CreateInstance<TLevel>();
		level.Create(this);
#if UNITY_EDITOR
		AssetDatabase.CreateAsset(level, $"{filepath}{levelName}{extension}");
#endif
	}

	public override void Load() {
		Clear();
		level = Resources.Load<TLevel>(currentLevel);
		level.Load(this);

		if (Application.isPlaying)
		{
			doInitialise();
		}
	}

	public override void Clear()
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
