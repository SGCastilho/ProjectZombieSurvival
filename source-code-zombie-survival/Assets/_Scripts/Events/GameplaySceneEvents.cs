using Core.UI;
using Core.Player;
using Core.Enemies;
using Core.Managers;
using UnityEngine;

namespace Core.Events
{
    public sealed class GameplaySceneEvents : MonoBehaviour
    {
        [Header("Classes")]
        [SerializeField] private ObjectPoolingManager _poolingManager;

        [SerializeField] private GameplayUI _gameplayUI;

        private PlayerBehaviour _playerBehaviour;

        private EnemyBehaviour[] _spawnedEnemies;

        private void Awake() => CacheVariables();

        private void CacheVariables()
        {
            _playerBehaviour = FindObjectOfType<PlayerBehaviour>();

            CacheEnemiesInScene();
        }

        private void CacheEnemiesInScene()
        {
            GameObject[] enemiesInScene = _poolingManager.EnemiesInScene.ToArray();

            _spawnedEnemies = new EnemyBehaviour[enemiesInScene.Length];

            for (int i = 0; i < _spawnedEnemies.Length; i++)
            {
                _spawnedEnemies[i] = enemiesInScene[i].GetComponent<EnemyBehaviour>();
            }

            _poolingManager.EnemiesInScene.Clear();
        }

        private void OnEnable() => EnableEvents();

        private void EnableEvents()
        {
            EnablePlayerEvents();
            EnableEnemiesEvents();
        }

        private void EnablePlayerEvents()
        {
            _playerBehaviour.Attack.OnProjectileSpawn += _poolingManager.SpawnPooling;

            _playerBehaviour.Attack.OnChangeWeapon += _gameplayUI.SetWeaponHUD;
            _playerBehaviour.Attack.OnCapacityLost += _gameplayUI.RefreshWeaponHUD;

            _playerBehaviour.WeaponRotation.OnCheckRotationList += _gameplayUI.RefreshWeaponRotation;

            _playerBehaviour.Status.OnChangeHealth += _gameplayUI.RefreshHealthHUD;
            _playerBehaviour.Status.OnChangeUltimateCharge += _gameplayUI.RefreshUltimateHUD;
        }

        private void EnableEnemiesEvents()
        {
            foreach(EnemyBehaviour enemy in _spawnedEnemies)
            {
                enemy.Drops.OnDropSpawn += _poolingManager.SpawnPooling;
            }
        }

        private void OnDisable() => DisableEvents();

        private void DisableEvents()
        {
            DisablePlayerEvents();
            DisableEnemiesEvents();
        }

        private void DisablePlayerEvents()
        {
            _playerBehaviour.Attack.OnProjectileSpawn -= _poolingManager.SpawnPooling;

            _playerBehaviour.Attack.OnChangeWeapon -= _gameplayUI.SetWeaponHUD;
            _playerBehaviour.Attack.OnCapacityLost -= _gameplayUI.RefreshWeaponHUD;

            _playerBehaviour.WeaponRotation.OnCheckRotationList -= _gameplayUI.RefreshWeaponRotation;

            _playerBehaviour.Status.OnChangeHealth -= _gameplayUI.RefreshHealthHUD;
            _playerBehaviour.Status.OnChangeUltimateCharge -= _gameplayUI.RefreshUltimateHUD;
        }

        private void DisableEnemiesEvents()
        {
            foreach(EnemyBehaviour enemy in _spawnedEnemies)
            {
                enemy.Drops.OnDropSpawn -= _poolingManager.SpawnPooling;
            }
        }
    }
}
