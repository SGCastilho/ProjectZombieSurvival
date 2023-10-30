using UnityEngine.InputSystem;
using UnityEngine;

namespace Core.Player
{
    public sealed class PlayerInput : MonoBehaviour
    {
        #region Encapsulation
        public float HorizontalAxis { get => _horizontalAxis;}
        #endregion

        private GameplayInputActions _inputActions;

        private float _horizontalAxis;

        private void Awake()
        {
            _inputActions = new GameplayInputActions();
        }

        private void OnEnable()
        {
            _inputActions.Enable();
        }

        private void OnDisable()
        {
            _inputActions.Disable();
        }

        private void Update()
        {
            _horizontalAxis = _inputActions.Gameplay.HorizontalAxis.ReadValue<float>();
        }
    }
}
