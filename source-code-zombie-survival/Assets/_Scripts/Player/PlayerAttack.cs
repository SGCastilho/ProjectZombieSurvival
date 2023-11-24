using Core.Utilities;
using Core.Interfaces;
using Core.Projectiles;
using Core.AnimationEvents;
using Core.ScriptableObjects;
using System.Threading.Tasks;
using UnityEngine;

namespace Core.Player
{
    public sealed class PlayerAttack : MonoBehaviour
    {
        #region Delegates
        public delegate GameObject ProjectileSpawn(string poolKey, Vector2 position);
        public delegate void ChangeWeapon(Sprite icon, string name);
        public delegate void CapacityLost(int currentCapacity, int maxCapacity);

        private delegate void Shoot();
        #endregion

        #region Events
        public event ProjectileSpawn OnProjectileSpawn;
        public event ChangeWeapon OnChangeWeapon;
        public event CapacityLost OnCapacityLost;

        private event Shoot OnShoot;
        #endregion

        #region Constants
        private const int BURST_FREQUENCY = 16;
        private const int BURST_BULLET_FREQUENCY = 3;
        private const int SHOTGUN_BULLET_INSTANCES = 3;

        private const string PROJECTILE_KEY = "projectile_default";
        #endregion

        #region Encapsulation
        public WeaponData CurrentWeapon { get => _currentWeapon; }
        public Sprite SetWeaponGraphics { set => _currentWeaponSprite.sprite = value; }
        public WeaponData MeleeWeapon { get => _startedMeleeWeapon; set => _startedMeleeWeapon = value; }

        public int CurrentCapacity { get => _currentWeaponCapacity; }
        #endregion

        [Header("Classes")]
        [SerializeField] private PlayerBehaviour _behaviour;

        [Space(8)]

        [SerializeField] private RayCastAnimationEvent _rayCastAnimationEvent;

        [Header("Settings")]
        [SerializeField] private WeaponData _startedMeleeWeapon;

        [Space(8)]

        [SerializeField] private Transform _shootPosition;

        [Space(8)]

        [SerializeField] [Range(0.6f, 2f)] private float _meleeAttackCouldown = 1f;

        [Space(8)]

        [SerializeField] private WeaponData _currentWeapon;
        [SerializeField] private SpriteRenderer _currentWeaponSprite;
        [SerializeField] private int _currentWeaponCapacity;

        private bool _canAttack;

        private bool _meleeInCouldown;
        private float _currentMeleeAttackCouldown;

        private float _currentFireRate;
        private float _currentFireRateTimer;

        private void Awake() => SetupAttacks();

        private void SetupAttacks()
        {
            _canAttack = true;
            _meleeInCouldown = false;

            _currentWeaponCapacity = 0;
            _currentFireRateTimer = 0;
            _currentMeleeAttackCouldown = 0;
        }

        private void Start() => ChangeCurrentWeapon(_startedMeleeWeapon);

        private void Update()
        {
            AttackTimer();
            MeleeCouldownTimer();
        }

        private void AttackTimer()
        {
            if (!_canAttack)
            {
                _currentFireRateTimer += Time.deltaTime;
                if (_currentFireRateTimer >= _currentFireRate)
                {
                    _canAttack = true;
                    _currentFireRateTimer = 0;
                }
            }
        }

        private void MeleeCouldownTimer()
        {
            if (_meleeInCouldown)
            {
                _currentMeleeAttackCouldown += Time.deltaTime;
                if (_currentMeleeAttackCouldown >= _meleeAttackCouldown)
                {
                    _meleeInCouldown = false;
                    _currentMeleeAttackCouldown = 0;
                }
            }
        }

        public void RangedAttack()
        {
            if(!_canAttack) return;

            if(_currentWeapon != _startedMeleeWeapon)
            {
                if(_currentWeaponCapacity > 0)
                {
                    if(OnShoot == null) return;

                    OnShoot();
                }
                else
                {
                    _currentWeaponCapacity = 0;
                    _behaviour.WeaponRotation.CheckWeaponState();
                }
            }
            else{ MeleeAttack(); }
        }

