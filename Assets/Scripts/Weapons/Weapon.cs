using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField]
    protected Transform[] shootPoints;
    //Current Stats
    [SerializeField]
    public bool aimAtClosest = true;

    protected Dictionary<WeaponStatType, float> stats = new Dictionary<WeaponStatType, float>();
    protected float damage { get { return stats[WeaponStatType.Damage]; } set { stats[WeaponStatType.Damage] = value; } }
    protected float cooldown { get { return stats[WeaponStatType.CoolDown]; } set { stats[WeaponStatType.CoolDown] = value; } }
    protected float bulletCount { get { return stats[WeaponStatType.BulletCount]; } set { stats[WeaponStatType.BulletCount] = value; } }
    protected float duration { get { return stats[WeaponStatType.Duration]; } set { stats[WeaponStatType.Duration] = value; } }
    protected float size { get { return stats[WeaponStatType.Size]; } set { stats[WeaponStatType.Size] = value; } }
    protected int penetration { get { return (int)stats[WeaponStatType.Penetration]; } set { stats[WeaponStatType.Penetration] = value; } }


    public int level { get; private set; }

    public int displayLevel { get { return level + 1; } }
    public Character owner { get; private set; }
    public WeaponData data { get; private set; }

    public float damageDealt { get; private set; }
    public int aquireTime { get; private set; }

    protected float nextShootTime;

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
    }

    protected virtual void SetStats(WeaponData data)
    {
        stats[WeaponStatType.Damage] = data.Stats.Damage;
        stats[WeaponStatType.CoolDown] = data.Stats.CoolDown;
        stats[WeaponStatType.Penetration] = data.Stats.Penetration;
        stats[WeaponStatType.BulletCount] = data.Stats.BulletCount;
        stats[WeaponStatType.Duration] = data.Stats.Duration;
        stats[WeaponStatType.Size] = data.Stats.Size;
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
        if(CanShoot())
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

        nextShootTime = Time.time + cooldown;
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
    }
}
