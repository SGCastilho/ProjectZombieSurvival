using Core.Interactables;
using UnityEngine;

namespace Core.Player
{
    public sealed class PlayerInteract : MonoBehaviour
    {
        #region Encapsulation
        public bool CanInteract { get => _canInteract; }
        #endregion

        private const string INTERACTABLE_TAG = "Interactable";

        [Header("Classes")]
        [SerializeField] private PlayerBehaviour _behaviour;

        private bool _canInteract;
        private Interactable _interatactableObject;

        private void OnEnable() 
        {
            _canInteract = false;
        }

        internal void DoInteraction()
        {
            if(!_canInteract || _interatactableObject == null) return;

            InteractableCheck();

            _interatactableObject.gameObject.SetActive(false);
            _interatactableObject = null;
        }

        private void InteractableCheck()
        {
            switch(_interatactableObject.Type)
            {
                case InteractableType.HEALTH:
                    HealthRecovery();
                    break;
                case InteractableType.ULTIMATE:
                    UltimateRecovery();
                    break;
                case InteractableType.WEAPON_RANDOMIZER:
                    WeaponRecovery();
                    break;
            }
        }

        #region Interaction check methods
        private void HealthRecovery()
        {
            int healthAmount = (int)(_behaviour.Status.MaxHealth * _interatactableObject.HealthRecovery);
            
            _behaviour.Status.AddHealth(healthAmount);
        }

        private void UltimateRecovery()
        {
            float ultimateAmount = _behaviour.Status.MaxUltCharge * _interatactableObject.UltimateRecovery;
            
            _behaviour.Status.AddUltimateCharge(ultimateAmount);
        }
        
        private void WeaponRecovery()
        {
            _behaviour.WeaponRotation.AddWeaponToRotation(_interatactableObject.WeaponRecovery);
        }
        #endregion

        #region Collision detection methods
        private void OnTriggerEnter2D(Collider2D other) 
        {
            if(other.CompareTag(INTERACTABLE_TAG))
            {
                _interatactableObject = other.GetComponent<Interactable>();

                _canInteract = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other) 
        {
            if(other.CompareTag(INTERACTABLE_TAG))
            {
                _interatactableObject = null;

                _canInteract = false;
            }
        }
        #endregion
    }
}
