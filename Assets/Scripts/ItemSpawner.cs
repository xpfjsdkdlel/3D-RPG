using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField]
    private Item[] items;
    [SerializeField]
    private GameObject prefabs;

    int num = 0;

    private void Awake()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject pre = Instantiate(prefabs, transform.position, Quaternion.identity, transform);
            pre.SetActive(false);
        }
    }

    public void DropItem(Vector3 pos)
    {
        num = Random.Range(0, 14);
        Debug.Log(num);
        if(num < 11)
            SpawnItem(num, pos);
    }

    private void SpawnItem(int number, Vector3 pos)
    {
        GameObject select = null;
        ItemInfo itemInfo;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (!transform.GetChild(i).gameObject.activeSelf)
            {
                select = transform.GetChild(i).gameObject;
                select.SetActive(true);
                itemInfo = select.GetComponent<ItemInfo>();
                select.transform.position = pos + new Vector3(0, 1, 0);
                select.transform.rotation = Quaternion.identity;
                itemInfo.item = items[number];
                break;
            }
        }
        if(select == null)
        {
            GameObject pre = Instantiate(prefabs, transform.position, Quaternion.identity, transform);
            pre.SetActive(false);
        }
    }
}
