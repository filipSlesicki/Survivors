using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
public class Projectile : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    Weapon weapon;
    public bool penetrating = false;
    float damage;
    public ObjectPool<Projectile> bulletPool;
    float destroyTime;

    public void Launch(float speed, float damage, Weapon owner, bool penetrating, float lifeTime)
    {
        this.weapon = owner;
        this.damage = damage;
        rb.velocity = transform.right * speed;
        this.penetrating = penetrating;
        destroyTime = Time.time + lifeTime;
    }

    private void Update()
    {
        if(Time.time > destroyTime)
        {
            DestroyObject();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Entity damagable;
        if(collision.TryGetComponent<Entity>(out damagable))
        {
            damagable.TakeDamage(new DamageData(damage,weapon));
            if(!penetrating)
            {
                bulletPool.Release(this);
                return;
            }
        }
        else
            bulletPool.Release(this);  //Hit wall
    }

   void DestroyObject()
    {
        bulletPool.Release(this);
    }
}
