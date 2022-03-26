using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TankPlayer : MonoBehaviour
{
    public float moveSpeed;
    public float turnSpeed;
    public AudioSource audioSource;
    public AudioClip tankFireClip;
    public AudioClip powerUpClip;
    public float bulletForce;
    public bool poweredUp;

    PhotonView view;

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
        view = GetComponent<PhotonView>();
    }

    // Start is called before the first frame update
    void Start()
    {
        poweredUp = false;
        fired = false;
        moveInput = 0f;
        turnInput = 0f;
        movement = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (view.IsMine)
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
    }

    private void FixedUpdate()
    {
        if (view.IsMine)
        {
            Move();
            Turn();
        }

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
        int modifier = 1;
        if (poweredUp)
        {
            modifier = 2;
        }
        Debug.Log("[TankPlayer] Bullet fired");
        fired = true;
        audioSource.PlayOneShot(tankFireClip);
        Rigidbody bulletInstance = Instantiate(bulletPrefab.GetComponent<Rigidbody>(), bulletSpawnPoint.position, bulletSpawnPoint.rotation) as Rigidbody;
        bulletInstance.velocity = modifier * bulletForce * bulletSpawnPoint.forward;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Power Up"))
        {
            Debug.Log("[TankPlayer] Power Up Collected");
            AudioSource.PlayClipAtPoint(powerUpClip, Camera.main.transform.position);
            Destroy(collision.gameObject);
            poweredUp = true;
        }
    }
}
