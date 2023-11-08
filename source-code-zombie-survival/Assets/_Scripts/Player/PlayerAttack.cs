using Core.Utilities;
using Core.AnimationEvents;
using Core.ScriptableObjects;
using System.Threading.Tasks;
using UnityEngine;

namespace Core.Player
{
    public sealed class PlayerAttack : MonoBehaviour
    {
        public delegate GameObject ProjectileSpawn(string poolKey, Vector2 position);
        public event ProjectileSpawn OnProjectileSpawn;

        private delegate void Shoot();
        private event Shoot OnShoot;

        private const int BURST_FREQUENCY = 16;
        private const int BURST_BULLET_FREQUENCY = 3;
        private const int SHOTGUN_BULLET_INSTANCES = 3;

        private const string PROJECTILE_KEY = "projectile_default";

        #region Encapsulation
        public WeaponData CurrentWeapon { get => _currentWeapon; }
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
        [SerializeField] private int _currentWeaponCapacity;

        private bool _canAttack;

        private bool _meleeInCouldown;
        private float _currentMeleeAttackCouldown;

        private float _currentFireRate;
        private float _currentFireRateTimer;

        private void Awake()
        {
            _canAttack = true;
            _meleeInCouldown = false;

            _currentWeaponCapacity = 0;
            _currentFireRateTimer = 0;
            _currentMeleeAttackCouldown = 0;

            _currentWeapon = _startedMeleeWeapon;
        }

        private void Update()
        {
            if(!_canAttack)
            {
                _currentFireRateTimer += Time.deltaTime;
                if(_currentFireRateTimer >= _currentFireRate)
                {
                    _canAttack = true;
                    _currentFireRateTimer = 0;
                }
            }

            if(_meleeInCouldown)
            {
                _currentMeleeAttackCouldown += Time.deltaTime;
                if(_currentMeleeAttackCouldown >= _meleeAttackCouldown)
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

                    Debug.Log("Sem capacidade");
                }
            }
            else{ MeleeAttack(); }
        }

        #region Melee attack functions
        public void MeleeAttack()
        {
            if(!_behaviour.Animation.IsMeleeAnimationFinish || _meleeInCouldown) return;

            _behaviour.Animation.CallAttackTrigger();

            _currentFireRate = _startedMeleeWeapon.FireRate;

            _canAttack = false;
            _meleeInCouldown = true;
        }

        public void ApplyMeleeDamage()
        {
            Debug.Log("ApplyDamage");
        }
        #endregion

        public void ChangeCurrentWeapon(WeaponData weaponData)
        {
            if(weaponData == null) return;

            _currentWeapon = weaponData;

            if(weaponData.Type != WeaponType.MELEE)
            {
                _currentWeaponCapacity = weaponData.Capacity;

                CheckForShootType();
            }
        }

        #region All shooting type logics
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

                    _currentWeaponCapacity--;

                    await Task.Delay(BURST_FREQUENCY);
                }
            }
        }

        private void SingleShootType()
        {
            if(OnProjectileSpawn != null)
            {
                var projectile = OnProjectileSpawn(PROJECTILE_KEY, _shootPosition.position);

                projectile.GetComponent<MoveObjectHorizontal>().MoveRight = !_behaviour.Moviment.IsFlipped;

                _currentWeaponCapacity--;
            }

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

                    _currentWeaponCapacity -= SHOTGUN_BULLET_INSTANCES;
                }
            }

            _currentFireRate = _currentWeapon.FireRate;
            _canAttack = false;
        }
        #endregion
    }
}
