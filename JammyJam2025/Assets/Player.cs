using UnityEngine;

public class Player : MonoBehaviour
{
    float gravity = -20;
    Vector3 velocity;

    PlayerController controller;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        velocity.y += gravity + Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
