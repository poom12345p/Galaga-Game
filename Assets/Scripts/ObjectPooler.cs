using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public struct Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    // Start is called before the first frame update
    void Awake()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach (var pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                objectPool.Enqueue(obj);
                obj.SetActive(false);
            }
            poolDictionary.Add(pool.tag, objectPool);
        }
      
     
    }

    /// <summary>
    /// spawn object in the pool
    /// </summary>
    /// <param name="tag">
    /// pool tag
    /// </param>
    /// <param name="pos">
    /// spawn position
    /// </param>
    /// <param name="rotation">spawn Quaternion</param>
    /// <returns></returns>
    public GameObject SpawnFromPool(string tag,Vector3 pos,Quaternion rotation)
    {
        if(!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("pool \""+tag+"\" does't exist");
            return null;
        }
        GameObject obj = poolDictionary[tag].Dequeue();

        obj.SetActive(true);
        obj.transform.position = pos;
        obj.transform.rotation = rotation;

        PooledObject poolObj = obj.GetComponent<PooledObject>();
        if(poolObj != null)
        {
            poolObj.OnSpawn();
        }

        poolDictionary[tag].Enqueue(obj);
        return obj;
    }

}


