using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player-controlled movement script for the sake of testing
public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody rb;

    Vector3 movement;

    // Start is called before the first frame update
    void Start()
    {
        movement = Vector3.zero;
    }

    // Update is called once per frame
    // Input goes here
    void Update()
    {
        movement = Vector3.zero;
        movement.x = Input.GetAxis("Horizontal");
        movement.z = Input.GetAxis("Vertical");
    }

    // Movement goes here
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
