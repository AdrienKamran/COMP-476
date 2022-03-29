using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviour
{
    public AudioClip tankHitClip;
    public AudioClip wallHitClip;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 10f);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("[Bullet] Wall hit by bullet");
            AudioSource.PlayClipAtPoint(wallHitClip, Camera.main.transform.position);
            //Destroy(other.gameObject);
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("NPC"))
        {
            Debug.Log("[Bullet] Tank hit by bullet");
            AudioSource.PlayClipAtPoint(tankHitClip, Camera.main.transform.position);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
