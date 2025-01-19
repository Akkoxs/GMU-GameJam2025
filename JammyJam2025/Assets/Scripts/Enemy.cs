using Unity.VisualScripting;
using UnityEngine;

public class Enemy : LivingEntity
{   
    public GameManager gameManager;
    public float speed = 3.0f;
    public float jumpForce = 7.0f;
    public float lineOfSightRange = 10.0f;
    public LayerMask obstaclesLayer;
    public Transform groundPoint;
    public float groundPointRadius = 0.2f;
    public LayerMask groundLayer;

    public SpriteRenderer spriteRenderer;

    public Transform middlePoint;

    private Rigidbody2D rb;
    private bool isGrounded = false;
    private long lastAttackTime = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (gameManager.player.transform == null) return;

        // TODO Is using collisions better?
        // isGrounded = Physics2D.OverlapCircle(groundPoint.position, groundPointRadius, groundLayer);

        float horizontalSpeed = Mathf.Abs(rb.linearVelocity.x);
        // Flip sprite if walking left.
        spriteRenderer.flipX = rb.linearVelocity.x < 0;

        Animator animator = spriteRenderer.GetComponent<Animator>();
        animator.SetFloat("Speed", horizontalSpeed);
        animator.SetBool("IsAttacking", false);
        animator.SetBool("IsJumping", !isGrounded);
        MoveTowardsTarget();

        AttackIfTargetInRange();
    }

    void AttackIfTargetInRange() {
        // Vector3 target = gameManager.playerData.middlePoint.position;
        // float distanceToTarget = Vector2.Distance(transform.position, target);

        // if (distanceToTarget <= 2.0f)
        // {
        //     Attack(target);
        //     return;
        // }

        // Vector3 target = gameManager.shroomaloomData.middlePoint.position;
        // float distanceToTarget = Vector2.Distance(transform.position, target);

        // if (distanceToTarget <= 2.0f)
        // {
        //     Attack(target);
        // }
    }

    void Attack(Vector3 target) {
        Animator animator = spriteRenderer.GetComponent<Animator>();
        animator.SetBool("IsAttacking", true);
    }

    void ExecuteAttack(Vector3 target) {
        // Check that target is in LOS.
        Vector2 direction = (target - middlePoint.transform.position).normalized;
        RaycastHit2D obstacleHit = Physics2D.Raycast(middlePoint.position, direction, 1.0f, obstaclesLayer);

        if (obstacleHit.collider != null && isGrounded) {
            Jump();
        }
    }

    private bool HasLineOfSight()
    {
        Vector3 middle = middlePoint.position;
        float distanceToPlayer = Vector2.Distance(middle, gameManager.player.transform.position);
        Debug.Log("distanceToPlayer: " + distanceToPlayer);
        if (distanceToPlayer > lineOfSightRange) return false;

        Vector2 directionToPlayer = (gameManager.player.transform.position - middle).normalized;
        RaycastHit2D hit = Physics2D.Raycast(middle, directionToPlayer, lineOfSightRange, obstaclesLayer);

        return hit.collider == null || hit.collider.CompareTag("Player");
    }

    private void MoveTowardsTarget()
    {
        Vector3 target = HasLineOfSight() ? gameManager.player.transform.position : gameManager.shroomaloom.transform.position;
        Vector2 direction = (target - transform.position).normalized;

        RaycastHit2D obstacleHit = Physics2D.Raycast(middlePoint.position, Vector2.right * Mathf.Sign(direction.x), 1.0f, obstaclesLayer);
        // Debug.DrawRay(middlePoint.position, Vector2.right * Mathf.Sign(direction.x), Color.red);

        // Move towards player.
        rb.linearVelocity = new Vector2(direction.x * speed, rb.linearVelocity.y);

        if (obstacleHit.collider != null && isGrounded) {
            Jump();
        }
    }

    private void Jump()
    {
        // rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        Debug.Log("Jumping");
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lineOfSightRange);

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

        if (gameObject.IsDestroyed()) {
            gameManager.OnEnemyKilled();
        }
    }
}