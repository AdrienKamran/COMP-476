using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public float moveSpeed;
    public float maxSpeed;
    public float rotationSpeed = 300f;
    public GameObject targetObj;
    public Rigidbody rb;
    private Animator animator;
    public float stopRadius = 1f;
    public float slowRadius = 6f;
    public float farRadius = 1000f;

    [SerializeField] private float distance;
    private Transform target;
    private float seekVelocityMagnitude = 0;

    [SerializeField] private Vector3 movement;

    // Start is called before the first frame update
    void Start()
    {
        movement = Vector3.zero;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        if (targetObj != null)
        {
            target = targetObj.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Blend", seekVelocityMagnitude / maxSpeed);
    }

    private void FixedUpdate()
    {
        if (targetObj != null)
        {
            target = targetObj.transform;
            // First, find the target.
            LockTarget();
            // Do the kinematic seek math to determine the velocity
            movement = KinematicSeek();
            LookAtMovement();
            Move();
        }
    }

    // Apply the movement vector to the rigidbody.
    void Move()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    // Kinematic seek, has built-in arrive function.
    Vector3 KinematicSeek()
    {
        Vector3 desiredVelocity = target.position - transform.position;
        float distance = desiredVelocity.magnitude;
        desiredVelocity = desiredVelocity.normalized * maxSpeed;

        if (distance <= stopRadius)
        {
            Debug.Log("Inside stop radius");
            desiredVelocity *= 0;
        }
        else if (distance < slowRadius)
        {
            Debug.Log("Inside slow radius");
            desiredVelocity *= (distance / slowRadius);
        }

        seekVelocityMagnitude = desiredVelocity.magnitude;
        Debug.Log(seekVelocityMagnitude);
        return desiredVelocity;
    }

    // Look in the direction of movement.
    void LookAtMovement()
    {
        // If the player is moving...
        if (movement != Vector3.zero)
        {
            // ... Create a quaternion which sets where the player should be facing.
            Quaternion targetRotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(movement, Vector3.up), rotationSpeed * Time.fixedDeltaTime);
            // Rotate towards that direction at a set rotation speed.
            rb.MoveRotation(targetRotation);
        }
    }

    // Save the NPC's target and the distance to said target.
    void LockTarget()
    {
        movement = target.position - transform.position;
        distance = movement.magnitude;
    }
}
