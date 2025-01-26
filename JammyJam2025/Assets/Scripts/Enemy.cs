using System.Collections;
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

    private Rigidbody2D rb;
    private bool isGrounded = false;
    private long lastAttackTime = 0;
    public bool isBeingAttacked;

    public override void Start()
    {
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
        spriteRenderer.flipX = rb.linearVelocity.x < 0.01;

        Animator animator = spriteRenderer.GetComponent<Animator>();
        animator.SetFloat("Speed", horizontalSpeed);
        animator.SetBool("IsJumping", !isGrounded);
        if (!isBeingAttacked)
        {
            MoveTowardsTarget();
        } else if (isBeingAttacked)
        {
            StartCoroutine(Wait());
        }

        AttackIfTargetInRange();
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
        animator.SetBool("IsAttacking", true);
        Debug.Log("Attack time: " + currentTime);

        // This animation is fucked, idk whats going on tbh.
        Invoke(nameof(ExecuteAttack), 670 / 2 / 1000f);
        Invoke(nameof(ResetAttackAnimation), 670 / 1000f);
    }

    void ExecuteAttack() {
        long currentTime = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();
        Debug.Log("Execute time: " + currentTime);

        Vector3 target = gameManager.player.middlePoint.position;
        float distanceToTarget = Vector2.Distance(transform.position, target);
        if (distanceToTarget <= 1) {
            gameManager.player.TakeDamage(damage);
            return;
        }

        target = gameManager.shroomaloom.middlePoint.position;
        distanceToTarget = Mathf.Abs(target.x - middlePoint.position.x);
        if (distanceToTarget <= 1.5) {
            gameManager.shroomaloom.TakeDamage(damage);
        }

        // TODO: The above is kinda unfair, LOS check?
        // Vector2 direction = (entity.middlePoint.position - middlePoint.transform.position).normalized;
        // RaycastHit2D obstacleHit = Physics2D.Raycast(middlePoint.position, direction, 1.0f, obstaclesLayer);
    }

    void ResetAttackAnimation() {
        long currentTime = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();
        Debug.Log("Reset time: " + currentTime);

        Animator animator = spriteRenderer.GetComponent<Animator>();
        animator.SetBool("IsAttacking", false);
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

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        if (health <= 0)
        {
            Destroy(gameObject);
        }

        if (gameObject.IsDestroyed()) {
            gameManager.OnEnemyKilled();
        }
    }

    IEnumerator Wait()
    {
        rb.linearVelocity = new Vector2(0, 0);
        rb.AddForce(new Vector2(-gameManager.player.targetVelocityX * 1.5f, 0), ForceMode2D.Impulse);
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        isBeingAttacked = false;
        spriteRenderer.color = Color.white;
    }
}