        public void MeleeAttack()
        {
            if(!_behaviour.Animation.IsMeleeAnimationFinish || _meleeInCouldown) return;

            _behaviour.Animation.CallAttackTrigger();

            _currentFireRate = _startedMeleeWeapon.FireRate;

            _canAttack = false;
            _meleeInCouldown = true;
        }

        public void ApplyMeleeDamage(GameObject damagableObject)
        {
            var enemyToDamage = damagableObject.GetComponent<IDamagable>();

            enemyToDamage.DoDamage(_startedMeleeWeapon.Damage);
        }

        public void UltimateAttack()
        {
            Debug.Log("Ultimate");

            //Fazer ultimate
        }

        public void ChangeCurrentWeapon(WeaponData weaponData)
        {
            if(weaponData == null) return;

            _currentWeapon = weaponData;

            if(weaponData.Type != WeaponType.MELEE)
            {
                _currentWeaponCapacity = weaponData.Capacity;

                CheckForShootType();
            }

            OnChangeWeapon?.Invoke(weaponData.Icon, weaponData.Name);
        }

        private void CheckForShootType()
        {
            OnShoot = null;

            var checkType = _currentWeapon.ShootType;

            switch(checkType)
            {
                case WeaponShootType.BURST:
                    OnShoot += BurstShootType;
                    break;
                case WeaponShootType.SINGLE:
                    OnShoot += SingleShootType;
                    break;
                case WeaponShootType.SPREAD:
                    OnShoot += SpreadShootType;
                    break;
            }
        }

        private async void BurstShootType()
        {
            _canAttack = false;
            _currentFireRate = _currentWeapon.FireRate;

            if(OnProjectileSpawn != null)
            {
                for(int i = 0; i < BURST_BULLET_FREQUENCY; i++)
                {
                    var projectile = OnProjectileSpawn(PROJECTILE_KEY, _shootPosition.position);

                    projectile.GetComponent<MoveObjectHorizontal>().MoveRight = !_behaviour.Moviment.IsFlipped;
                    projectile.GetComponent<ProjectileStats>().Damage = _currentWeapon.Damage;

                    _currentWeaponCapacity--;

                    await Task.Delay(BURST_FREQUENCY);
                }
            }

            OnCapacityLost?.Invoke(_currentWeaponCapacity, _currentWeapon.Capacity);
        }

        private void SingleShootType()
        {
            if(OnProjectileSpawn != null)
            {
                var projectile = OnProjectileSpawn(PROJECTILE_KEY, _shootPosition.position);

                projectile.GetComponent<MoveObjectHorizontal>().MoveRight = !_behaviour.Moviment.IsFlipped;
                projectile.GetComponent<ProjectileStats>().Damage = _currentWeapon.Damage;

                _currentWeaponCapacity--;
            }

            OnCapacityLost?.Invoke(_currentWeaponCapacity, _currentWeapon.Capacity);

            _currentFireRate = _currentWeapon.FireRate;
            _canAttack = false;
        }

        private void SpreadShootType()
        {
            if(OnProjectileSpawn != null)
            {
                for(int i = 0; i < SHOTGUN_BULLET_INSTANCES; i++)
                {
                    var projectile = OnProjectileSpawn(PROJECTILE_KEY, _shootPosition.position);

                    projectile.GetComponent<MoveObjectHorizontal>().MoveRight = !_behaviour.Moviment.IsFlipped;

                    var currentSpeed = projectile.GetComponent<MoveObjectHorizontal>().CurrentSpeed;

                    float randomizeSpeed = Random.Range(currentSpeed / 1.16f, currentSpeed);

                    projectile.GetComponent<MoveObjectHorizontal>().CurrentSpeed = randomizeSpeed;

                    int randomizeRotation = Random.Range(-2, 6);    

                    projectile.transform.eulerAngles = randomizeRotation * Vector3.forward;

                    projectile.GetComponent<ProjectileStats>().Damage = _currentWeapon.Damage;
                }

                _currentWeaponCapacity -= SHOTGUN_BULLET_INSTANCES;
            }

            OnCapacityLost?.Invoke(_currentWeaponCapacity, _currentWeapon.Capacity);

            _currentFireRate = _currentWeapon.FireRate;
            _canAttack = false;
        }
    }
}
