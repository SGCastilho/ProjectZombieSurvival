using UnityEngine;

namespace Core.Enemies
{
    public sealed class EnemyBehaviour : MonoBehaviour
    {
        #region Encapsulation
        public EnemyMoviment Moviment { get => _enemyMoviment; }
        public EnemyStateMachine StateMachine { get => _stateMachine; }
        #endregion

        [Header("Classes")]
        [SerializeField] private EnemyMoviment _enemyMoviment;
        [SerializeField] private EnemyStateMachine _stateMachine;
    }
}
