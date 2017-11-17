using UnityEngine;
using System.Collections.Generic;

public class PooledObject : MonoBehaviour
{
    [System.NonSerialized]
    private long id;

    [System.NonSerialized]
    protected List<PooledObject> myPool;

    public virtual void OnFirstInit() {}
    
    //called by Prefab manager, don't need to worry about this function.
    public void SetPool(List<PooledObject> pool)
    {
        myPool = pool;
    }

    //should this function be called internally or externally ?
    //It makes a little more sense to call it externally.
    protected void ReturnToPool()
    {
        if (myPool == null)
        {
            Destroy(this.gameObject);
            return;
        }
        myPool.Add(this);
        gameObject.SetActive(false);
    }

    //Asthetic object like corpse, or object that is not reference and check, does not need to have ID.
    public virtual bool RequireID() { return true; }

    //only set by the pool
    //maybe remove this function and put this is the 
    //call from pool or something
    public void SetID(long _id) { id = _id; }

    //get ID, usually called to check for validity of the object
    public long GetID() { return id; }
}
