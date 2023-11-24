using Core.Interfaces;
using UnityEngine;

namespace Core.Projectiles
{
    public sealed class ProjectileStats : MonoBehaviour
    {
        #region Encapsulation
        public int Damage { set => _projectileDamage = value; }
        #endregion

        [Header("Settings")]
        [SerializeField] private int _projectileDamage;

        public void ApplyDamageToEnemy(GameObject enemy)
        {
            var applyDamage = enemy.GetComponent<IDamagable>();

            applyDamage.DoDamage(_projectileDamage);

            gameObject.SetActive(false);
        }
    }
}
