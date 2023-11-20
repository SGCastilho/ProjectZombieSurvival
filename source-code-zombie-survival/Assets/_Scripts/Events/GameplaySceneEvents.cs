using Core.UI;
using Core.Player;
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

        private void Awake() => CacheVariables();

        private void CacheVariables()
        {
            _playerBehaviour = GameObject.FindObjectOfType<PlayerBehaviour>();
        }

        private void OnEnable() => EnablePlayerEvents();

        private void EnablePlayerEvents()
        {
            _playerBehaviour.Attack.OnProjectileSpawn += _poolingManager.SpawnPooling;

            _playerBehaviour.Attack.OnChangeWeapon += _gameplayUI.SetWeaponHUD;
            _playerBehaviour.Attack.OnCapacityLost += _gameplayUI.RefreshWeaponHUD;

            _playerBehaviour.WeaponRotation.OnCheckRotationList += _gameplayUI.RefreshWeaponRotation;

            _playerBehaviour.Status.OnChangeHealth += _gameplayUI.RefreshHealthHUD;
            _playerBehaviour.Status.OnChangeUltimateCharge += _gameplayUI.RefreshUltimateHUD;
        }

        private void OnDisable() => DisablePlayerEvents();

        private void DisablePlayerEvents()
        {
            _playerBehaviour.Attack.OnProjectileSpawn -= _poolingManager.SpawnPooling;

            _playerBehaviour.Attack.OnChangeWeapon -= _gameplayUI.SetWeaponHUD;
            _playerBehaviour.Attack.OnCapacityLost -= _gameplayUI.RefreshWeaponHUD;

            _playerBehaviour.WeaponRotation.OnCheckRotationList -= _gameplayUI.RefreshWeaponRotation;

            _playerBehaviour.Status.OnChangeHealth -= _gameplayUI.RefreshHealthHUD;
            _playerBehaviour.Status.OnChangeUltimateCharge -= _gameplayUI.RefreshUltimateHUD;
        }
    }
}
