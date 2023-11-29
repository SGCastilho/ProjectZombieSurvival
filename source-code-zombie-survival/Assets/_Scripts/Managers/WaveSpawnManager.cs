using System.Collections;
using UnityEngine;

namespace Core.Managers
{
    public sealed class WaveSpawnManager : MonoBehaviour
    {
        [System.Serializable]
        public struct EnemiesSpawner
        {
            #region Encapsulation
            public string SpawnKey { get => _enemySpawnKey; }

            public int WaveApparition { get => _waveApparition; }
            public bool ReadyToSpawn { get => _readyToSpawn; set => _readyToSpawn = value; }

            public int MaxSpawns { get => _maxEnemySpawns; }
            public int EnemiesInScene { get => _currentSpawnsInScene; set => _currentSpawnsInScene = value; }

            public float ApparitionChance { get => _apparitionChance; }
            #endregion

            [SerializeField] private string _enemyName;

            [Space(8)]

            [SerializeField] private string _enemySpawnKey;

            [Space(6)]

            [SerializeField] private int _waveApparition;
            [SerializeField] private bool _readyToSpawn;

            [Space(6)]

            [SerializeField] private int _maxEnemySpawns;
            [SerializeField] private int _currentSpawnsInScene;

            [Space(6)]

            [SerializeField] [Range(0.1f, 1f)] private float _apparitionChance;
        }

        [Header("Classes")]
        [SerializeField] private ObjectPoolingManager _poolingManager;

        [Header("Settings")]
        [SerializeField] private EnemiesSpawner[] _enemiesSpawner;

        [Space(8)]

        [SerializeField] [Range(12f, 20f)] private float _spawnDistanceX = 12f;

        [Space(6)]

        [SerializeField] [Range(0.6f, 4f)] private float _spawnTimer = 1.8f;

        private Transform _playerTransform;

        private int _spawnerReadyIndex;

        private bool _lockSpawn;
        private bool _spawningEnemy;
        private bool _spawnRightSide;

        private float _currentSpawnTimer;

        private void Awake() => CacheVariables();

        private void CacheVariables()
        {
            _playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        }

        private void OnEnable() => EnableManager();

        private void EnableManager()
        {
            _lockSpawn = false;
            _spawningEnemy = false;
            _spawnRightSide = true;

            _currentSpawnTimer = 0;

            CheckForReadyIndex();
        }

        private void Update()
        {
            if(_enemiesSpawner[0].EnemiesInScene >= _enemiesSpawner[0].MaxSpawns)
            {
                _lockSpawn = true;
            }
            else
            {
                _lockSpawn = false;
            }

            SpawnTimer();
        }

        private void SpawnTimer()
        {
            if (!_lockSpawn && !_spawningEnemy)
            {
                _currentSpawnTimer += Time.deltaTime;
                if (_currentSpawnTimer >= _spawnTimer)
                {
                    _spawningEnemy = true;
                    _currentSpawnTimer = 0;

                    SpawnEnemy();
                }
            }
        }

        private void SpawnEnemy()
        {
            int spawnSelector = 0;

            if(_spawnerReadyIndex > 0)
            {
                spawnSelector = Random.Range(0, _spawnerReadyIndex);
            }

            var enemyObject = _poolingManager.SpawnPooling(_enemiesSpawner[spawnSelector].SpawnKey);

            _enemiesSpawner[spawnSelector].EnemiesInScene++;

            Vector2 spawnXPosistion = Vector2.zero;

            if(_spawnRightSide)
            {
                spawnXPosistion = new Vector2(_playerTransform.transform.position.x + _spawnDistanceX, 
                    enemyObject.transform.position.y);
            }
            else
            {
                spawnXPosistion = new Vector2(_playerTransform.transform.position.x - _spawnDistanceX, 
                    enemyObject.transform.position.y);
            }

            _spawnRightSide = !_spawnRightSide;

            enemyObject.transform.position = spawnXPosistion;

            _spawningEnemy = false;
        }

        private void CheckForReadyIndex()
        {
            _spawnerReadyIndex = -1;

            for (int i = 0; i < _enemiesSpawner.Length; i++)
            {
                if (_enemiesSpawner[i].ReadyToSpawn)
                {
                    _spawnerReadyIndex++;
                }
            }
        }

        public void DecreaseEnemiesCounter(string enemyKey)
        {
            for(int i = 0; i < _enemiesSpawner.Length; i++)
            {
                if(_enemiesSpawner[i].SpawnKey == enemyKey)
                {
                    _enemiesSpawner[i].EnemiesInScene--;
                    break;
                }
            }
        }
    }
}
