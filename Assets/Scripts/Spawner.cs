using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private int spawnNumber;
    void Spawn()
    {


    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            Spawn();
    }
}
