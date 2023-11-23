using UnityEngine;

namespace Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New EnemyData", menuName = "Data/Create EnemyData")]
    public sealed class EnemyData : ScriptableObject
    {
        #region Encapsulation
        public string Key { get => _enemyKey; }
        public string Name { get => _enemyName; }

        public int MaxHealth { get => _enemyMaxHealth; }
        public int Damage { get => _enemyDamage; }

        public float HealthScaling { get => _healthScaling; }
        public float DamageScaling { get => _damageScaling; }
        #endregion

        [Header("Settings")]
        [SerializeField] private string _enemyKey = "enemy_enemyName";

        [Space(6)]

        [SerializeField] private string _enemyName = "Enemy Name";

        [Space(8)]

        [SerializeField] private int _enemyMaxHealth = 80;
        [SerializeField] private int _enemyDamage = 20;

        [Space(8)]

        [SerializeField] [Range(0.1f, 1f)] private float _healthScaling = 0.8f;
        [SerializeField] [Range(0.1f, 1f)] private float _damageScaling = 0.8f;
    }
}
