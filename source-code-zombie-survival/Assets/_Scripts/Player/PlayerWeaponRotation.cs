using Core.ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Player
{
    public sealed class PlayerWeaponRotation : MonoBehaviour
    {
        public delegate void CheckRotationList(WeaponData[] weaponRotation);

        public event CheckRotationList OnCheckRotationList;

        [Header("Classes")]
        [SerializeField] private PlayerBehaviour _behaviour;

        [Header("Settings")]
        [SerializeField] private List<WeaponData> _weaponRotationList;

        private void Awake() => CacheVariables();

        private void CacheVariables()
        {
            _weaponRotationList = new List<WeaponData>();
        }

        public void AddWeaponToRotation(WeaponData weaponData)
        {
            if(weaponData== null) return; 

            _weaponRotationList.Add(weaponData);

            CheckWeaponState();
        }

        public void CheckWeaponState()
        {
            if(_weaponRotationList.Count > 0)
            {
                if(_behaviour.Attack.CurrentCapacity < 1)
                {
                    AddWeaponFromRotation();
                }

                if(_behaviour.Attack.CurrentWeapon == _behaviour.Attack.MeleeWeapon)
                {
                    AddWeaponFromRotation();
                }

                OnCheckRotationList?.Invoke(_weaponRotationList.ToArray());
            }
            else if(_weaponRotationList.Count == 0)
            {
                _behaviour.Attack.ChangeCurrentWeapon(_behaviour.Attack.MeleeWeapon);
                _behaviour.Attack.SetWeaponGraphics = _behaviour.Attack.MeleeWeapon.Graphics;
            }
        }

        private void AddWeaponFromRotation()
        {
            if(_weaponRotationList[0] == null) return;

            var weaponToEquip = _weaponRotationList[0];
            _weaponRotationList.Remove(weaponToEquip);

            _behaviour.Attack.ChangeCurrentWeapon(weaponToEquip);
            _behaviour.Attack.SetWeaponGraphics = weaponToEquip.Graphics;
        }
    }
}
