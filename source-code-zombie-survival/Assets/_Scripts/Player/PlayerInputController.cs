using UnityEngine.InputSystem;
using UnityEngine;

namespace Core.Player
{
    public sealed class PlayerInputController : MonoBehaviour
    {
        #region Encapsulation
        public float HorizontalAxis { get => _horizontalAxis;}
        #endregion

        [Header("Classes")]
        [SerializeField] private PlayerBehaviour _behaviour;

        private GameplayInputActions _inputActions;

        private float _horizontalAxis;

        private void Awake() => CacheVariables();

        private void CacheVariables()
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

        #region CallbackContext Inputs
        public void AttackInput(InputAction.CallbackContext context)
        {
            _behaviour.Attack.RangedAttack();
        }

        public void MeleeAttackInput(InputAction.CallbackContext context)
        {
            _behaviour.Attack.MeleeAttack();
        }

        public void UltimateInput(InputAction.CallbackContext context)
        {
            _behaviour.Attack.UltimateAttack();
        }

        public void InteractInput(InputAction.CallbackContext context)
        {
            _behaviour.Interact.DoInteraction();
        }

        public void PauseGameInput(InputAction.CallbackContext context)
        {
            Debug.Log("Pause Game");
        }
        #endregion
    }
}
