using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankZombie : MonoBehaviour
{
    public Transform target;
    public AudioSource audioSource;
    public AudioClip tankFireClip;
    public float bulletForce;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;

    private NavMeshAgent agent;
    private bool fired;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = target.position;
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(
            // Where is the ray starting from
            bulletSpawnPoint.transform.position,
            // Where is the ray pointing to
            bulletSpawnPoint.transform.TransformDirection(Vector3.forward),
            // The raycast instance
            out hit,
            // The reach of the raycast
            Mathf.Infinity)
            &&
            hit.transform.CompareTag("Player")
            )
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            if (!fired)
            {
                StartCoroutine(Fire());
            }
        }
    }

    private IEnumerator Fire()
    {
        Debug.Log("[TankZombie] Bullet fired");
        fired = true;
        audioSource.PlayOneShot(tankFireClip);
        Rigidbody bulletInstance = Instantiate(bulletPrefab.GetComponent<Rigidbody>(), bulletSpawnPoint.position, bulletSpawnPoint.rotation) as Rigidbody;
        bulletInstance.velocity = bulletForce * bulletSpawnPoint.forward;
        yield return new WaitForSeconds(3);
        fired = false;
    }
}
