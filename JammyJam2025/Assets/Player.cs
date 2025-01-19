using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerController))]
public class Player : LivingEntity
{
    public Transform middlePoint;
    public float jumpHeight = 4;
    public float timeToJumpApex = .4f;
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;
    float moveSpeed = 2;

    float gravity;
    float jumpVelocity;
    [HideInInspector]
    public Vector3 velocity;

    public float targetVelocityX;
    float velocityXSmoothing;

    private bool facingRight;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;

    [HideInInspector]
    private bool isAttacking;

    PlayerController controller;

    public override void Start()
    {
        base.Start();
        controller = GetComponent<PlayerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        print("Gravity: " + gravity + "  Jump Velocity: " + jumpVelocity);
    }

    void Update()
    {
        if (controller.collisionInfo.above || controller.collisionInfo.below)
        {
            velocity.y = 0;
            animator.SetBool("isJumping", false);
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetKeyDown(KeyCode.Space) && controller.collisionInfo.below)
        {
            velocity.y = jumpVelocity;
            animator.SetBool("isJumping", true);
        }

        if ((velocity.x > 0 || velocity.x < 0) && !PlayerAttack.Instance.isAttacking)
        {
            animator.SetBool("isWalking", true);
        } else
        {
            animator.SetBool("isWalking", false);
        }

        targetVelocityX = input.x * moveSpeed;

        if (velocity.x < 0f)
        {
            spriteRenderer.flipX = true;
        } else if (velocity.x > 0f) {
            spriteRenderer.flipX = false;
        }

        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisionInfo.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.x = targetVelocityX;
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}