using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player-controlled movement script for the sake of testing
public class Player : MonoBehaviour
{
    public float moveSpeed = 15f;
    public float rotationSpeed = 300f;
    public Rigidbody rb;

    Vector3 movement;

    // Start is called before the first frame update
    void Start()
    {
        movement = Vector3.zero;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    // Input goes here
    void Update()
    {
        // Capture the 2D movement from the input keys (WASD or directional keys)
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.z = Input.GetAxisRaw("Vertical");
        movement.Normalize();
    }

    // Movement goes here
    private void FixedUpdate()
    {
        // Move the player in the chosen direction at a set move speed
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        // If the player is moving...
        if (movement!= Vector3.zero)
        {
            // ... Create a quaternion which sets where the player should be facing
            Quaternion targetRotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(movement), rotationSpeed * Time.fixedDeltaTime);
            // Rotate towards that direction at a set rotation speed
            rb.MoveRotation(targetRotation);
        }

    }
}
