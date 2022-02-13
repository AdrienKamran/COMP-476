using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public float moveSpeed = 15f;
    public float rotationSpeed = 300f;
    public Rigidbody rb;

    Vector3 movement;
    public float frequency = 3f;
    private float timeRemaining;

    // Start is called before the first frame update
    void Start()
    {
        movement = Vector3.zero;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Wandering behaviour
        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0)
        {
            movement = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
            //movement.Normalize();
            timeRemaining += frequency;
        }
    }

    private void FixedUpdate()
    {
        // Move the player in the chosen direction at a set move speed
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        // If the player is moving...
        if (movement != Vector3.zero)
        {
            // ... Create a quaternion which sets where the player should be facing
            Quaternion targetRotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(movement), rotationSpeed * Time.fixedDeltaTime);
            // Rotate towards that direction at a set rotation speed
            rb.MoveRotation(targetRotation);
        }
    }

}
