using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public static PlayerAttack Instance;
    public bool isAttacking = false;
    public Animator animator;

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
        if (Input.GetKeyDown(KeyCode.F) && !isAttacking)
        {
            Debug.Log("isAttacking: " + isAttacking);
            isAttacking = true;
        }
    }
}                                                                           