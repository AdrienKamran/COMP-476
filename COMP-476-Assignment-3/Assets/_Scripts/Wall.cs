using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Wall : MonoBehaviour
{
    private bool isDestroyed;
    private PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDestroyed)
        {
            return;
        }
        if (other.gameObject.CompareTag("Bullet"))
        {
            if (photonView.IsMine)
            {
                photonView.RPC("DestroyWallGlobally", RpcTarget.AllBufferedViaServer);
            }
            else
            {
                DestroyWallLocally();
            }
        }
    }

    [PunRPC]
    private void DestroyWallGlobally()
    {
        isDestroyed = true;
        PhotonNetwork.Destroy(gameObject);
    }

    private void DestroyWallLocally()
    {
        isDestroyed = true;
        this.gameObject.SetActive(false);
    }
}
