using System.Collections.Generic;
using UnityEngine;

namespace Core.Managers
{
    [System.Serializable]
    public sealed class Pool
    {
        public Pool(string key, int size, GameObject prefab)
        {
            _poolKey = key;
            _poolSize = size;
            _poolPrefab = prefab;
        }

        #region Encapsulation
        public string Key { get => _poolKey; }
        public int Size { get => _poolSize; }
        public GameObject Prefab { get => _poolPrefab; }
        #endregion

        [Header("Pool Settings")]

        #region Editor Variable
        #if UNITY_EDITOR
        [SerializeField] private string _poolName = "Pool Name";

        [Space(8)]
        #endif
        #endregion

        [SerializeField] private string _poolKey = "pool_key";
        [SerializeField] private int _poolSize = 0;
        [SerializeField] private GameObject _poolPrefab;
    }

    public sealed class ObjectPoolingManager : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private List<Pool> _poolingList;

        private Dictionary<string, Queue<GameObject>> _poolingDictionary;

        private void Awake()
        {
            _poolingDictionary = new Dictionary<string, Queue<GameObject>>();

            InitPooling();
        }

        public void AddPool(Pool pool)
        {
            if(pool == null) return;

            _poolingList.Add(pool);
        }

        public void AddPool(string poolKey, int poolSize, GameObject poolPrefab)
        {
            if(poolKey == null || poolSize <= 0 || poolPrefab == null) return;

            var newPool = new Pool(poolKey, poolSize, poolPrefab);

            _poolingList.Add(newPool);
        }

        private void InitPooling()
        {
            if(_poolingList.Count <= 0) return;

            foreach(Pool pool in _poolingList)
            {
                Queue<GameObject> queue = new Queue<GameObject>();

                for(int i = 0; i < pool.Size; i++)
                {
                    var poolPrefab = Instantiate(pool.Prefab);
                    poolPrefab.SetActive(false);
                    
                    queue.Enqueue(poolPrefab);
                }

                _poolingDictionary.Add(pool.Key, queue);
            }
        }

        public GameObject SpawnPooling(string poolKey, Vector2 posistion)
        {
            var pool = _poolingDictionary[poolKey].Dequeue();

            pool.transform.position = posistion;
            pool.SetActive(true);

            return pool;
        }

        public GameObject SpawnPooling(string poolKey, Vector2 posistion, Quaternion rotation)
        {
            var pool = _poolingDictionary[poolKey].Dequeue();

            pool.transform.position = posistion;
            pool.transform.rotation = rotation;
            pool.SetActive(true);

            return pool;
        }
    }
}
