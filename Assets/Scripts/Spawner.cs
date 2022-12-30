using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject monster;
    [SerializeField]
    private int count;
    [SerializeField]
    private float timer;
    [SerializeField]
    private float prevTimer;
    [SerializeField]
    private bool action = false;

    private void Awake()
    {
        for (int i = 0; i < count; i++)
        {
            GameObject mon = Instantiate(monster, transform.position, Quaternion.identity, transform);
            mon.SetActive(false);
        }
        prevTimer = timer;
    }

    private void SpawnMonster()
    {
        GameObject select = null;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (!transform.GetChild(i).gameObject.activeSelf)
            {
                select = transform.GetChild(i).gameObject;
                select.SetActive(true);
                select.transform.position = transform.position + new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2));
                break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            action = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            prevTimer += Time.deltaTime;
            if(prevTimer >= timer && action)
            {
                prevTimer = 0;
                SpawnMonster();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            action = false;
    }
}
