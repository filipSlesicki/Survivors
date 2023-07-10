using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour
{
    protected Weapon weapon;
    protected int penetration = 0;
    protected ObjectPool<Projectile> bulletPool;
    protected float destroyTime;
    public Rigidbody2D rb;
    [SerializeField] SpriteRenderer renderer;

    public virtual void Launch(Weapon weapon, ObjectPool<Projectile> bulletPool, Color color)
    {
        penetration = weapon.penetration;
        transform.localScale = Vector3.one * weapon.size;
        this.weapon = weapon;
        gameObject.layer = weapon.GetProjectileLayer();
        destroyTime = Time.time + weapon.duration;
        this.bulletPool = bulletPool;
        renderer.color = color;
    }

    private void Update()
    {
        if (Time.time > destroyTime)
        {
            DestroyObject();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<Entity>())
        {
            foreach (var effect in weapon.data.hitEffects)
            {
                effect.OnHit(other, weapon);
            }
            penetration--;
            if (penetration <= 0)
            {
                DestroyObject();
                return;
            }
        }
        else
            DestroyObject();  //Hit wall
    }

    void DestroyObject()
    {
        bulletPool.Release(this);
    }
}
