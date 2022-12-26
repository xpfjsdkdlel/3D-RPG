using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private int spawnNumber;
    void Spawn()
    {
        GameObject enemy = GameSceneManager.instance.pool.Get(spawnNumber);
        enemy.transform.position = transform.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            Spawn();
    }
}
