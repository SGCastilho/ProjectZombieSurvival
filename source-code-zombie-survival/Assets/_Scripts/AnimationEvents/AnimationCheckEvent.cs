using UnityEngine;

namespace Core.AnimationEvents
{
    public sealed class AnimationCheckEvent : MonoBehaviour
    {
        [Header("Classes")]
        [SerializeField] private Animator _animator;

        [Header("Settings")]
        [SerializeField] private string _animatorKey;

        public void AnimationStarted()
        {
            _animator.SetBool(_animatorKey, false);
        }

        public void AnimationFinish()
        {
            _animator.SetBool(_animatorKey, true);
        }
    }
}
