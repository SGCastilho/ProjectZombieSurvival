using UnityEngine;

namespace Core.Player
{
    public sealed class PlayerMoviment : MonoBehaviour
    {
        [Header("Classes")]
        [SerializeField] private PlayerBehaviour _playerBehaviour;

        [Space(8)]

        [SerializeField] private Rigidbody2D _rigidBody2D;

        [Header("Settings")]
        [SerializeField] [Range(1f, 20f)] private float _movimentSpeed = 6f;

        private float _xVelocity;
        private Vector2 _finalVelocity;

        private void FixedUpdate()
        {
            _xVelocity = _playerBehaviour.Input.HorizontalAxis * _movimentSpeed;
            _finalVelocity = new Vector2(_xVelocity, _rigidBody2D.velocity.y);

            _rigidBody2D.velocity = _finalVelocity;
        }
    }
}
