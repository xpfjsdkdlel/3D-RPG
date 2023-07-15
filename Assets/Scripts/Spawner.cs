using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject monster;
    [SerializeField]
    private int maxCount; // 최대로 소환될 수 있는 몬스터의 수
    [SerializeField]
    private int spawnCount = 1; // 한번에 스폰되는 몬스터의 수
    [SerializeField]
    private float timer; // 리스폰에 걸리는 시간
    [SerializeField]
    private float prevTimer; // 경과된 시간
    [SerializeField]
    private bool action = false; // 활성화 여부

    private void Awake()
    {
        for (int i = 0; i < maxCount; i++)
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
                select.transform.position = transform.position + new Vector3(Random.Range(-4, 4), 0, Random.Range(-2, 2));
                select.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
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
                for (int i = 0; i < spawnCount; i++)
                    SpawnMonster();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            action = false;
        if (other.CompareTag("Enemy"))
            other.GetComponent<Enemy>().Retreat();
    }
}
