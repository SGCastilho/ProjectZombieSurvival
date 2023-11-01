using UnityEngine;

namespace Core.Player
{
    public sealed class PlayerAnimation : MonoBehaviour
    {
        [Header("Classes")]
        [SerializeField] private Animator _animator;

        private const string TRIGGER_ATTACK_KEY = "Attack";
        private const string BOOL_ISMELEE_ATTACK_KEY = "IsMeleeAttack";
        private const string BOOL_ISMELEE_ANIMATION_FINISH_KEY = "IsMeleeAnimationFinish";

        public bool IsMeleeAttack 
        {
            set 
            {
                _animator.SetBool(BOOL_ISMELEE_ATTACK_KEY, value);
            }
        }

        public bool IsMeleeAnimationFinish
        {
            get 
            {
                return _animator.GetBool(BOOL_ISMELEE_ANIMATION_FINISH_KEY);
            }
        }

        public void CallAttackTrigger()
        {
            _animator.SetTrigger(TRIGGER_ATTACK_KEY);
        }
    }
}
