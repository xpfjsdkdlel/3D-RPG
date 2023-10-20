using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField]
    private Item[] items;
    [SerializeField]
    private GameDB gameDB;
    [SerializeField]
    private GameObject prefabs;
    private AudioClip sound;

    int num = 0;

    private void Awake()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject pre = Instantiate(prefabs, transform.position, Quaternion.identity, transform);
            pre.SetActive(false);
        }
        items = new Item[gameDB.ItemData.Count];
        for (int i = 0; i < gameDB.ItemData.Count; i++)
        {
            items[i] = new Item();
            items[i].uid = gameDB.ItemData[i].uid;
            items[i].type = gameDB.ItemData[i].type;
            items[i].name = gameDB.ItemData[i].name;
            items[i].iconImg = Resources.Load <Sprite>("Sprite/" + gameDB.ItemData[i].iconImg);
            items[i].classNum = gameDB.ItemData[i].classNum;
            items[i].price = gameDB.ItemData[i].price;
            items[i].count = gameDB.ItemData[i].count;
            items[i].maxCount = gameDB.ItemData[i].maxCount;
            items[i].damage = gameDB.ItemData[i].damage;
            items[i].armor = gameDB.ItemData[i].armor;
            items[i].speed = gameDB.ItemData[i].speed;
            items[i].range = gameDB.ItemData[i].range;
            items[i].heal = gameDB.ItemData[i].heal;
        }
            sound = Resources.Load<AudioClip>("AudioSource/SFX/Item");
    }

    public void DropItem(Vector3 pos)
    {
        num = Random.Range(0, 20);
        Debug.Log(num);
        if(num < gameDB.ItemData.Count)
            SpawnItem(num, 1, pos);
    }

    private void SpawnItem(int number, int count, Vector3 pos)
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
                itemInfo.item.count = count;
                itemInfo.sound = sound;
                break;
            }
        }
        if (select == null)
        {
            GameObject pre = Instantiate(prefabs, transform.position, Quaternion.identity, transform);
            pre.SetActive(false);
        }
    }
}
