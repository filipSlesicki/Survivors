using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField]
    protected Transform[] shootPoints;
    //Current Stats
    [SerializeField]
    protected bool aimAtClosest = true;
    [SerializeField]
    protected float range = 1;

    protected float cooldown;
    protected float damage;
    protected bool penetrating;
    protected float sizeModifier = 1;
    protected int bulletCount =1;
    public int level { get; private set; }

    public int displayLevel { get { return level + 1; } }
    public Character owner { get; private set; }
    public WeaponData data { get; private set; }

    public float damageDealt { get; private set; }
    public int aquireTime { get; private set; }

    protected float nextShootTime;

    private void OnDisable()
    {
        Enemy.OnEnemyDamaged -= UpdateDamageDealt;
    }

    void UpdateDamageDealt(DamageData dd)
    {
        if(dd.weapon == this)
        {
            damageDealt += dd.damage;
        }
    }

    public virtual void Setup(WeaponData data, Character owner)
    {
        this.owner = owner;
        aquireTime = GameTime.totalTimeInSeconds;
        this.data = data;
        damage = data.BaseStats.Damage;
        owner.bonuses.ApplyAllBonuses(PlayerStats.Damage, ref damage);
        cooldown = data.BaseStats.CoolDown;
        owner.bonuses.ApplyAllBonuses(PlayerStats.AttackSpeed, ref cooldown);
        penetrating = data.BaseStats.Penetrating;
        bulletCount = data.BaseStats.BulletCount;

        Enemy.OnEnemyDamaged += UpdateDamageDealt;

    }
    public void Tick()
    {
        if(aimAtClosest)
        {
            Enemy target = EnemyManager.GetClosestEnemyFromPoint(owner.movement.lastPosition);

            if (target)
            {
                AimWeapon(target.movement.lastPosition);
            }
        }
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
        WeaponStats bonus = data.BonusPerLevel[level];
        damage += bonus.Damage;
        sizeModifier += bonus.Size;
        bulletCount += bonus.BulletCount;
        cooldown *= (1- bonus.CoolDown*0.01f);
        if(bonus.Penetrating)
        {
            penetrating = true;
        }
        level++;
    }
}
