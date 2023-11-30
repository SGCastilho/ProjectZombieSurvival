using Core.Enemies;
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

            #region Editor method
            #if UNITY_EDITOR
            [SerializeField] private string _enemyName;

            [Space(8)]
            #endif
            #endregion

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

        [System.Serializable]
        public struct BossSpawner
        {
            #region Encapsulation
            public string Key { get => _bossSpawnKey; }
            #endregion

            #region Editor method
            #if UNITY_EDITOR
            [SerializeField] private string _bossName;

            [Space(8)]
            #endif
            #endregion

            [SerializeField] private string _bossSpawnKey;
        }

        public struct EnemiesChecker
        {
            #region Encapsulation
            public int RealTimeAlive { get => _realEnemiesAlive; set => _realEnemiesAlive = value; }
            public string EnemiesKeys { get => _enemiesKeys; set => _enemiesKeys = value; }
            #endregion

            private int _realEnemiesAlive;
            private string _enemiesKeys;
        }

        [Header("Classes")]
        [SerializeField] private ObjectPoolingManager _poolingManager;

        [Header("Settings")]
        [SerializeField] private EnemiesSpawner[] _enemiesSpawner;

        [Space(6)]

        [SerializeField] private BossSpawner[] _bossesSpawner;

        [Space(8)]

        [SerializeField] private int _currentWave;
        [SerializeField] private int _maxWaves = 5;

        [Space(6)]

        [SerializeField] private int _bossWaveCountdown = 5;

        [Space(6)]

        [SerializeField] private int _enemiesPerWave = 6;
        [SerializeField] private int _fixedIncreaseEnemiesPerWave;
        [SerializeField] [Range(1f, 4f)] private float _nextRoundTimer = 2.8f;

        [Space(6)]

        [SerializeField] [Range(12f, 20f)] private float _spawnDistanceX = 12f;

        [Space(6)]

        [SerializeField] [Range(0.6f, 4f)] private float _spawnTimer = 1.8f;

        private Transform _playerTransform;

        private int _spawnerIndex;
        private int _spawnerReadyIndex;
        private int _currentBoss;
        private int _currentBossCountdown;
        private int _currentSpawnedEnemies;

        private bool _lockSpawn;
        private bool _roundEnded;
        private bool _spawningEnemy;
        private bool _spawnRightSide;
        private bool _startingNewRound;

        private float _currentSpawnTimer;
        private float _currentNextRoundTimer;
        private float _spawnChanceCalculation;

        private EnemyStatus[] _enemiesInScene;
        private EnemiesChecker[] _enemiesCheckers;

        private void Awake() => CacheVariables();

        private void CacheVariables()
        {
            _playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();

            var enemiesInScene = _poolingManager.EnemiesInScene.ToArray();
            _enemiesInScene = new EnemyStatus[enemiesInScene.Length];

            for(int i = 0; i < enemiesInScene.Length; i++)
            {
                _enemiesInScene[i] = enemiesInScene[i].GetComponent<EnemyStatus>();
            }

            _poolingManager.EnemiesInScene.Clear();

            _enemiesCheckers = new EnemiesChecker[_enemiesSpawner.Length];

            for(int i = 0; i < _enemiesCheckers.Length; i++)
            {
                _enemiesCheckers[i].EnemiesKeys = _enemiesSpawner[i].SpawnKey;
            }
        }

        private void OnEnable() => EnableManager();

        private void EnableManager()
        {
            _lockSpawn = false;
            _spawningEnemy = false;
            _spawnRightSide = true;

            _currentBoss = 0;
            _currentWave = 0;
            _spawnerIndex = 0;
            _currentSpawnTimer = 0;

            CheckForReadyIndex();
        }

        private void Update()
        {
            if (_currentSpawnedEnemies >= _enemiesPerWave)
            {
                _lockSpawn = true;
                _roundEnded = true;
                _spawningEnemy = false;
            }

            if (_enemiesSpawner[0].EnemiesInScene >= _enemiesSpawner[0].MaxSpawns)
            {
                _spawningEnemy = false;
                _lockSpawn = true;
            }
            else if (_currentSpawnedEnemies < _enemiesPerWave)
            {
                _lockSpawn = false;
            }

            SpawnTimer();
            NextRoundTimer();
        }

        private void SpawnTimer()
        {
            if (!_roundEnded && !_lockSpawn && !_spawningEnemy)
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

        private void NextRoundTimer()
        {
            if (!_startingNewRound && _roundEnded)
            {
                _currentNextRoundTimer += Time.deltaTime;
                if (_currentNextRoundTimer >= _nextRoundTimer)
                {
                    _startingNewRound = true;
                    _currentNextRoundTimer = 0;

                    NextRound();
                }
            }
        }

        private void SpawnEnemy()
        {
            int currentSpawn = _spawnerIndex;

            if(currentSpawn > 0)
            {
                if(_enemiesSpawner[currentSpawn].EnemiesInScene == _enemiesSpawner[currentSpawn].MaxSpawns)
                {
                    currentSpawn = 0;
                }
                else
                {
                    _spawnChanceCalculation = Random.Range(0.1f, 1f);

                    if(_spawnChanceCalculation > _enemiesSpawner[currentSpawn].ApparitionChance)
                    {
                        currentSpawn = 0;
                    }
                }
            }

            var enemyObject = _poolingManager.SpawnPooling(_enemiesSpawner[currentSpawn].SpawnKey);

            _enemiesSpawner[currentSpawn].EnemiesInScene++;

            if(_spawnerReadyIndex > 0)
            {
                _spawnerIndex++;
                if(_spawnerIndex > _spawnerReadyIndex)
                {
                    _spawnerIndex = 0;
                }
            }

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

            _currentSpawnedEnemies++;

            _spawningEnemy = false;
        }

        private void SpawnBoss()
        {
            var enemyObject = _poolingManager.SpawnPooling(_bossesSpawner[_currentBoss].Key);

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

            _currentBoss++;
            _currentBossCountdown = 0;
        }

        private void NextRound()
        {
            _currentWave++;

            if(_currentWave > _maxWaves) return; //Finalizar o jogo, ou abrir o upgrade

            _currentBossCountdown++;

            for (int i = 0; i < _enemiesSpawner.Length; i++)
            {
                if (_enemiesSpawner[i].WaveApparition >= _currentWave)
                {
                    _enemiesSpawner[i].ReadyToSpawn = true;
                }
            }

            CheckForReadyIndex();
            CheckEnemiesInScene();

            _enemiesPerWave += _fixedIncreaseEnemiesPerWave;

            _currentSpawnedEnemies = 0;

            if(_currentBossCountdown >= _bossWaveCountdown)
            {
                SpawnBoss();

                _currentBossCountdown = 0;
            }

            _lockSpawn = false;
            _roundEnded = false;
            _spawningEnemy = false;
            _startingNewRound = false;
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

        private void CheckEnemiesInScene()
        {
            foreach(EnemyStatus enemyEnabled in _enemiesInScene)
            {
                if(enemyEnabled.gameObject.activeInHierarchy)
                {
                    for(int i = 0; i < _enemiesCheckers.Length; i++)
                    {
                        if(_enemiesCheckers[i].EnemiesKeys == enemyEnabled.Key)
                        {
                            _enemiesCheckers[i].RealTimeAlive++;
                            break;
                        }
                    }
                }
            }

            for(int i = 0; i < _enemiesSpawner.Length; i++)
            {
                _enemiesSpawner[i].EnemiesInScene = _enemiesCheckers[i].RealTimeAlive;
            }

            for(int i = 0; i < _enemiesCheckers.Length; i++)
            {
                _enemiesCheckers[i].RealTimeAlive = 0;
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
