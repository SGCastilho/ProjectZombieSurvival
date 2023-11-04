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
            //Aplicar dano ao inimigo
        }
    }
}
