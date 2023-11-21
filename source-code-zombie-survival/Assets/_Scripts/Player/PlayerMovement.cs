using UnityEngine;

namespace Core.Player
{
    public sealed class PlayerMovement : MonoBehaviour
    {
        #region Encapsulation
        public bool IsFlipped { get => _isFlipped; }
        #endregion

        [Header("Classes")]
        [SerializeField] private PlayerBehaviour _playerBehaviour;

        [Space(8)]

        [SerializeField] private Rigidbody2D _rigidBody2D;

        [Header("Settings")]
        [SerializeField] private Transform _flipGroup;
        [SerializeField] private bool _isFlipped;

        [Space(8)]

        [SerializeField] [Range(1f, 20f)] private float _movimentSpeed = 6f;

        private float _xVelocity;
        private Vector2 _finalVelocity;

        private void FixedUpdate()
        {
            Moviment();
            FlipGraphics();
        }

        private void Moviment()
        {
            _xVelocity = _playerBehaviour.Input.HorizontalAxis * _movimentSpeed;
            _finalVelocity = new Vector2(_xVelocity, _rigidBody2D.velocity.y);

            _rigidBody2D.velocity = _finalVelocity;
        }

        private void FlipGraphics()
        {
            if (_playerBehaviour.Input.HorizontalAxis > 0)
            {
                _flipGroup.localScale = new Vector2(1f, 1f);
                _isFlipped = false;
            }
            else if (_playerBehaviour.Input.HorizontalAxis < 0)
            {
                _flipGroup.localScale = new Vector2(-1f, 1f);
                _isFlipped = true;
            }
        }
    }
}
