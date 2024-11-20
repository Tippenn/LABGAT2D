using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifetime = 5f;
    public int damage = 10;

    void Start()
    {
        // Destroy the projectile after a certain time
        Destroy(gameObject, lifetime);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            Idamageable idamageable = collision.gameObject.GetComponent<Idamageable>();
            if (idamageable != null)
            {
                idamageable.TakeDamage(1f, this.transform);
            }
            
        }
        Destroy(gameObject);

    }
}
