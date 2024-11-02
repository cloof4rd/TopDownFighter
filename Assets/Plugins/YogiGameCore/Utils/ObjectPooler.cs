using System.Collections.Generic;
using UnityEngine;
using YogiGameCore.Log;

namespace YogiGameCore.Utils
{
    public class ObjectPooler : MonoSingleton<ObjectPooler>
    {
        private ObjectPooler()
        {
            isGolbal = true;
        }

        [System.Serializable]
        public class Pool
        {
            public string tag;
            public int size;
            public GameObject prefab;
            public Queue<GameObject> poolGos = new Queue<GameObject>();
            public Transform poolsParent;

            public Pool(string tag, int size, GameObject prefab)
            {
                this.tag = tag;
                this.size = size;
                this.prefab = prefab;
            }
        }


        public List<Pool> pools = new List<Pool>();

        public void AddPool(string tag, GameObject prefab, int size = 100)
        {
            AddPool(new Pool(tag, size, prefab));
        }

        public void AddPool(Pool pool)
        {
            // Create a empty gameObject to store pool
            GameObject singlePoolParent = new GameObject(pool.tag);
            singlePoolParent.transform.SetParent(transform);
            pool.poolsParent = singlePoolParent.transform;

            for (int i = 0; i < pool.size; i++)
            {
                GameObject go = GameObject.Instantiate(pool.prefab, pool.poolsParent);
                go.SetActive(false);
                pool.poolGos.Enqueue(go);
            }
            
            pools.Add(pool);
        }

        public bool ContainsPool(string tag)
        {
            foreach (var pool in pools)
            {
                if (pool.tag.Equals(tag))
                    return true;
            }

            return false;
        }

        public T Spawn<T>(string tag, Vector3 pos, Quaternion rot, Transform parent = null)
        {
            GameObject go = null;
            foreach (var pool in pools)
            {
                if (pool.tag.Equals(tag))
                {
                    if (pool.poolGos.Count > 0)
                    {
                        go = pool.poolGos.Dequeue();
                        break;
                    }
                    else
                    {
                        go = GameObject.Instantiate(pool.prefab);
                        break;
                    }
                }
            }

            if (go != null)
            {
                go.SetActive(true);
                go.transform.position = pos;
                go.transform.rotation = rot;
                if (parent != null)
                    go.transform.SetParent(parent);
                else
                    go.transform.parent = null;
            }

            var component = go.GetComponent<T>();
            if (component != null)
            {
                return component;
            }
            LogCore.Error("ObjectPooler.Spawn<T>(): Can't find component of type " + typeof(T).ToString());
            return default(T);
        }

        public void DeSpawn(string tag, GameObject go)
        {
            foreach (var pool in pools)
            {
                if (pool.tag.Equals(tag))
                {
                    go.SetActive(false);
                    go.transform.SetParent(pool.poolsParent);
                    pool.poolGos.Enqueue(go);

                    Trim();
                }
            }
        }

        void Trim()
        {
            foreach (var pool in pools)
            {
                if (pool.size < pool.poolGos.Count)
                {
                    for (int i = 0; i < pool.poolGos.Count - pool.size; i++)
                    {
                        GameObject go = pool.poolGos.Dequeue();
                        Destroy(go);
                    }
                }
            }
        }
    }
}