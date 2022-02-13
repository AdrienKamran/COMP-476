using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    // Set-up for teleporter script (set in editor per wall)
    public BoxCollider bc;
    public float offset = 2f;
    public bool north;
    public bool south;
    public bool east;
    public bool west;

    // When the player collides with a teleporter wall, teleport the player to the diametric opposite wall at an offset.
    // The offset is used to avoid infinite teleports.
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Wall triggered");
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player teleporting...");
            Transform playerToTeleport = other.GetComponent<Transform>();
            if (north)
            {
                Debug.Log("... North to South");
                playerToTeleport.position = new Vector3(playerToTeleport.position.x, playerToTeleport.position.y, -playerToTeleport.position.z + offset);
            }
            if (east)
            {
                Debug.Log("... East to West");
                playerToTeleport.position = new Vector3(-playerToTeleport.position.x + offset, playerToTeleport.position.y, playerToTeleport.position.z);
            }
            if (south)
            {
                Debug.Log("... South to North");
                playerToTeleport.position = new Vector3(playerToTeleport.position.x, playerToTeleport.position.y, -playerToTeleport.position.z - offset);
            }
            if (west)
            {
                Debug.Log("... West to East");
                playerToTeleport.position = new Vector3(-playerToTeleport.position.x - offset, playerToTeleport.position.y, playerToTeleport.position.z);
            }
        }
    }
}
