using UnityEngine;

namespace Core.Player
{
    public sealed class PlayerBehaviour : MonoBehaviour
    {
        #region Encapsulation
        public PlayerInput Input { get => _input; }
        public PlayerMovement Moviment { get => _moviment; }
        #endregion

        [Header("Classes")]
        [SerializeField] private PlayerInput _input;
        [SerializeField] private PlayerMovement _moviment;
    }
}
