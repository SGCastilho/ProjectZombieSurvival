using TMPro;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public sealed class GameplayUI : MonoBehaviour
    {
        [Header("Top Classes")]
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

            Debug.Log(ConvertToBarValues(currentCapacity, maxCapacity));
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
