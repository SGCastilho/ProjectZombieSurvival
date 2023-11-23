using Core.Utilities;
using UnityEngine;

namespace Core.Enemies
{
    public sealed class ChasingState : State
    {
        [Header("Exclusive Classes")]
        [SerializeField] private EnemyStateMachine _stateMachine;

        [Space(8)]

        [SerializeField] private State _nextState;

        #region Editor Variable
        #if UNITY_EDITOR
        [SerializeField] private bool _showAttackRangeGizmos = false;
        #endif
        #endregion

        private float _currentRange;

        public override void PlayState()
        {
            _stateMachine.Behaviour.Moviment.CanMove = true;
            
            _currentRange = _stateMachine.CalculateDistanceFromPlayer();

            if(_currentRange <= _stateMachine.AttackRange)
            {
                _stateMachine.Behaviour.Moviment.CanMove = false;

                _stateMachine.ChangeState(_nextState);
            }
        }

        #region Editor Methods
        #if UNITY_EDITOR
        private void OnDrawGizmos() 
        {
            if(_showAttackRangeGizmos)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(transform.position, _stateMachine.AttackRange);
            }
        }
        #endif
        #endregion
    }
}
