using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour,Idamageable
{
    public float currHealth;
    public float maxHealth;
    public float initialSpeed;
    public float currSpeed;
    public float fadeDuration;

    private SpriteRenderer spriteRenderer;
    private Collider2D cl;
    private Rigidbody2D rb;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        cl = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        currHealth = maxHealth;
        currSpeed = initialSpeed + LevelManager.Instance.score / 1000f;
    }

    void Update()
    {
        transform.Translate(Vector2.left * currSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Idamageable idamageable = collision.gameObject.GetComponent<Idamageable>();
        if (idamageable != null)
        {
            idamageable.TakeDamage(1f,this.transform);
        }
    }

    public void TakeDamage(float damage, Transform damageSource)
    {
        currHealth -= damage;
        if(currHealth <= 0f)
        {
            Dead();
        }
    }

    public void Dead()
    {
        LevelManager.Instance.EnemyDeath();
        
        cl.enabled = false;
        rb.gravityScale = 0f;
        AudioManager.Instance.PlaySFX(AudioManager.Instance.enemyDeath);

        StartCoroutine(FadeOutAndDestroy());
    }

    private IEnumerator FadeOutAndDestroy()
    {
        float fadeSpeed = 1f / fadeDuration;
        Color color = spriteRenderer.color;

        while (color.a > 0)
        {
            color.a -= fadeSpeed * Time.deltaTime;
            spriteRenderer.color = color;
            yield return null;
        }

        Destroy(gameObject);
    }
}
