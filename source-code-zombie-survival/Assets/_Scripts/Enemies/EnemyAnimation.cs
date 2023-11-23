using UnityEngine;

namespace Core.Enemies
{
    public sealed class EnemyAnimation : MonoBehaviour
    {
        #region Constants
        private const string TRIGGER_ATTACK_KEY = "Attack";
        private const string BOOL_ATTACK_ANIMATION_FINISH_KEY = "IsMeleeAnimationFinish";
        #endregion

        #region Encapsulation
        public bool IsAttackingAnimationFinish { get => _animator.GetBool(BOOL_ATTACK_ANIMATION_FINISH_KEY); }
        #endregion

        [Header("Classes")]
        [SerializeField] private Animator _animator;

        public void CallAttackTrigger()
        {
            _animator.SetTrigger(TRIGGER_ATTACK_KEY);
        }
    }
}
