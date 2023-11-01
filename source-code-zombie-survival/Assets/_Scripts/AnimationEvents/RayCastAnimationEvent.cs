using UnityEngine.Events;
using UnityEngine;

namespace Core.AnimationEvents
{
    public sealed class RayCastAnimationEvent : MonoBehaviour
    {
        #region Encapsulation
        public GameObject RayCastTarget { get => _rayCastTarget; }
        #endregion

        [Header("Settings")]
        [SerializeField] private Transform _raycastSpawn;
        [SerializeField] private LayerMask _targetLayer;

        [Space(8)]

        [SerializeField] private float _raycastRange;

        [Space(8)]

        [SerializeField] private UnityEvent OnRaycastDrawed;
        
        private GameObject _rayCastTarget;

        #region Editor Variables
        #if UNITY_EDITOR
        [SerializeField] private bool _showRayCast;
        #endif
        #endregion

        public void DrawRaycast()
        {
            var target = Physics2D.OverlapCircle(_raycastSpawn.position, _raycastRange, _targetLayer);

            if(target == null) return;

            _rayCastTarget = target.gameObject;

            OnRaycastDrawed.Invoke();
        }

        #region Editor Methods
        #if UNITY_EDITOR
        public void OnDrawGizmos()
        {
            if(!_showRayCast) return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_raycastSpawn.position, _raycastRange);
        }
        #endif
        #endregion
    }
}
