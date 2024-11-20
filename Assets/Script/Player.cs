using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour, Idamageable
{
    [Header("Movement")]
    public float movementSpeed;
    public float jumpForce;
    public float speedShow;
    public float groundCheckDistance = 0.3f;
    private bool isFacingRight = true;
    public bool isWalking = false;
    public bool isFalling = false;
    public bool isGrounded = false;
    public bool isJumping = false;
    public LayerMask groundLayer;

    [Header("Stats")]
    public float currentHealth;
    [HideInInspector]
    public float maxHealth = 5;

    [Header("KnockBack")]
    public bool isKnocked = false;
    public float knockbackForce;
    public float knockbackDuration = 0.5f;
    private float knockbackTimer;

    public bool isInvis = false;
    private bool isBlinking;
    public float invisDuration = 2f;
    private float invisTimer;
    public float blinkRate = 0.5f;

    [Header("Mellee Attack")]
    public GameObject attackVFX;
    public Transform attackPointUsed;
    public Transform attackPointLeft;
    public Transform attackPointRight;
    public Animator attackVFXAnimation;
    public float slashCooldown;
    public bool canSlash = true;
    public float meleeAttackRadius;
    public LayerMask enemyLayer;

    [Header("Range Attack")]
    public GameObject bulletPrefab;
    public float bulletSpeed;
    public float shotCooldown;
    public bool canShot = true;
    public int currAmmo;
    public int maxAmmo;
    public bool isReloading = false;
    public float reloadCooldown = 0.5f;
    public float reloadTimer;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Animator animator;
    

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        attackVFXAnimation = attackVFX.GetComponent<Animator>();
    }
    void Start()
    {
        currentHealth = maxHealth;
        currAmmo = maxAmmo;
        UpdateAttackPoint();
    }

    void Update()
    {
        speedShow = rb.velocity.magnitude;

        //check for invis
        if(isInvis)
        {
            if(!isBlinking)
            {
                isBlinking = true;
                StartCoroutine(CharFlashing());
            }
            invisTimer -= Time.deltaTime;
            if(invisTimer < 0)
            {
                isInvis = false;
            }
        }

        //check for animation
        if(isFalling)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", true);
        }
        else if(isJumping)
        {
            animator.SetBool("isJumping", true);
            animator.SetBool("isFalling", false);
        }
        else if(isWalking)
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("isFalling", false);
            animator.SetBool("isJumping", false);
        }
        else
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isFalling", false);
            animator.SetBool("isJumping", false);
        }


        //Input
        if (!isKnocked)
        {
            //movement
            float inputX = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(inputX * movementSpeed, rb.velocity.y);

            //facing
            if(inputX > 0)
            {
                isFacingRight = true;
                CheckForFacing();
                UpdateAttackPoint();
            }
            else if(inputX < 0)
            {
                isFacingRight = false;
                CheckForFacing();
                UpdateAttackPoint();
            }

            //x axis
            if(rb.velocity.x > 0)
            {
                isWalking = true;
            }
            else if(rb.velocity.x == 0)
            {
                isWalking = false;
            }
            else
            {
                isWalking = true;
            }

            //y axis
            if(rb.velocity.y > 0)
            {
                isJumping = true;
                isFalling = false;
            }
            else if(rb.velocity.y == 0)
            {
                isJumping = false;
                isFalling = false;
            }
            else
            {
                isJumping = false;
                isFalling = true;
            }

            //ground check
            GroundCheck();

            //jump
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                isGrounded = false;
                Jump();
            }

            //reload
            if (Input.GetKeyDown(KeyCode.L))
            {
                isReloading = true;
            }

            if (Input.GetKeyUp(KeyCode.L))
            {
                isReloading = false;
                reloadTimer = 0;
            }

            if(isReloading)
            {
                Reload();
            }

            //range attack
            if (Input.GetKey(KeyCode.J) && canShot  && currAmmo > 0 && isReloading == false)
            {
                canShot = false;
                RangeAttack();
                Debug.Log("range");

                Invoke(nameof(ResetRangeAttack), shotCooldown);
            }

            //mellee attack
            if(Input.GetKey(KeyCode.K) && canSlash && isReloading == false)
            {
                canSlash = false;
                MelleeAttack();
                Debug.Log("mellee");

                Invoke(nameof(ResetMelleeAttack), slashCooldown);
            }
        }
        else
        {
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0)
            {
                isKnocked = false;
            }
        }
    }

    void Jump()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.jump);
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    public void GroundCheck()
    {
        
        float raycastDistance = (GetComponent<Collider2D>().bounds.size.y / 2f) + groundCheckDistance;
        // Check if grounded using raycast
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, (GetComponent<Collider2D>().bounds.size.y / 2f) + groundCheckDistance, groundLayer);
    }

    void CheckForFacing()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        SpriteRenderer srVFX = attackVFX.GetComponent<SpriteRenderer>();
        if (isFacingRight)
        {       
            sr.flipX = false;
            srVFX.flipX = false;
        }
        else
        {
            sr.flipX = true;
            srVFX.flipX = true;
        }
    }

    #region Idamageable
    public void TakeDamage(float damage,Transform enemy)
    {
        if (isInvis)
            return;

        // -1 poin health
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            //mati
            Dead();
        }
        //sound
        AudioManager.Instance.PlaySFX(AudioManager.Instance.damage);

        //particle hit

        // knockback + invis 1s-2s
        isKnocked = true;
        knockbackTimer = knockbackDuration;

        Vector2 knockbackDirection = (transform.position - enemy.position).normalized;
        rb.velocity = knockbackDirection * knockbackForce;

        isInvis = true;
        invisTimer = invisDuration;
    }

    public void Dead()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.playerDeath);
        LevelManager.Instance.onDead?.Invoke();
    }
    #endregion

    public IEnumerator CharFlashing()
    {
        while(isInvis)
        {
            spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f, 255f);           
            yield return new WaitForSeconds(blinkRate/2);
            spriteRenderer.color = new Color(255f, 255f, 255f, 255f);
            yield return new WaitForSeconds(blinkRate/2);
            
        }

        isBlinking = false;
        yield return null;
    }

    #region Attack
    public void UpdateAttackPoint()
    {
        if (isFacingRight)
        {
            attackPointUsed = attackPointRight;
        }
        else
        {
            attackPointUsed = attackPointLeft;           
        }
        attackVFX.transform.position = attackPointUsed.position;
    }

    public void MelleeAttack()
    {
        attackVFXAnimation.SetTrigger("attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPointUsed.position, meleeAttackRadius, enemyLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Hit " + enemy.name);
            // Damage the enemy
            Idamageable idamageable = enemy.gameObject.GetComponent<Idamageable>();
            idamageable.TakeDamage(1f, this.transform);
        }
    }

    public void RangeAttack()
    {
        currAmmo--;

        AudioManager.Instance.PlaySFX(AudioManager.Instance.shoot);

        GameObject projectile = Instantiate(bulletPrefab, attackPointUsed.position, Quaternion.identity);

        // Set the projectile's velocity based on facing direction
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(isFacingRight ? bulletSpeed : -bulletSpeed, 0);
    }

    public void ResetRangeAttack()
    {
        canShot = true;
    }

    public void ResetMelleeAttack()
    {
        canSlash = true;
    }

    public void Reload()
    {
        reloadTimer += Time.deltaTime;
        if (reloadTimer > reloadCooldown && currAmmo < maxAmmo)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.reload);
            currAmmo++;
            reloadTimer = 0f;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPointUsed == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPointUsed.position, meleeAttackRadius);
    }
    #endregion
}
