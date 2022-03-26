using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    // Parameters to be adjusted in the editor
    [SerializeField] private float rotationDPS = 90f;

    // Update is called once per frame
    void Update()
    {
        // Rotate the transform around the Y axis at a set degrees per second, normalized for time
        transform.Rotate(rotationDPS * Time.deltaTime, 0, 0);
    }
}
