using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public static PlayerAttack Instance;
    public bool isAttacking = false;
    public Animator animator;
    public Transform attackPoint;
    public LayerMask enemyLayer;
    public Player player;

    void Awake()
    {
        animator = GetComponent<Animator>();
        Instance = this;
        isAttacking = false;
    }

    void Update()
    {
        Attack();
    }

    void Attack()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking && player.velocity.y < 0)
        {
            player.velocity.x = 0f;
            Debug.Log("isAttacking: " + isAttacking);
            isAttacking = true;
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, 0.5f, enemyLayer);

            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<Enemy>().TakeDamage(50);
            }

            if (hitEnemies != null)
            {
            }
        }

    }

    public void slashHitstop()
    {
        HitStop.Instance.Stop(0.15f);
    }
}                                                                           