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
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            Debug.Log("isAttacking: " + isAttacking);
            isAttacking = true;
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, 0.5f, enemyLayer);

            foreach (Collider2D enemy in hitEnemies)
            {

            }
        }

    }
}                                                                           