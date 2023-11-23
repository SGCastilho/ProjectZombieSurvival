using Core.Utilities;
using UnityEngine;

namespace Core.Enemies
{
    public sealed class DeathState : State
    {
        [Header("Exclusive Classes")]
        [SerializeField] private EnemyStateMachine _stateMachine;
        
        public override void PlayState()
        {
            _stateMachine.Behaviour.Moviment.CanMove = false;

            Debug.Log("Dead");

            _stateMachine.Behaviour.gameObject.SetActive(false);

            //Dropar algum coletavel
        }
    }
}
