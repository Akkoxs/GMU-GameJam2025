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
    [SerializeField] public HealthBar player_healthBar; 

    public bool isDying = false; //when youre taking damage lol
    private float dieFlash = 0.5f; 

    [HideInInspector]
    private bool isAttacking;

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

    public void PlayerHP(){ // the exact same code as the PlayerHealth.cs file
        health = (int)player_healthBar.healthSlider.value; //the slider value is the health
     }

    public override void TakeDamage(int damage){
        base.TakeDamage(damage);
        isDying = true;
        player_healthBar.healthSlider.value = health;
        StartCoroutine(ResetisDying());
        if (health <= 0)
        {
            SceneManager.LoadSceneAsync("GameOver");
        }
    }

     private IEnumerator ResetisDying(){
         yield return new WaitForSeconds(dieFlash);
         isDying = false;

     }

}