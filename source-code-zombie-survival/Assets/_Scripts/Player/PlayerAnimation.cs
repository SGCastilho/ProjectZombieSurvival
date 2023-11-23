using UnityEngine;

namespace Core.Player
{
    public sealed class PlayerAnimation : MonoBehaviour
    {
        #region Constants
        private const string TRIGGER_ATTACK_KEY = "Attack";
        private const string BOOL_ISMELEE_ATTACK_KEY = "IsMeleeAttack";
        private const string BOOL_ISMELEE_ANIMATION_FINISH_KEY = "IsMeleeAnimationFinish";
        #endregion

        #region Encapsulation
        public bool IsMeleeAttack { set => _animator.SetBool(BOOL_ISMELEE_ATTACK_KEY, value); }
        public bool IsMeleeAnimationFinish { get => _animator.GetBool(BOOL_ISMELEE_ANIMATION_FINISH_KEY); }
        #endregion

        [Header("Classes")]
        [SerializeField] private Animator _animator;

        public void CallAttackTrigger()
        {
            _animator.SetTrigger(TRIGGER_ATTACK_KEY);
        }
    }
}
