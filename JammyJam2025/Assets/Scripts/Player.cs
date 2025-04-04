using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(PlayerController))]
public class Player : LivingEntity
{

    public Transform middlePoint;
    public float jumpHeight = 4;
    public float timeToJumpApex = .4f;
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;
    public float moveSpeed = 1;
    public float knockbackForce = 1.9f;
    public float invincibilityTime = 1.8f;

    float gravity;
    float jumpVelocity;
    [HideInInspector]
    public Vector3 velocity;

    public float targetVelocityX;
    float velocityXSmoothing;
    
    public GameOver GameOver;
    private bool facingRight;
    [HideInInspector]
    public SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    [SerializeField] public HealthBar player_healthBar; 
    [SerializeField] public Animator vfx;


    public bool isDying = false; //when youre taking damage lol
    private float dieFlash = 0.6f; 

    [HideInInspector]
    private bool isAttacking;
    private bool isBeingAttacked;
    private bool hasBeenAttacked;

    [HideInInspector]
    public Vector2 input;

    PlayerController controller;

    public override void Start()
    {
        health = (int)player_healthBar.healthSlider.value; //starting health is whatever the slider is set to
        controller = GetComponent<PlayerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        print("Gravity: " + gravity + "  Jump Velocity: " + jumpVelocity);
    }

    void Update()
    {
        PlayerHP();

        if (controller.collisionInfo.above || controller.collisionInfo.below)
        {
            velocity.y = 0;
            animator.SetBool("isJumping", false);
        }

        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

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


        if (!isBeingAttacked) {
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisionInfo.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
            velocity.x = targetVelocityX;
        } 

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void PlayerHP(){ // the exact same code as the PlayerHealth.cs file
        health = (int)player_healthBar.healthSlider.value; //the slider value is the health
     }

    public void TakeDamage(int damage, Vector2 direction){
        if (!hasBeenAttacked)
        {
            isBeingAttacked = true;
            base.TakeDamage(damage);
            velocity = direction * knockbackForce;
            animator.SetTrigger("isHurt");
            vfx.SetTrigger("playerHitVFX");
            isDying = true;
            player_healthBar.healthSlider.value = health;
            if (health <= 0)
            {
                GameOver.TriggerGameOver();
            }
            StartCoroutine(invicibility());
            StartCoroutine(ResetisDying());
        }
    }

    private IEnumerator ResetisDying(){
        yield return new WaitForSeconds(dieFlash);
        isDying = false;
        isBeingAttacked = false;
    }

    IEnumerator invicibility()
    {
        hasBeenAttacked = true;
        yield return new WaitForSecondsRealtime(invincibilityTime);
        hasBeenAttacked = false;
    }
}