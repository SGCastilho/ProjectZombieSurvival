using System.Collections.Generic;
using UnityEngine;

namespace Core.Managers
{
    public sealed class EnemiesSpawnerManager : MonoBehaviour
    {
        [System.Serializable]
        public struct EnemiesSpawner
        {
            #region Encapsulation
            public string Key { get => _enemyKey; }

            public int RoundApparition { get => _roundApparition; }
            public bool ReadyToSpawn { get => _readyToSpawn; set => _readyToSpawn = value; }

            public int MaxSpawns { get => _enemyMaxSpawns; }
            public int EnemyInScene { get => _currentEnemyInScene; set => _currentEnemyInScene = value; }
            #endregion

            [Header("Settings")]
            [SerializeField] private string _enemyKey;

            [Space(6)]

            [SerializeField] private int _roundApparition;
            [SerializeField] private bool _readyToSpawn;

            [Space(6)]

            [SerializeField] private int _enemyMaxSpawns;
            [SerializeField] private int _currentEnemyInScene;
        }

        [Header("Classes")]
        [SerializeField] private ObjectPoolingManager _poolingManager;

        [Header("Settings")]
        [SerializeField] private EnemiesSpawner[] _enemiesSpawner;

        [Space(8)]

        [SerializeField] private Transform _leftSpawnPointTransform;
        [SerializeField] private Transform _rightSpawnPointTransform;

        [Space(8)]

        [SerializeField] [Range(14f, 16f)] private float _minSpawnXPosistion = 14f;
        [SerializeField] [Range(16f, 20f)] private float _maxSpawnXPosistion = 16f;

        [Space(16)]

        [SerializeField] private int _maxWaveRounds = 5;
        [SerializeField] private int _bossWaveRound = 5;

        [Space (6)]

        [SerializeField] private int _maxEnemiesPerRound = 8;
        [SerializeField] [Range(0.1f, 1f)] private float _enemiesPerRoundScaling = 0.6f;

        [Space(6)]

        [SerializeField] [Range(0.1f, 2f)] private float _enemiesSpawnTimer = 1f;
        [SerializeField] [Range(0.1f, 0.6f)] private float _minEnemiesSpawnTimer = 0.6f;
        [SerializeField] [Range(0.1f, 1f)] private float _enemiesSpawnTimerScaling = 0.2f;

        [Space(6)]

        [SerializeField] [Range(2f, 6f)] private float _waitingForNewRoundTimer = 3.2f;

        private Transform _transform;
        private Transform _cameraTransform;

        private int _currentRound;
        private int _currentEnemiesInScene;

        private bool _usingRightSpawn;
        private bool _enemySpawnStarted;
        private bool _waitingForNewRound;

        private float _currentEnemiesSpawnTimer;
        private float _currentWaitingForNewRoundTimer;
        private float _randomizeSpawnXPosistion;

        private Vector2 _adjustSpawnPosistion;

        private void Awake()
        {
            _transform = transform;
            _cameraTransform = Camera.main.transform;

            _usingRightSpawn = true;
            _waitingForNewRound = false;
        }

        private void Update() 
        {
            if(!_enemySpawnStarted && !_waitingForNewRound)
            {
                _currentEnemiesSpawnTimer += Time.deltaTime;
                if(_currentEnemiesSpawnTimer >= _enemiesSpawnTimer)
                {
                    _enemySpawnStarted = true;
                    _currentEnemiesSpawnTimer = 0;

                    SpawnEnemy();
                }
            }

            if(_waitingForNewRound)
            {
                _currentWaitingForNewRoundTimer += Time.deltaTime;
                if(_currentWaitingForNewRoundTimer >= _waitingForNewRoundTimer)
                {
                    _currentWaitingForNewRoundTimer = 0;

                    _waitingForNewRound = false;

                    SetNewRound();
                }
            }

            //DEBUG
            if(Input.GetKeyDown(KeyCode.Tab))
            {
                _waitingForNewRound = true;
            }
        }

