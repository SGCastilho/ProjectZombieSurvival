using UnityEngine;

namespace Core.Utilities
{
    public sealed class MoveObjectHorizontal : MonoBehaviour
    {
        #region Encapsulation
        public bool MoveRight 
        {
            get => _moveRight; 
            set 
            {
                if(value == true) { _movimentVector = Vector2.right; }
                else { _movimentVector = Vector2.left; }

                _moveRight = value;
            }
        }

        public float CurrentSpeed { get => _currentMovementSpeed; set => _currentMovementSpeed = value; }
        #endregion

        [Header("Settings")]
        [SerializeField] [Range(2f, 20f)] private float _movementSpeed = 12f;
        [SerializeField] private bool _moveRight = true;

        private Transform _transform;
        private Vector2 _movimentVector;

        private float _currentMovementSpeed;

        private void Awake()
        {
            _transform = GetComponent<Transform>();
            
            MoveRight = _moveRight;
            _currentMovementSpeed = _movementSpeed;
        }

        private void OnDisable()
        {
            transform.position = Vector2.zero;
            transform.eulerAngles = Vector3.zero;

            _currentMovementSpeed = _movementSpeed;
        }

        private void Update()
        {
            _transform.Translate(_currentMovementSpeed * Time.deltaTime * _movimentVector);
        }
    }
}
