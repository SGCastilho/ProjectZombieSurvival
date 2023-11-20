using Core.ScriptableObjects;
using UnityEngine;

namespace Core.Interactables
{
    public enum InteractableType 
    {
        HEALTH,
        ULTIMATE,
        WEAPON_RANDOMIZER
    }

    public sealed class Interactable : MonoBehaviour
    {
        #region Encapsulation
        public InteractableType Type { get => _interactableType; }

        public WeaponData WeaponRecovery { get => _weaponRecovery; }

        public float HealthRecovery { get => _healthRecovery; }
        public float UltimateRecovery { get => _ultimateRecovery; }
        #endregion

        [Header("Settings")]
        [SerializeField] private InteractableType _interactableType;

        [Space(8)]

        [SerializeField] 
        [Range(0.1f, 1f)] 
        [Tooltip("The recovery works with PERCENTAGE")]
        private float _healthRecovery = 0.5f;

        [SerializeField] 
        [Range(0.1f, 1f)]
        [Tooltip("The recovery works with PERCENTAGE")] 
        private float _ultimateRecovery = 0.2f;

        [SerializeField] private WeaponData _weaponRecovery;
    }
}
