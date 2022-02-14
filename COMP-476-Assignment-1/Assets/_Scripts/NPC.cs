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
    public float farRadius = 15f;
    public float detectionHalfAngle;
    public float maxDetectionHalfAngle = 60f;
    public bool flee;

    private float distance;
    private Transform target;
    private float seekVelocityMagnitude = 0;

    [SerializeField] private Vector3 movement;

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
        UpdateDetectionHalfAngle();
        DrawDebugLines();
    }

    // All movement is set here, because ridigbodies like FixedUpdate.
    private void FixedUpdate()
    {
        // First, find the target.
        LockTarget();
        if (flee)
        {
            movement = KinematicFlee();
        }
        else
        {
            // If we're not fleeing, we're seeking.
            movement = KinematicSeek();
        }
        // Turn to the locked target.
        LookAtMovement();
        // If we're still out of the seek range, stop here.
        if (distance > farRadius)
        {
            return;
        }
        else
        {
            // If we're inside the far radius and inside the vision cone, move.
            if (InsideFOV())
            {
                Move();
            }
        }
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
        // Fancy math to make the seeking NPC go faster the further away you are.
        float distanceFactor = (1 / Mathf.Pow(farRadius, 2) * Mathf.Pow(distance, 2));
        desiredVelocity = desiredVelocity.normalized * distanceFactor * maxSpeed;

        if (distance <= stopRadius)
        {
            desiredVelocity *= 0;
        }
        else if (distance < slowRadius)
        {
            desiredVelocity *= (distance / slowRadius);
        }

        seekVelocityMagnitude = desiredVelocity.magnitude;
        return desiredVelocity;
    }

    Vector3 KinematicFlee()
    {
        Vector3 desiredVelocity = transform.position - target.position;
        float distance = desiredVelocity.magnitude;
        // Fancy math to make the fleeing NPC go faster the closer you are.
        float distanceFactor = 1 - (1 / Mathf.Pow(farRadius, 2) * Mathf.Pow(distance, 2));
        desiredVelocity = desiredVelocity.normalized * distanceFactor * maxSpeed;

        return desiredVelocity;
    }

    // Save the NPC's target and the distance to said target.
    void LockTarget()
    {
        movement = target.position - transform.position;
        distance = movement.magnitude;
    }

    // Some debug stuff to show detection radii, etc.
    private void OnDrawGizmos()
    {
        if (!flee)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, slowRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, stopRadius);
            Gizmos.color = Color.magenta;
            Quaternion leftAngle = Quaternion.AngleAxis(-detectionHalfAngle, Vector3.up);
            Quaternion rightAngle = Quaternion.AngleAxis(detectionHalfAngle, Vector3.up);
            Vector3 leftAngleResult = leftAngle * transform.forward;
            Vector3 rightAngleResult = rightAngle * transform.forward;
            Gizmos.DrawRay(transform.position, leftAngleResult * farRadius);
            Gizmos.DrawRay(transform.position, rightAngleResult * farRadius);
        }
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, farRadius);

    }

    private void DrawDebugLines()
    {
        Debug.DrawLine(transform.position, target.position, Color.green);
    }

    // Speed-based reduction of the detection angle. 
    private void UpdateDetectionHalfAngle()
    {
        if (distance > farRadius)
        {
            detectionHalfAngle = maxDetectionHalfAngle;
        }
        else
        {
            float speedFactor = 1 - (1 / Mathf.Pow(maxSpeed, 2) * Mathf.Pow(seekVelocityMagnitude, 2));
            detectionHalfAngle = maxDetectionHalfAngle * speedFactor;
        }
    }

    private bool InsideFOV()
    {
        if (Vector3.Angle(movement, transform.forward) < detectionHalfAngle)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
