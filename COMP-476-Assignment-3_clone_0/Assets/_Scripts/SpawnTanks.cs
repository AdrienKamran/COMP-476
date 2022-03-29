using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnTanks : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject zombiePrefab;
    public Transform firstPlayerSpawnPoint;
    public Transform secondPlayerSpawnPoint;
    public Transform firstZombieSpawnPoint;
    public Transform secondZombieSpawnPoint;

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(playerPrefab.name, firstPlayerSpawnPoint.position, Quaternion.identity);
            PhotonNetwork.Instantiate(zombiePrefab.name, firstZombieSpawnPoint.position, Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate(playerPrefab.name, secondPlayerSpawnPoint.position, Quaternion.identity);
            PhotonNetwork.Instantiate(zombiePrefab.name, secondZombieSpawnPoint.position, Quaternion.identity);
        }
    }
}
