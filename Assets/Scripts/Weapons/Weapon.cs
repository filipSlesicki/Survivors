using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Weapon : MonoBehaviour
{
    [HideInInspector]
    public bool autoPlaceShootPositions;
    public float autoDistanceFromWeapon = 1;
    [HideInInspector]
    public Transform[] shootPositions;

    //Current Stats
    public bool aimAtClosest = true;
    public UnityEvent<Weapon> onFinishSetup;
    public UnityEvent onStartShooting;
    public UnityEvent onStopShooting;
    public UnityEvent<int, Vector3> onUpdateHitPoint;

    protected Dictionary<WeaponStat, float> stats = new Dictionary<WeaponStat, float>();
    public float damage { get { return stats[WeaponStat.Damage]; } set { stats[WeaponStat.Damage] = value; } }
    public float cooldown { get { return stats[WeaponStat.CoolDown]; } set { stats[WeaponStat.CoolDown] = value; } }
    public int bulletCount { get { return (int)stats[WeaponStat.BulletCount]; } set { stats[WeaponStat.BulletCount] = value; } }
    public float duration { get { return stats[WeaponStat.Duration]; } set { stats[WeaponStat.Duration] = value; } }
    public float range { get { return stats[WeaponStat.Range]; } set { stats[WeaponStat.Range] = value; } }
    public float size { get { return stats[WeaponStat.Size]; } set { stats[WeaponStat.Size] = value; } }
    public int penetration { get { return (int)stats[WeaponStat.Penetration]; } set { stats[WeaponStat.Penetration] = value; } }
    public float projectileSpeed { get { return stats[WeaponStat.BulletSpeed]; } set { stats[WeaponStat.BulletSpeed] = value; } }

    public int level { get; private set; }

    public int displayLevel { get { return level + 1; } }
    public Character owner { get; private set; }
    public WeaponData data { get; private set; }

    public float damageDealt { get; private set; }
    public int aquireTime { get; private set; }

    protected float nextShootTime;

    public int GetProjectileLayer()
    {
        if (owner is Player)
        {
            return LayerMask.NameToLayer("PlayerBullet");
        }
        else if (owner is Enemy)
        {
            return LayerMask.NameToLayer("EnemyBullet");
        }
        return 0;
    }

    public int GetTargetLayer()
    {
        if (owner is Player)
        {
           return LayerMask.GetMask("Enemy");
        }
        else if (owner is Enemy)
        {
            return LayerMask.GetMask("Player");
        }
        return 0;
    }

   

    private void OnDisable()
    {
        //Enemy.OnEnemyDamaged -= UpdateDamageDealt;
    }

    void UpdateDamageDealt(DamageInfo dd)
    {
        if(dd.weapon == this)
        {
            damageDealt += dd.damage;
        }
    }

    public void Setup(WeaponData data, Character owner)
    {
        this.data = data;
        SetStats(data);
        SetOwner(owner);
        UpdateShootPositions();
        onFinishSetup?.Invoke(this);
    }

    protected virtual void SetStats(WeaponData data)
    {
        stats[WeaponStat.Damage] = data.stats.Damage;
        stats[WeaponStat.CoolDown] = data.stats.CoolDown;
        stats[WeaponStat.Penetration] = data.stats.Penetration;
        stats[WeaponStat.BulletCount] = data.stats.BulletCount;
        stats[WeaponStat.Duration] = data.stats.Duration;
        stats[WeaponStat.Range] = data.stats.Range;
        stats[WeaponStat.Size] = data.stats.Size;
        stats[WeaponStat.BulletSpeed] = data.stats.BulletSpeed;
    }

    protected virtual void SetOwner(Character owner)
    {
        this.owner = owner;
        //owner.bonuses.ApplyAllBonuses(CharacterStats.Damage, ref damage);
        //owner.bonuses.ApplyAllBonuses(CharacterStats.AttackSpeed, ref cooldown);
        if(owner is Player)
        {
            aquireTime = GameTime.totalTimeInSeconds;
            //Enemy.OnEnemyDamaged += UpdateDamageDealt;
        }
    }

    public void Tick()
    {

        if (CanShoot())
        {
            Shoot();
        }
        AdditionalUpdate();

    }

    public void AimWeapon(Vector3 targetPos)
    {
        Vector3 toTarget = targetPos - transform.position;
        transform.right = toTarget;
    }

    protected virtual void AdditionalUpdate()
    {
    }
    public bool CanShoot()
    {
        if (Time.time < nextShootTime)
            return false;

        return true;
    }

    public virtual void Shoot()
    {
        if (!CanShoot())
            return;

        if(owner is Player)
        {
            if(aimAtClosest)
                AimWeapon(PlayerAttack.closestEnemyPos);
        }
        else
        {
            if (aimAtClosest)
                AimWeapon(Player.Instance.movement.lastPosition);
        }
        nextShootTime = Time.time + cooldown;
        onStartShooting?.Invoke();
        data.fireMode.Shoot(this);
    }

    public void DoWeaponEffect()
    {
        data.shootType.Shoot(this);
    }

    public virtual void AddLevel()
    {
        WeaponBonusPerLevel allBonuses = data.BonusPerLevel[level];
        foreach (var bonus in allBonuses.bonuses)
        {
            float currentValue = stats[bonus.stat];
            Bonuses.ApplyBonusToValue(ref currentValue, bonus.increaseType, bonus.increaseValue);
            stats[bonus.stat] = currentValue;
        }
        level++;
        UpdateShootPositions();
    }

    void UpdateShootPositions()
    {
        //if(shootPositions.Length != bulletCount)
        //{
        //    shootPositions = new PosRotPair[bulletCount];
        //    if (!autoPlaceShootPositions)
        //    {
        //        for (int i = 0; i < bulletCount; i++)
        //        {
        //            shootPositions[i] = (new PosRotPair(customShootPoints[i]));
        //        }
        //    }
        //}

        if (!autoPlaceShootPositions || shootPositions.Length == bulletCount)
        {
            return;
        }

        foreach (var shootPos in shootPositions)
        {
            Destroy(shootPos.gameObject);
        }
        shootPositions = new Transform[bulletCount];
            

        float weaponAngle = transform.rotation.eulerAngles.z;
        float anglePerBullet = 360 / bulletCount;
        for (int i = 0; i < bulletCount; i++)
        {
            float currentAngle = anglePerBullet * i;
            Debug.Log(currentAngle);
            Vector3 position = transform.position +
                new Vector3(Mathf.Cos(Mathf.Deg2Rad * currentAngle) * autoDistanceFromWeapon,
                Mathf.Sin(Mathf.Deg2Rad * currentAngle) * autoDistanceFromWeapon,
                0);

            Quaternion outwardDir = Quaternion.Euler(0, 0, weaponAngle + currentAngle);
            GameObject newShootPos = new GameObject();
            newShootPos.transform.SetPositionAndRotation(position, outwardDir);
            newShootPos.transform.SetParent(transform);

            shootPositions[i] = newShootPos.transform;
            
        }
    }
}
