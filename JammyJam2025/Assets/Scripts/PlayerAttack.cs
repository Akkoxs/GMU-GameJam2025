using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public static PlayerAttack Instance; //static to ensure 1 instance globally?
    public bool isAttacking = false;
    public bool isHeavyAttacking = false;
    public Animator animator;
    public Transform attackPoint;
    public LayerMask enemyLayer;
    public PlayerController player;
    public float targetTime = 1.2f;


    void Awake()
    {
        animator = GetComponent<Animator>();
        Instance = this; //sets Instance to this instance of this class? 
        isAttacking = false;
        isHeavyAttacking = false;
    }

    void Update()
    {
        Attack();
    }

    void Attack()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking && player.collisionInfo.below) 
        {
            isAttacking = true;
        }

        if (Input.GetMouseButtonDown(1) && !isAttacking && player.collisionInfo.below)
        {
            isHeavyAttacking = true;
        }
    }

    public void slashHitstop()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, 0.4f, enemyLayer);
        int damage;
        float hitS;
        if (hitEnemies.Length != 0)
        {
            if (isHeavyAttacking)
            {
                hitS = 0.25f;
                damage = 10;
            } else
            {
                hitS = 0.15f;
                damage = 5;
            }
            HitStop.Instance.Stop(hitS);
            foreach (Collider2D enemy in hitEnemies)
            {
                Enemy e = enemy.GetComponentInParent<Enemy>();
                if (animator.GetCurrentAnimatorStateInfo(0).IsTag("FinalAttack"))
                {
                    e.isFinalAttack = true;
                }

                if (isHeavyAttacking)
                {
                    e.isHeavyAttack = true;
                }

                e.isBeingAttacked = true;
                e.TakeDamage(damage);
            }
        }
    }

}                                                                           