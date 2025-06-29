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
    public float moveSpeed = 1;
    public float knockbackForce = 1.9f;
    public float invincibilityTime = 1.8f;

    public float gravity;
    float jumpVelocity;
    [HideInInspector]
    public Vector3 velocity;

    public float targetVelocityX;
    float velocityXSmoothing;
    
    [HideInInspector]
    public SpriteRenderer spriteRenderer;
    [SerializeField] public Animator animator;
    [SerializeField] public HealthBar player_healthBar; 
    [SerializeField] public Animator vfx;


    public bool isDying = false; //when youre taking damage lol
    private float dieFlash = 0.6f; 

    [HideInInspector]
    private bool isBeingAttacked;
    private bool hasBeenAttacked;

    [HideInInspector]
    public Vector2 input;

    public PlayerController controller;
    public IntroSequence intro;
    GameOver gameOver;

    public override void Start()
    {
        health = (int)player_healthBar.healthSlider.value; //starting health is whatever the slider is set to
        controller = GetComponent<PlayerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameOver = GetComponent<GameOver>();
        intro = GetComponent<IntroSequence>();
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        print("Gravity: " + gravity + "  Jump Velocity: " + jumpVelocity);
    }

    void Update()
    {
        PlayerHP();
        
        if(intro.enableIntro) return; {

            if(controller.collisionInfo.below){
                animator.SetBool("isJumping", false);
                animator.SetBool("isFalling", false);
            }

            if ((controller.collisionInfo.above || controller.collisionInfo.below))
            {
                velocity.y = 0;
                animator.SetBool("isJumping", false);
            }
        
            input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            if (Input.GetKeyDown(KeyCode.Space) && controller.collisionInfo.below )
            {
                velocity.y = jumpVelocity;
                animator.SetBool("isJumping", true);
                animator.SetBool("isFalling", false);
            }
    
            if ((velocity.x > 0 || velocity.x < 0) && !PlayerAttack.Instance.isAttacking && controller.collisionInfo.below)
            {
                animator.SetBool("isWalking", true);
            } else
            {
                animator.SetBool("isWalking", false);
            }

            if ((velocity.y <= 0) && !PlayerAttack.Instance.isAttacking && !controller.collisionInfo.below){
                animator.SetBool("isFalling", true);
                animator.SetBool("isJumping", false);
            }
            
            Debug.Log(controller.collisionInfo.below);
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
            vfx.SetTrigger("playerHitVFX");
            isDying = true;
            player_healthBar.healthSlider.value = health;
            if (health <= 0){
                gameOver.TriggerGameOver();
            }
            else{ //these corouts fuck with triggering a game over corout. leave em here 
                animator.SetTrigger("isHurt");
                StartCoroutine(invicibility());
                StartCoroutine(ResetisDying());
            }
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