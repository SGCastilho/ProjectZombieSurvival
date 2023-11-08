using UnityEngine;

namespace Core.ScriptableObjects
{
    public enum WeaponType
    {
        MELEE,
        RIFLE,
        PISTOL,
        SHOTGUN,
        SUBMACHINE,
        ASSAULT_RIFLE
    }

    public enum WeaponShootType
    {
        BURST,
        SINGLE,
        SPREAD
    }

    [CreateAssetMenu(fileName = "New WeaponData", menuName = "Data/Create WeaponData")]
    public sealed class WeaponData : ScriptableObject
    {
        #region Encapsulation
        public string Name { get => _weaponName; }
        public Sprite Icon { get => _weaponIcon; }
        public Sprite Graphics { get => _weaponGraphics; }
        public WeaponType Type { get => _weaponType; }
        public WeaponShootType ShootType { get => _weaponShootType; }

        public int Damage { get => _weaponDamage; }
        public int Capacity { get => _weaponCapacity; }
        public float FireRate { get => _weaponFireRate; }
        #endregion

        [Header("Settings")]
        [SerializeField] private string _weaponName = "Weapon Name";
        [SerializeField] private Sprite _weaponIcon;
        [SerializeField] private Sprite _weaponGraphics;
        [SerializeField] private WeaponType _weaponType = WeaponType.RIFLE;
        [SerializeField] private WeaponShootType _weaponShootType = WeaponShootType.SINGLE;

        [Space(8)]

        [SerializeField] private int _weaponDamage = 10;
        [SerializeField] private int _weaponCapacity = 16;
        [SerializeField] [Range(0.1f, 2f)] private float _weaponFireRate = 1f;
        
        #region Editor Variable
        #if UNITY_EDITOR
        [Space(16)]

        [SerializeField] [Multiline(6)] private string _devNotes = "Put your dev notes here.";
        #endif
        #endregion
    }
}
