using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LinkedLevelObject<TObject> where TObject : class
{
    public TObject obj {
        get; private set;
    }

    protected object parent {
        get; private set;
    }

    public void Initialise(TObject o) {
        obj = o;
        Initialise();
    }

    protected abstract void Initialise();

    public void Create(TObject o) {
        obj = o;
        doCreate();
    }

    protected abstract void doCreate();

    public TObject Load(object parent = null) {
        this.parent = parent;
        Initialise(doLoad());
        return obj;
    }

    protected abstract TObject doLoad();

    public void Save() {
        doSave();
    }

    protected abstract void doSave();

}

[System.Serializable]
public class LinkedLevelList<TObject, TLinkedObject> :
    LinkedLevelObject<EventList<TObject>>, IEnumerable
    where TObject : class
    where TLinkedObject : LinkedLevelObject<TObject>, new()
{
    public List<TLinkedObject> lobjects = new List<TLinkedObject>();

    public IEnumerator GetEnumerator()
    {
        return lobjects.GetEnumerator();
    }

    public TLinkedObject this[int i]
    {
        get {
            return lobjects[i];
        }
    }

    public int Count {
        get {
            return lobjects.Count;
        }
    }

    private void SetupListeners() {
        obj.OnAdded.AddListener(Add);
        obj.OnRemoved.AddListener(Remove);
        obj.OnCleared.AddListener(Clear);
        obj.OnInserted.AddListener(Insert);
        obj.OnRemovedAt.AddListener(RemoveAt);
    }

    protected override void Initialise() {
        SetupListeners();
        for (int i = 0; i != obj.Count; ++i)
        {
            if (lobjects.Count <= i)
            {
                Add(obj[i]);
            }
        }
    }

    protected override EventList<TObject> doLoad()
    {
        var list = new EventList<TObject>();
        foreach (var lo in lobjects) {
            list.Add(lo.Load(parent));
        }
        Initialise(list);
        return list;
    }

    protected override void doCreate()
    {
        SetupListeners();
        for (int i = 0; i != obj.Count; ++i) {
            if (lobjects.Count > i) {
                lobjects[i].Create(obj[i]);
            } else {
                var lo = new TLinkedObject();
                lobjects.Add(lo);
                lo.Create(obj[i]);
            }
        }
    }

    protected override void doSave()
    {
        foreach (var lo in lobjects) {
            lo.Save();
        }
    }

    protected virtual void Clear()
    {
        lobjects.Clear();
    }

    protected virtual void Add(TObject o)
    {
        var lo = new TLinkedObject();
        lo.Initialise(o);
        lobjects.Add(lo);
    }

    protected virtual void Insert(int index, TObject o)
    {
        var lo = new TLinkedObject();
        lo.Initialise(o);
        lobjects.Insert(index, lo);
    }

    protected virtual void Remove(TLinkedObject lo)
    {
        lobjects.Remove(lo);
    }

    protected virtual void Remove(TObject o)
    {
        var lo = lobjects.Find((obj) => obj.obj == o);
        if (o != null)
        {
            Remove(lo);
        }
    }

    protected virtual void RemoveAt(int index)
    {
        lobjects.RemoveAt(index);
    }
}
