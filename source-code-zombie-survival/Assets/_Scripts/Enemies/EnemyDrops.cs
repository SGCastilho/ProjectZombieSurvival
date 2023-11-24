using UnityEngine;

namespace Core.Enemies
{
    public sealed class EnemyDrops : MonoBehaviour
    {
        [System.Serializable]
        public class DropChance
        {
            #region Encapsulation
            public string Key { get => _dropKey; }
            public float Chance { get => _dropChance; }
            #endregion
            
            #region Editor Variable
            #if UNITY_EDITOR
            [SerializeField] private string _dropName = "Drop Name";

            [Space(8)]
            #endif
            #endregion

            [SerializeField] private string _dropKey = "drop_key";
            [SerializeField] [Range(0.1f, 1f)] private float _dropChance = 0.6f;
        }

        public delegate GameObject DropSpawn(string poolKey, Vector2 position);
        public event DropSpawn OnDropSpawn;

        [Header("Settings")]
        [SerializeField] [Range(0.1f, 1f)] private float _doubleDropChance = 0.8f;

        [Space(6)]

        [SerializeField] private DropChance[] _possibleDrops;

        [Space(8)]

        [SerializeField] private Transform _spawnTransform;
        [SerializeField] [Range(0.1f, 1f)] private float _minSpawnXRandomizer;
        [SerializeField] [Range(1f, 2f)] private float _maxSpawnXRandomizer;

        private int _dropAmount;
        private int _currentDropAmount;

        private bool _doubleDropActive;

        private float _chanceCalculation;
        private float _doubleDropchanceCalculation;

        private Vector2 _randomizeDropPosition;

        private void OnEnable() 
        {
            _dropAmount = 1;
            _currentDropAmount = 0;
            _doubleDropActive = false;
        }

        public void SpawnDrop()
        {
            if(_possibleDrops.Length <= 0 || _possibleDrops == null) return;

            _doubleDropchanceCalculation = Random.Range(0.1f, 1f);

            if(_doubleDropchanceCalculation <= _doubleDropChance)
            {
                _dropAmount = 2;
                _doubleDropActive = true;
            }

            for(int i = 0; i < _possibleDrops.Length; i++)
            {
                if(_currentDropAmount >= _dropAmount) break;

                _chanceCalculation = Random.Range(0.1f, 1f);

                if(_chanceCalculation <= _possibleDrops[i].Chance)
                {
                    float dropXPosistion;

                    if(_currentDropAmount >= 1)
                    {
                        dropXPosistion = _spawnTransform.position.x - Random.Range(_minSpawnXRandomizer, 
                            _maxSpawnXRandomizer);
                    }
                    else
                    {
                        dropXPosistion = _spawnTransform.position.x + Random.Range(_minSpawnXRandomizer, 
                            _maxSpawnXRandomizer);
                    }

                    _randomizeDropPosition = new Vector2(dropXPosistion, _spawnTransform.position.y);

                    OnDropSpawn?.Invoke(_possibleDrops[i].Key, _randomizeDropPosition);

                    _currentDropAmount++;

                    if(!_doubleDropActive) break;
                }
            }
        }
    }
}
