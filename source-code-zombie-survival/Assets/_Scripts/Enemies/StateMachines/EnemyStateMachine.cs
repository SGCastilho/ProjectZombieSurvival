using Core.Player;
using Core.Utilities;
using UnityEngine;

namespace Core.Enemies
{
    public sealed class EnemyStateMachine : StateMachine
    {
        #region Encapsulation
        public PlayerBehaviour Player { get => _playerBehaviour; }
        public EnemyBehaviour Behaviour { get => _enemyBehaviour; }

        internal float AttackRange { get => _attackRange; }
        #endregion

        [Header("Exclusive Classes")]
        [SerializeField] private EnemyBehaviour _enemyBehaviour;

        [Header("Exclusive Settings")]
        [SerializeField] [Range(1f, 20f)] private float _attackRange = 2f;

        private PlayerBehaviour _playerBehaviour;

        protected override void Awake()
        {
            base.Awake();

            _playerBehaviour = FindObjectOfType<PlayerBehaviour>();
        }

        protected override void Update()
        {
            base.Update();
        }

        internal float CalculateDistanceFromPlayer()
        {
            return Mathf.Abs(_playerBehaviour.transform.position.x - _enemyBehaviour.transform.position.x);
        }
    }
}
