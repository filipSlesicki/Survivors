using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    Weapon weapon;
    public bool penetrating = false;
    float damage;
    public void Launch(float speed, float damage, Weapon owner, bool penetrating, float lifeTime)
    {
        this.weapon = owner;
        this.damage = damage;
        rb.velocity = transform.right *speed;
        this.penetrating = penetrating;
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Entity damagable;
        if(collision.TryGetComponent<Entity>(out damagable))
        {
            damagable.TakeDamage(new DamageData(damage,weapon));
            if(!penetrating)
            {
                Destroy(gameObject);
                return;
            }
        }
        else
        Destroy(gameObject); //Hit wall
    }

   
}
