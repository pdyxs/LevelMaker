using System.Collections;
using System.Collections.Generic;
using PDYXS.ThingSpawner;
using UnityEngine;

public abstract class LevelObject
{
    public static ULevelObject UpdateFromSpawnable<ULevelObject, UActor, UModel>
    (
        ULevelObject current,
        Spawnable<UActor, UModel> spawnable
    )
        where ULevelObject : LevelObject<UActor, UModel>, new()
        where UActor : MonoBehaviour, IEntityInitialisable<UModel>, ISpawnTrackable
        where UModel : class
    {
        if (spawnable.Get != null)
        {
            if (current == null)
            {
                current = new ULevelObject();
                current.SetActor(spawnable.Get);
            }
            current.UpdateFromActor();
            return current;
        }
        return null;
    }

    public static ULevelObject UpdateFromActor<ULevelObject, UActor>
    (
        ULevelObject current,
        UActor actor
    )
        where ULevelObject : LevelObject<UActor>, new()
        where UActor : MonoBehaviour
    {
        if (current == null)
        {
            current = new ULevelObject();
            current.SetActor(actor);
        }
        current.UpdateFromActor(actor);
        return current;
    }

    public static void UpdateSpawnable<ULevelObject, UActor, UModel>
    (
        ULevelObject current,
        Spawnable<UActor, UModel> spawnable,
        LevelObject<UActor, UModel>.CreateModelDelegate CreateModel
    )
        where ULevelObject : LevelObject<UActor, UModel>, new()
        where UActor : MonoBehaviour, IEntityInitialisable<UModel>, ISpawnTrackable
        where UModel : class
    {
        if (current != null)
        {
            if (spawnable.Get == null)
            {
                spawnable.Initialise(CreateModel());
            }
            current.SetActor(spawnable.Get);
            current.UpdateActor();
        } else {
            spawnable.Recycle();
        }
    }

    public abstract void UpdateActor();

    public abstract void UpdateFromActor();
}

[System.Serializable]
public abstract class LevelObject<TActor> : LevelObject {

    public TActor actor {
        get; private set;
    }

    public virtual void SetActor(TActor actor) {
        this.actor = actor;
    }

    public void UpdateActor(TActor actor) {
        SetActor(actor);
        UpdateActor();
    }

    public void UpdateFromActor(TActor actor)
    {
        SetActor(actor);
        UpdateFromActor();
    }

}

[System.Serializable]
public abstract class LevelObject<TActor, TModel> :
    LevelObject<TActor>
    where TActor : MonoBehaviour, IEntityInitialisable<TModel>, ISpawnTrackable
    where TModel : class
{
    public TModel model {
        get {
            return actor.entity;
        }
    }

    public delegate TModel CreateModelDelegate();
}
