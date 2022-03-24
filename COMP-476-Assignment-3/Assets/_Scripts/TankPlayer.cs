using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankPlayer : MonoBehaviour
{
    public float moveSpeed;
    public float turnSpeed;
    public AudioSource audioSource;
    public AudioClip tankFireClip;
    public float bulletForce;

    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;

    private float moveInput;
    private float turnInput;
    private Vector3 movement;
    private Quaternion turnRotation;
    private bool fired;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        fired = false;
        moveInput = 0f;
        turnInput = 0f;
        movement = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = Input.GetAxisRaw("Vertical");
        turnInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Fire1") && !fired)
        {
            Fire();
        }
        else if (Input.GetButtonUp("Fire1") && fired)
        {
            fired = false;
        }
    }

    private void FixedUpdate()
    {
        Move();
        Turn();
    }

    private void Move()
    {
        movement = transform.forward * moveInput * moveSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + movement);
    }

    private void Turn()
    {
        float turn = turnInput * turnSpeed * Time.deltaTime;
        turnRotation = Quaternion.Euler(0f, turn, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);
    }

    private void Fire()
    {
        Debug.Log("[TankPlayer] Bullet fired");
        fired = true;
        audioSource.PlayOneShot(tankFireClip);
        Rigidbody bulletInstance = Instantiate(bulletPrefab.GetComponent<Rigidbody>(), bulletSpawnPoint.position, bulletSpawnPoint.rotation) as Rigidbody;
        bulletInstance.velocity = bulletForce * bulletSpawnPoint.forward;
    }
}
