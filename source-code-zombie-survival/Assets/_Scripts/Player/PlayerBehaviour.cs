using UnityEngine;

namespace Core.Player
{
    public sealed class PlayerBehaviour : MonoBehaviour
    {
        #region Encapsulation
        public PlayerInputController Input { get => _input; }
        public PlayerAttack Attack { get => _attack; }
        public PlayerMovement Moviment { get => _moviment; }
        public PlayerAnimation Animation { get => _animation; }
        #endregion

        [Header("Classes")]
        [SerializeField] private PlayerInputController _input;
        [SerializeField] private PlayerAttack _attack;
        [SerializeField] private PlayerMovement _moviment;
        [SerializeField] private PlayerAnimation _animation;
    }
}
