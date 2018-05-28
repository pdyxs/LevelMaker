using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Level : 
	ScriptableObject
{
	protected LevelActorBase _actor { get; private set; }
	
	public void Save() {
		UpdateFromActor();
	}

	public void Load(LevelActorBase actor)
	{
		this._actor = actor;
		UpdateActor();
	}

	public void Create(LevelActorBase actor)
	{
		this._actor = actor;
		doInitialise();
		UpdateFromActor();
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
	where TActor : LevelActorBase
{
	public TActor actor => _actor as TActor;
}
