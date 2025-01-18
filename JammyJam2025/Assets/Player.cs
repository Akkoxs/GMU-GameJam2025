using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    float gravity = -20;
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
        Vector2 input = new(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        velocity.x = input.x * moveSpeed;
        velocity.y += gravity + Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
