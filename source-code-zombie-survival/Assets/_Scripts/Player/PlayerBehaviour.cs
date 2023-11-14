using UnityEngine;

namespace Core.Player
{
    public sealed class PlayerBehaviour : MonoBehaviour
    {
        #region Encapsulation
        public PlayerStatus Status { get => _status; }
        public PlayerInputController Input { get => _input; }
        public PlayerAttack Attack { get => _attack; }
        public PlayerMovement Moviment { get => _moviment; }
        public PlayerAnimation Animation { get => _animation; }
        public PlayerWeaponRotation WeaponRotation { get => _weaponRotation; }
        public PlayerInteract Interact { get => _interact; }
        #endregion

        [Header("Classes")]
        [SerializeField] private PlayerStatus _status;
        [SerializeField] private PlayerInputController _input;
        [SerializeField] private PlayerAttack _attack;
        [SerializeField] private PlayerMovement _moviment;
        [SerializeField] private PlayerAnimation _animation;
        [SerializeField] private PlayerWeaponRotation _weaponRotation;
        [SerializeField] private PlayerInteract _interact;
    }
}
