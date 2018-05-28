using System.Collections;
using System.Collections.Generic;
using PDYXS.ThingSpawner;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class LevelObjectList<TActor, TModel, TLevelObject> : 
	LevelObject<SpawnableList<TActor, TModel>>, IEnumerable
	where TLevelObject : LevelObject<TActor, TModel>, new()
	where TActor : MonoBehaviour, IEntityInitialisable<TModel>, ISpawnTrackable
	where TModel : class
{
	public List<TLevelObject> levelObjects = new List<TLevelObject>();

	public int Count {
		get {
			return levelObjects.Count;
		}
	}

	public TLevelObject this[int i] {
		get {
			return levelObjects[i];
		}
	}

	public IEnumerator GetEnumerator()
	{
		return actor.GetEnumerator();
	}

	public void Add(TLevelObject o) {
		levelObjects.Add(o);
	}

	public void ClearExtraLevelObjects() {
		levelObjects.RemoveRange(actor.Count, levelObjects.Count - actor.Count);
	}

	public override void UpdateFromActor()
	{
		for (int i = 0; i != actor.Count; ++i)
		{
			if (Count == i)
			{
				Add(new TLevelObject());
			}
			levelObjects[i].SetActor(actor[i]);
			levelObjects[i].UpdateFromActor();
		}
		ClearExtraLevelObjects();
	}

	public override void UpdateActor()
	{}

	public void UpdateActor(
		SpawnableList<TActor, TModel> actor, 
		UnityAction CreateFunction,
		UnityAction<int> RemoveFromFunction
	) {
		UpdateActor(actor);
		for (int i = 0; i != Count; ++i)
		{
			if (actor.Count == i)
			{
				CreateFunction.Invoke();
			}
			levelObjects[i].UpdateActor(actor[i]);
		}
		RemoveFromFunction.Invoke(levelObjects.Count);
	}
}