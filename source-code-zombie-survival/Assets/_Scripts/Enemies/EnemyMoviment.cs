using UnityEngine;

namespace Core.Enemies
{
    public sealed class EnemyMoviment : MonoBehaviour
    {
        #region Encapsulation
        public bool CanMove 
        { 
            set
            {
                _rb2D.velocity = Vector2.zero;
                _canMove = value;
            }
        }
        #endregion

        [Header("Classes")]
        [SerializeField] private EnemyBehaviour _behaviour;

        [Space(8)]

        [SerializeField] private Rigidbody2D _rb2D;
        [SerializeField] private Transform _flipGroupTransform;

        [Header("Settings")]
        [SerializeField] [Range(2f, 20f)] private float _movimentSpeed = 6f;
        [SerializeField] private bool _canMove;
        [SerializeField] private bool _isFlipped;

        private Transform _transform;

        private Vector2 _currentScaling;

        private void Awake()
        {
            _transform = GetComponent<Transform>();

            _currentScaling = _flipGroupTransform.localScale;
        }

        private void OnEnable() 
        {
            _canMove = true;
            
            FlipGraphics();
        }

        private void FixedUpdate() => Moviment();

        private void Moviment()
        {
            if (!_canMove) return;

            FlipGraphics();

            if (_isFlipped)
            {
                _rb2D.velocity = _movimentSpeed * Vector2.left;
            }
            else
            {
                _rb2D.velocity = _movimentSpeed * Vector2.right;
            }
        }

        internal void FlipGraphics()
        {
            _isFlipped = FlipToPlayer();

            if(_isFlipped)
            {
                _flipGroupTransform.localScale = new Vector2(-_currentScaling.x, _currentScaling.y);
            }
            else
            {
                _flipGroupTransform.localScale = new Vector2(_currentScaling.x, _currentScaling.y);
            }
        }

        private bool FlipToPlayer()
        {
            var playerPos = _behaviour.StateMachine.Player.transform;

            if(_transform.position.x > playerPos.transform.position.x)
            {
                return true;
            }

            return false;
        }
    }
}
