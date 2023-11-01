using Core.AnimationEvents;
using UnityEngine;

namespace Core.Player
{
    public sealed class PlayerAttack : MonoBehaviour
    {
        [Header("Classes")]
        [SerializeField] private PlayerBehaviour _behaviour;

        [Space(8)]

        [SerializeField] private RayCastAnimationEvent _rayCastAnimationEvent;

        public void RangedAttack()
        {
            MeleeAttack();
        }

        public void MeleeAttack()
        {
            if(!_behaviour.Animation.IsMeleeAnimationFinish) return;

            _behaviour.Animation.CallAttackTrigger();
        }

        public void ApplyMeleeDamage()
        {
            Debug.Log("ApplyDamage");
        }
    }
}
