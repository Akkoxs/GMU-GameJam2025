using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{

    public float jumpHeight = 4;
    public float timeToJumpApex = .4f;
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;
    float moveSpeed = 2;

    float gravity;
    float jumpVelocity;
    Vector3 velocity;
    float velocityXSmoothing;

    private bool facingRight;
    [SerializeField] private Animator animator;

    PlayerController controller;

    void Start()
    {
        controller = GetComponent<PlayerController>();

        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        print("Gravity: " + gravity + "  Jump Velocity: " + jumpVelocity);
    }

    void Update()
    {
        if (controller.collisionInfo.above || controller.collisionInfo.below)
        {
            velocity.y = 0;
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetKeyDown(KeyCode.Space) && controller.collisionInfo.below)
        {
            velocity.y = jumpVelocity;
        }

        if ((velocity.x > 0 || velocity.x < 0) && !PlayerAttack.Instance.isAttacking)
        {
            animator.SetBool("isWalking", true);
        } else
        {
            animator.SetBool("isWalking", false);
        }

        float targetVelocityX = input.x * moveSpeed;
        //Animate(targetVelocityX);
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisionInfo.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.x = targetVelocityX;
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    //private void Animate(float moveX)
    //{
    //    if (moveX < 0 && !facingRight)
    //    {
    //        FlipCharacter();
    //    }
    //    else if (moveX > 0 && facingRight)
    //    {
    //        FlipCharacter();
    //    }
    //}

    //private void FlipCharacter()
    //{
    //    facingRight = !facingRight;
    //    transform.Rotate(0f, 180f, 0f);
    //}
}