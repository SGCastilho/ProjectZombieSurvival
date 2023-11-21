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
            
        }
    }
}
