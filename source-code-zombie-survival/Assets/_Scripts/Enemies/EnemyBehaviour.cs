using UnityEngine;

namespace Core.Enemies
{
    public sealed class EnemyBehaviour : MonoBehaviour
    {
        #region Encapsulation
        public EnemyStatus Status { get => _status; }
        public EnemyMoviment Moviment { get => _enemyMoviment; }
        public EnemyAnimation Animation { get => _enemyAnimation; }
        public EnemyStateMachine StateMachine { get => _stateMachine; }
        public EnemyDrops Drops { get => _enemyDrops; }
        #endregion

        [Header("Classes")]
        [SerializeField] private EnemyStatus _status;
        [SerializeField] private EnemyMoviment _enemyMoviment;
        [SerializeField] private EnemyAnimation _enemyAnimation;
        [SerializeField] private EnemyStateMachine _stateMachine;
        [SerializeField] private EnemyDrops _enemyDrops;
    }
}
