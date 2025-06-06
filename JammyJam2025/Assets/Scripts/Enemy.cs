using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : LivingEntity
{   
    public GameManager gameManager;
    public float speed = 0.5f;
    public float jumpForce = 7.0f;
    public float lineOfSightRange = 10.0f;
    public LayerMask obstaclesLayer;
    public Transform groundPoint;
    public float groundPointRadius = 0.2f;
    public LayerMask groundLayer;

    public SpriteRenderer spriteRenderer;

    public Transform middlePoint;
    public int damage = 25; //changed from 50
    public int shroomDamage = 5;

    private Rigidbody2D rb;
    private bool isGrounded = false;
    private long lastAttackTime = 0;
    public Animator animator;
    [HideInInspector]
    public bool isBeingAttacked;
    [HideInInspector]
    public bool isFinalAttack;
    [HideInInspector]
    public bool isHeavyAttack;
    private bool canMove;
    private bool canAttack = true;
    public Animator vfxAnimator;

    public override void Start()
    {
        canMove = true;
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        gameManager = FindFirstObjectByType<GameManager>();
    }

    void Update()
    {
        if (gameManager.player.gameObject.transform == null) return;

        // TODO Is using collisions better?
        // isGrounded = Physics2D.OverlapCircle(groundPoint.position, groundPointRadius, groundLayer);

        float horizontalSpeed = Mathf.Abs(rb.linearVelocity.x);
        // Flip sprite if walking left.
        spriteRenderer.flipX = rb.linearVelocity.x < 0;

        Animator animator = spriteRenderer.GetComponent<Animator>();
        animator.SetFloat("Speed", horizontalSpeed);
        animator.SetBool("IsJumping", !isGrounded);
        if (!isBeingAttacked && canMove)
        {
            MoveTowardsTarget();
        }

        if (!isBeingAttacked && canAttack)
        {
            AttackIfTargetInRange();
        }
    }

    void AttackIfTargetInRange() {
        Vector3 target = gameManager.player.middlePoint.position;
        float distanceToTarget = Vector2.Distance(middlePoint.position, target);
        if (distanceToTarget <= 1.5) {
            StartAttack();
            //gameManager.player.isDying = true;
            return;
        }

        target = gameManager.shroomaloom.middlePoint.position;
        distanceToTarget = Vector2.Distance(middlePoint.position, target);
        if (distanceToTarget <= 1.5) {
            StartAttack();
        }
    }

    void StartAttack() {
        long currentTime = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();
        if (currentTime - lastAttackTime < 2000) return;

        lastAttackTime = currentTime;
        Animator animator = spriteRenderer.GetComponent<Animator>();
        //animator.SetBool("IsAttacking", true);
        //Debug.Log("Attack time: " + currentTime);

        // This animation is fucked, idk whats going on tbh.
        Invoke(nameof(ExecuteAttack), 670 / 2 / 1000f);
        //Invoke(nameof(ResetAttackAnimation), 670 / 1000f);
    }

    void ExecuteAttack() {
        long currentTime = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();
        //Debug.Log("Execute time: " + currentTime);

        Vector3 target = gameManager.player.middlePoint.position;
        float distanceToTarget = Vector2.Distance(transform.position, target);
        if (distanceToTarget <= 1) {
            animator.SetTrigger("IsAttackingT");
            //return;
        }

        target = gameManager.shroomaloom.middlePoint.position;
        distanceToTarget = Mathf.Abs(target.x - middlePoint.position.x);
        if (distanceToTarget <= 1.5) {
            animator.SetTrigger("IsAttackingT");
            //Invoke(nameof(ResetAttackAnimation), 670 / 1000f);
        } 

        // TODO: The above is kinda unfair, LOS check?
        // Vector2 direction = (entity.middlePoint.position - middlePoint.transform.position).normalized;
        // RaycastHit2D obstacleHit = Physics2D.Raycast(middlePoint.position, direction, 1.0f, obstaclesLayer);
    }

    void ResetAttackAnimation() {
        long currentTime = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();
        //Debug.Log("Reset time: " + currentTime);

        Animator animator = spriteRenderer.GetComponent<Animator>();
        animator.SetBool("IsAttacking", false);
        StartCoroutine(attackTimer());
    }

    private bool HasLineOfSight()
    {
        Vector3 thisMiddle = middlePoint.position;
        float distanceToPlayer = Vector2.Distance(thisMiddle, gameManager.player.middlePoint.position);
        if (distanceToPlayer > lineOfSightRange) return false;

        Vector2 directionToPlayer = (gameManager.player.middlePoint.position - thisMiddle).normalized;
        RaycastHit2D hit = Physics2D.Raycast(thisMiddle, directionToPlayer, lineOfSightRange, obstaclesLayer);

        return hit.collider == null || hit.collider.CompareTag("Player");
    }

    private void MoveTowardsTarget()
    {
        Vector3 target = HasLineOfSight() ? gameManager.player.middlePoint.position : gameManager.shroomaloom.middlePoint.position;
        Vector2 direction = (target - transform.position).normalized;

        RaycastHit2D obstacleHit = Physics2D.Raycast(middlePoint.position, Vector2.right * Mathf.Sign(direction.x), 1.0f, obstaclesLayer);
        // Debug.DrawRay(middlePoint.position, Vector2.right * Mathf.Sign(direction.x), Color.red);

        // Move towards player.
        rb.linearVelocity = new Vector2(direction.x * 0.5f, rb.linearVelocity.y);
        //Debug.Log("enemy velocity: " + rb.linearVelocity);
        

        if (obstacleHit.collider != null && isGrounded) {
            Jump();
        }
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(middlePoint.position, lineOfSightRange);

        if (groundPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundPoint.position, groundPointRadius);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Vector2 normal = collision.GetContact(0).normal;
            if (normal == Vector2.up)
            {
                isGrounded = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Vector2 attack = new();
        if (collision.gameObject.CompareTag("Player"))
        {
            if (spriteRenderer.flipX)
            {
                attack = new Vector2(-1, 0);
            } else if (!spriteRenderer.flipX)
            {
                attack = new Vector2(1, 0);
            }

            gameManager.player.TakeDamage(damage, attack);
            HitStop.Instance.Stop(0.15f);
            canAttack = false;
            StartCoroutine(attackTimer());
        }

        if (collision.gameObject.CompareTag("Shroomaloom"))
        {
            Debug.Log("FoundShroom!");
            gameManager.shroomaloom.TakeDamage(shroomDamage);
            canAttack = false;
            StartCoroutine(attackTimer());
        }
    }
     
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        Wait();

        if (health <= 0)
        {
            Destroy(gameObject);
        }

        if (gameObject.IsDestroyed()) {
            gameManager.OnEnemyKilled();
        }
    }

    private void Wait()
    {
        rb.linearVelocity = new Vector2(0, 0);
        animator.SetTrigger("isAttacked");
        canMove = false;

        Vector2 attack = new();
        if (gameManager.player.spriteRenderer.flipX)
        {
            attack = new Vector2(-1, 0);
        }
        else if (!gameManager.player.spriteRenderer.flipX)
        {
            attack = new Vector2(1, 0);
        }

        vfxAnimator.SetTrigger("hit");

        if (isFinalAttack)
        {
            rb.AddForce(attack * 4f, ForceMode2D.Impulse);
            isFinalAttack = false;
        } else if (isHeavyAttack)
        {
            rb.AddForce(attack * 5f, ForceMode2D.Impulse);
            isHeavyAttack = false;
        } else
        {
            rb.AddForce(attack * 2f, ForceMode2D.Impulse);
        }

        StartCoroutine(WaitToMove());
    }

    IEnumerator WaitToMove()
    {
        yield return new WaitForSeconds(0.05f);
        canMove = true;
        isBeingAttacked = false;
    }

    IEnumerator attackTimer()
    {
        yield return new WaitForSecondsRealtime(2f);
        canAttack = true;
    }
}