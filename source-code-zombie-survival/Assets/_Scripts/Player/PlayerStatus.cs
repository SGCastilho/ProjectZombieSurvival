using UnityEngine;

namespace Core.Player
{
    public sealed class PlayerStatus : MonoBehaviour
    {
        #region Encapsulation
        public int Health { get => _playerHealth; }
        public int MaxHealth { get => _playerMaxHealth; }

        public float UltCharge { get => _playerUltimateCharge; }
        public float MaxUltCharge { get => _playerMaxUltimateCharge; }
        #endregion

        [Header("Settings")]
        [SerializeField] private int _playerHealth;
        [SerializeField] private int _playerMaxHealth = 100;

        [Space(8)]

        [SerializeField] private float _playerUltimateCharge;
        [SerializeField] [Range(1f, 6f)] private float _playerMaxUltimateCharge = 4f;

        private void OnEnable()
        {
            AddHealth(_playerMaxHealth);
        }

        public void AddHealth(int healthToAdd)
        {
            _playerHealth += healthToAdd;
            if(_playerHealth > _playerMaxHealth)
            {
                _playerHealth = _playerMaxHealth;
            }
        }

        public void RemoveHealth(int healthToRemove)
        {
            _playerHealth -= healthToRemove;
            if(_playerHealth <= 0)
            {
                _playerHealth = 0;

                //Death function
            }
        }

        public void AddUltimateCharge(float chargeToAdd)
        {
            _playerUltimateCharge += chargeToAdd;
            if(_playerUltimateCharge > _playerMaxUltimateCharge)
            {
                _playerUltimateCharge = _playerMaxUltimateCharge;
            }
        }

        public void RemoveUltimateCharge(float chargeToRemove)
        {
            _playerUltimateCharge -= chargeToRemove;
            if(_playerUltimateCharge <= 0)
            {
                _playerUltimateCharge = 0;
            }
        }
    }
}