        private void SpawnEnemy()
        {
            if(!_enemySpawnStarted || _currentEnemiesInScene >= _maxEnemiesPerRound) return;

            _adjustSpawnPosistion = new Vector2(_cameraTransform.position.x, _transform.position.y);
            _transform.position = _adjustSpawnPosistion;

            int randomEnemySpawn = Random.Range(0, _enemiesSpawner.Length);

            if(!_enemiesSpawner[randomEnemySpawn].ReadyToSpawn)
            {
                List<EnemiesSpawner> availableSpawners = new List<EnemiesSpawner>();

                foreach(EnemiesSpawner spawner in _enemiesSpawner)
                {
                    if(spawner.ReadyToSpawn)
                    {
                        availableSpawners.Add(spawner);
                    }
                }

                int selectSpawnerRandom = 0;

                if(availableSpawners.Count > 1)
                {
                    selectSpawnerRandom = Random.Range(0, availableSpawners.Count);
                }
                
                randomEnemySpawn = selectSpawnerRandom;
            }

            var adjustSpawnPos = Vector2.zero;

            if(_enemiesSpawner[randomEnemySpawn].EnemyInScene >= _enemiesSpawner[randomEnemySpawn].MaxSpawns)
            {
                _enemySpawnStarted = false;
                return;
            }

            if(_usingRightSpawn)
            {
                _randomizeSpawnXPosistion = Random.Range(_minSpawnXPosistion, _maxSpawnXPosistion);

                adjustSpawnPos = new Vector2(_randomizeSpawnXPosistion, _rightSpawnPointTransform.position.y);

                _usingRightSpawn = false;
            }
            else
            {
                _randomizeSpawnXPosistion = Random.Range(-_maxSpawnXPosistion, -_minSpawnXPosistion);

                adjustSpawnPos = new Vector2(_randomizeSpawnXPosistion, _rightSpawnPointTransform.position.y);

                _usingRightSpawn = true;
            }

            _poolingManager.SpawnPooling(_enemiesSpawner[randomEnemySpawn].Key, adjustSpawnPos);

            _currentEnemiesInScene++;
            _enemiesSpawner[randomEnemySpawn].EnemyInScene++;

            if(_currentEnemiesInScene >= _maxEnemiesPerRound)
            {
                //Temporario, conforme os inimigos forem morrendo, eles serão subtraidos
                for(int i = 0; i < _enemiesSpawner.Length; i++)
                {
                    if(_enemiesSpawner[i].ReadyToSpawn)
                    {
                        _enemiesSpawner[i].EnemyInScene = 0;
                    }
                    
                    _currentEnemiesInScene = 0;
                }
                //Temporario, conforme os inimigos forem morrendo, eles serão subtraidos

                Debug.Log("Fim do round");
            }
            else
            {
                _enemySpawnStarted = false;
            }
        }

        private void SetNewRound()
        {
            _currentRound++;

            for(int i = 0; i < _enemiesSpawner.Length; i++)
            {
                if(_enemiesSpawner[i].RoundApparition == _currentRound)
                {
                    _enemiesSpawner[i].ReadyToSpawn = true;
                }
            }

            int calculationPerRound = (int) (_maxEnemiesPerRound * _enemiesPerRoundScaling);

            _maxEnemiesPerRound += calculationPerRound;

            float calculationSpawnTimer = _enemiesSpawnTimer * _enemiesSpawnTimerScaling;

            _enemiesSpawnTimer -= calculationSpawnTimer;

            if(_enemiesSpawnTimer < _minEnemiesSpawnTimer)
            {
                _enemiesSpawnTimer = _minEnemiesSpawnTimer;
            }

            _enemySpawnStarted = false;

            //Falta escalonar os status dos inimigos
            
            Debug.Log("Round iniciado");
        }
    }
}
