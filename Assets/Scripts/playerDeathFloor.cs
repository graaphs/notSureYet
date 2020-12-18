using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerDeathFloor : MonoBehaviour
{
    [SerializeField]
    Transform spawnPoint = null;

    private void OnCollisionEnter2D(Collision2D fall)
    {
        if (fall.transform.CompareTag("Player"))
            fall.transform.position = spawnPoint.position;
    }
}
