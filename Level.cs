using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Level : 
	ScriptableObject
{
	public const string extension = ".asset";
	
	protected ILevelActor _actor { get; private set; }
	
	public void Save() {
		UpdateFromActor();
	}

	public void Load(ILevelActor actor)
	{
		this._actor = actor;
		UpdateActor();
	}

	public void Create(ILevelActor actor)
	{
		this._actor = actor;
		doInitialise();
		if (Application.isPlaying) UpdateFromActor();
	}

	public void Clear()
	{
		doClear();
	}

	protected abstract void doClear();
	protected abstract void doInitialise();
	protected abstract void UpdateFromActor();
	protected abstract void UpdateActor();
}

public abstract class Level<TActor> :
	Level
	where TActor : MonoBehaviour, ILevelActor
{
	public TActor actor => _actor as TActor;
}
