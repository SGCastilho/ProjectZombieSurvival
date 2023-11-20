using TMPro;
using System.Text;
using Core.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Core.UI
{
    public sealed class GameplayUI : MonoBehaviour
    {
        [System.Serializable]
        public class WeaponRotationUI
        {
            #region Encapsulation
            public GameObject Group { get => _weaponGroup; }
            public Image WeaponImage { get => _weaponImage; }
            public TextMeshProUGUI TMP { get => _weaponNameTMP; }
            #endregion

            [SerializeField] private GameObject _weaponGroup;
            [SerializeField] private Image _weaponImage;
            [SerializeField] private TextMeshProUGUI _weaponNameTMP;
        }

        private const int MAX_SHOWED_WEAPON_ROTATION = 4;

        [Header("Top Classes")]
        [SerializeField] private WeaponRotationUI[] _weaponRotationUI;

        [Space(8)]

        [SerializeField] private TextMeshProUGUI _moneyTMP;
        [SerializeField] private TextMeshProUGUI _enemyKilledsTMP;

        [Header("Bottom Classes")]
        [SerializeField] private Image _weaponIcon;
        [SerializeField] private TextMeshProUGUI _weaponNameTMP;
        [SerializeField] private Image _weaponCapacityBar;
        
        [Space(8)]

        [SerializeField] private Image _healthBar;

        [Space(8)]

        [SerializeField] private Image _ultimateCapacityBar;

        #region Top UI Methods
        public void RefreshWeaponRotation(WeaponData[] weaponRotation)
        {
            int ocuppedIndex = weaponRotation.Length;

            if(ocuppedIndex >= MAX_SHOWED_WEAPON_ROTATION) { ocuppedIndex = MAX_SHOWED_WEAPON_ROTATION; }

            for(int i = 0; i < MAX_SHOWED_WEAPON_ROTATION; i++)
            {
                _weaponRotationUI[i].Group.SetActive(false);
            }

            for(int i = 0; i < ocuppedIndex; i++)
            {
                _weaponRotationUI[i].WeaponImage.sprite = weaponRotation[i].Icon;
                _weaponRotationUI[i].TMP.text = weaponRotation[i].Name;

                _weaponRotationUI[i].Group.SetActive(true);
            }
        }

        public void RefreshMoneyHUD(int currentMoney)
        {
            StringBuilder moneyBuilder = new StringBuilder($"$: {currentMoney}$");
            
            _moneyTMP.text = moneyBuilder.ToString();
        }

        public void RefreshKilledEnemiesHUD(int currentKilledEnemies)
        {
            StringBuilder enemyBuilder = new StringBuilder($"Killeds: {currentKilledEnemies}");
            
            _enemyKilledsTMP.text = enemyBuilder.ToString();
        }
        #endregion

        #region Bottom UI Methods
        public void SetWeaponHUD(Sprite icon, string name)
        {
            _weaponIcon.sprite = icon;
            _weaponNameTMP.text = name;

            _weaponCapacityBar.fillAmount = 1f;
        }

        public void RefreshWeaponHUD(int currentCapacity, int maxCapacity)
        {
            _weaponCapacityBar.fillAmount = ConvertToBarValues(currentCapacity, maxCapacity);
        }

        public void RefreshHealthHUD(int currentHealth, int maxHealth)
        {
            _healthBar.fillAmount = ConvertToBarValues(currentHealth, maxHealth);
        }

        public void RefreshUltimateHUD(float currentCharge, float maxCharge)
        {
            _ultimateCapacityBar.fillAmount = ConvertToBarValues(currentCharge, maxCharge);
        }

        private float ConvertToBarValues(int currentValue, int maxValue)
        {
            float convertedValue = (float)currentValue / maxValue;

            return convertedValue;
        }

        private float ConvertToBarValues(float currentValue, float maxValue)
        {
            float convertedValue = currentValue / maxValue;

            return convertedValue;
        }
        #endregion
    }
}
