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
    public PlayerController player;

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
        if (Input.GetMouseButtonDown(0) && !isAttacking && player.collisionInfo.below)
        {
            isAttacking = true;
        }

    }

    public void slashHitstop()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, 0.7f, enemyLayer);
        if (hitEnemies.Length != 0)
        {
            HitStop.Instance.Stop(0.15f);
            foreach (Collider2D enemy in hitEnemies)
            {
                Debug.Log("hit Enemies: " + enemy);
                enemy.GetComponentInParent<Enemy>().TakeDamage(30);
                enemy.GetComponentInParent<Rigidbody2D>().linearVelocity = new Vector2(-player.directionX * 1000, 0);
            }
        }
    }
}                                                                           