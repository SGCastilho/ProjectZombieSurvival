using Core.Player;
using Core.Managers;
using UnityEngine;

namespace Core.Events
{
    public sealed class GameplaySceneEvents : MonoBehaviour
    {
        [Header("Classes")]
        [SerializeField] private ObjectPoolingManager _poolingManager;

        private PlayerAttack _playerAttack;

        private void Awake()
        {
            _playerAttack = GameObject.FindObjectOfType<PlayerAttack>();
        }

        private void OnEnable()
        {
            _playerAttack.OnProjectileSpawn += _poolingManager.SpawnPooling;
        }

        private void OnDisable()
        {
            _playerAttack.OnProjectileSpawn -= _poolingManager.SpawnPooling;
        }
    }
}
