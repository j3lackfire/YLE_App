using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//static class, for loading and unloading prefabs
//also for pooling objects and other stuffs.
public class PrefabsManager
{
    public static Dictionary<string, List<PooledObject>> masterPool = new Dictionary<string, List<PooledObject>>();
    public static long objectCounts;

    //on scene reload
    public static void PreparePrefabsManager()
    {
        objectCounts = 0;
        masterPool.Clear();
    }

    //super genius pooling function.
    public static PooledObject GetObjectFromPool(string _objectType, string _resourcePath)
    {
        PooledObject objectPrefab;
        PooledObject returnObject;
        //create a temp list
        List<PooledObject> poolList = null;
        //try to get the list of said value from the pool
        if (masterPool.ContainsKey(_objectType))
        {
            poolList = masterPool[_objectType];
            objectPrefab = poolList[0];
        } else
        {
            //if we don't have that list yet, create it, and load the prefab.
            poolList = new List<PooledObject>();
            masterPool.Add(_objectType, poolList);
            if (Resources.Load(_resourcePath + _objectType) as GameObject == null)
            {
                Debug.Log("Can't load anything " + _resourcePath + " --- " + _objectType);
            }
            objectPrefab = (Resources.Load(_resourcePath + _objectType) as GameObject).GetComponent<PooledObject>();
            //the first value of a list will be saved as the prefab for others to instantiate from.
            poolList.Add(objectPrefab);
        }

        if (poolList.Count <= 1)
        {
            //if the pool is empty, we must instantiate a new object
            returnObject = GameObject.Instantiate<PooledObject>(objectPrefab);
            returnObject.SetPool(poolList);
            returnObject.OnFirstInit();
        }
        else
        {
            //0 is the prefab reference.
            //When we get a value from the pool, we get the first object in the pool
            //Which is object 1, because 0 is just a reference.
            //Then, remove it from the pool because it's not reserved anymore.
            returnObject = poolList[1];
            poolList.RemoveAt(1);
            returnObject.gameObject.SetActive(true);
        }

        //the ID is for checking and targetting
        //in case of cosmestic object, then there is no need for targetting or checking, so not increase the value
        if (returnObject.RequireID())
        {
            returnObject.SetID(objectCounts);
            objectCounts++;
        }
        else
        {
            returnObject.SetID(-1);
        }
        return returnObject;
    }

    public static T SpawnPrefab<T>(string prefabName, string prefabPath) where T : PooledObject
    {
        return GetObjectFromPool(prefabName, prefabPath) as T;
    }
}
