using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    public static ObjectPooling SharedInstance;
    [SerializeField] List<List<GameObject>> _pooledObjects = new();
    public List<ObjectToPool> ObjectsToPool;

    void Awake()
    {
        SharedInstance = this;
    }

    void Start()
    {
        GameObject tmp;
        for (int i = 0; i < ObjectsToPool.Count; i++)
        {
            var currentList = new List<GameObject>();
            for (int j = 0; j < ObjectsToPool[i].amountToPool; j++)
            {
                tmp = Instantiate(ObjectsToPool[i].Object, transform);
                tmp.SetActive(false);
                currentList.Add(tmp);
            }
            _pooledObjects.Add(currentList);
        }
    }

    public GameObject GetPooledObject(PoolObjectType objectType)
    {
        int objectTypeInt = (int)objectType;
        for (int i = 0; i < _pooledObjects[objectTypeInt].Count; i++)
        {
            if (!_pooledObjects[objectTypeInt][i].activeInHierarchy)
            {
                return _pooledObjects[objectTypeInt][i];
            }
        }
        return null;
    }
}

public enum PoolObjectType
{
    Item,
    Arrow,
}

[System.Serializable]
public struct ObjectToPool
{
    public GameObject Object;
    public PoolObjectType ObjectType;
    public int amountToPool;
}