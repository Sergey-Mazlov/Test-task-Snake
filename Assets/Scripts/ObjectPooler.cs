using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    #region Singelton
    public static ObjectPooler Instance;
    #endregion

    public List<Pool> pools;
    private Dictionary<string, Queue<GameObject>> _poolDictionary;

    private void Awake()
    {
        Instance = this;
        
        _poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, transform);
                obj.name = pool.tag + "_" + i;
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            _poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!_poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesnt exist.");
            return null;
        }
        GameObject objToSpawn = _poolDictionary[tag].Dequeue();
        objToSpawn.SetActive(true);
        if (objToSpawn.transform.childCount != 0)
        {
            foreach (Transform o in objToSpawn.transform) o.gameObject.SetActive(true);
        }
        objToSpawn.transform.position = position;
        objToSpawn.transform.rotation = rotation;
        
        _poolDictionary[tag].Enqueue(objToSpawn);
        return objToSpawn;
    }

    public void BackToThePool(GameObject go)
    {
        go.transform.SetParent(transform);
        go.transform.rotation = Quaternion.identity;
        go.SetActive(false);
    }
    
}
