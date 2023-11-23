using Core.ScriptableObjects;
using Core.Utilities;
using UnityEngine;

namespace Core.Enemies
{
    public sealed class EnemyStatus : MonoBehaviour
    {
        [Header("Classes")]
        [SerializeField] private EnemyData _enemyData;

        [Space(8)]

        [SerializeField] private EnemyBehaviour _behaviour;
        [SerializeField] private State _deathState;

        [Header("Settings")]
        [SerializeField] private int _enemyHealth;
        [SerializeField] private int _enemyDamage;

        private int _scaledHealth;
        private int _scaledDamage;
        
        private void Awake() 
        {
            _scaledDamage = _enemyData.Damage;
            _scaledHealth = _enemyData.MaxHealth;
        }

        private void OnEnable()
        {
            AddHealth(_scaledHealth);
            
            _enemyDamage =_scaledDamage;
        }

        public void AddHealth(int amount)
        {
            _enemyHealth += amount;
            if(_enemyHealth > _scaledHealth)
            {
                _enemyHealth = _scaledHealth;
            }
        }

        public void RemoveHealth(int amount)
        {
            _enemyHealth -= amount;
            if(_enemyHealth <= 0)
            {
                _enemyHealth = 0;

                _behaviour.StateMachine.ChangeState(_deathState);
            }
        }

        public void ScaleEnemy()
        {
            var healthScaling = (int)(_enemyData.MaxHealth * _enemyData.HealthScaling);

            _scaledHealth += healthScaling;

            var damageScaling = (int)(_enemyData.Damage * _enemyData.DamageScaling);

            _scaledDamage += damageScaling;
        }
    }
}