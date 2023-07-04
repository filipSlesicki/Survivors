using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
public class Projectile : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    Weapon weapon;
    int penetration = 0;
    float damage;
    public ObjectPool<Projectile> bulletPool;
    float destroyTime;

    public void Launch(float speed, float damage, Weapon owner, int penetration, float lifeTime)
    {
        this.weapon = owner;
        this.damage = damage;
        rb.velocity = transform.right * speed;
        this.penetration = penetration;
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
            damagable.TakeDamage(new DamageInfo(damage,weapon));
            penetration--;
            if(penetration <=0)
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
