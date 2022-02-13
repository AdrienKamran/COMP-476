using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float maxSpeed = 15f;
    public float rotationSpeed = 300f;
    public Rigidbody rb;
    public GameObject targetObj;
    public float frequency = 3f;
    public float stopRadius = 3f;
    public float slowRadius = 6f;

    private float timeRemaining;
    private Transform target;

    Vector3 movement;

    // Start is called before the first frame update
    void Start()
    {
        movement = Vector3.zero;
        rb = GetComponent<Rigidbody>();
        target = targetObj.transform;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        movement = KinematicSeek();
        Move();
        LookAtMovement();
    }

    // Look in the direction of movement
    void LookAtMovement()
    {
        // If the player is moving...
        if (movement != Vector3.zero)
        {
            // ... Create a quaternion which sets where the player should be facing
            Quaternion targetRotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(movement), rotationSpeed * Time.fixedDeltaTime);
            // Rotate towards that direction at a set rotation speed
            rb.MoveRotation(targetRotation);
        }
    }

    void Move()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    Vector3 KinematicSeek()
    {
        Vector3 desiredVelocity = target.position - transform.position;
        float distance = desiredVelocity.magnitude;
        desiredVelocity = desiredVelocity.normalized * maxSpeed;

        if (distance <= stopRadius)
        {
            desiredVelocity *= 0;
        }
        else if (distance < slowRadius)
        {
            desiredVelocity *= (distance / slowRadius);
        }

        return desiredVelocity;
    }


}
