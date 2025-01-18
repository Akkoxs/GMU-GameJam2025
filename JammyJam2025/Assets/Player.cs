using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    float gravity = -9.8f;
    public float jumpVelocity;
    Vector3 velocity;

    PlayerController controller;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        //stop player from moving if detect collision above or below
        if (controller.collisionInfo.top || controller.collisionInfo.bottom)
        {
            velocity.y = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space) && controller.collisionInfo.bottom)
        {
            Debug.Log("here in jump");
            velocity.y = jumpVelocity;
            Debug.Log("y velocity: " +  velocity.y);
        }

        Vector2 input = new(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")); //input

        //gravity + movement
        velocity.x = input.x * moveSpeed;
        velocity.y += gravity + Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
