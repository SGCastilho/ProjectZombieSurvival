using Core.Utilities;
using UnityEngine;

namespace Core.Enemies
{
    public sealed class AttackingState : State
    {
        [Header("Exclusive Classes")]
        [SerializeField] private EnemyStateMachine _stateMachine;

        [Space(8)]

        [SerializeField] private State _nextState;

        [Header("Exclusive Settings")]
        [SerializeField] [Range(0.2f, 2f)] private float _attackCouldown;

        private bool _canAttack;
        private float _currentAttackCouldown;

        private void OnEnable() 
        {
            _canAttack = true;
        }
        
        public override void PlayState()
        {
            if(_canAttack)
            {
                if(_stateMachine.Behaviour.Animation.IsAttackingAnimationFinish)
                {
                    _stateMachine.Behaviour.Moviment.FlipGraphics();

                    _stateMachine.Behaviour.Animation.CallAttackTrigger();

                    _canAttack = false;
                }
            }
            else
            {
                _currentAttackCouldown += Time.deltaTime;
                if(_currentAttackCouldown >= _attackCouldown)
                {
                    _currentAttackCouldown = 0;
                    
                    if(_stateMachine.CalculateDistanceFromPlayer() <= _stateMachine.AttackRange)
                    {
                        _canAttack = true;
                    }
                    else
                    {
                        _canAttack = true;
                        _stateMachine.ChangeState(_nextState);
                    }
                }
            }
        }
    }
}
