using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Store : MonoBehaviour
{
    [SerializeField]
    private GameDB gameDB; // 아이템 데이터
    [SerializeField]
    private GameObject ItemList; // 생성할 프리팹
    [SerializeField]
    private Transform buyList; // 프리팹을 생성할 위치
    [SerializeField]
    private Transform sellList; // 프리팹을 생성할 위치
    [SerializeField]
    private TextMeshProUGUI goldText;
    private ItemList list;

    private void Awake()
    {
        for (int i = 0; i < 15; i++)
        {
            Instantiate(ItemList, sellList).SetActive(false);
        }
        for (int i = 0; i < gameDB.ItemData.Count; i++)
        {
            GameObject obj = Instantiate(ItemList, buyList);
            list = obj.GetComponent<ItemList>();
            list.buy = true;
            list.item = new Item();
            list.item.uid = gameDB.ItemData[i].uid;
            list.item.type = gameDB.ItemData[i].type;
            list.item.name = gameDB.ItemData[i].name;
            list.item.iconImg = Resources.Load<Sprite>("Sprite/" + gameDB.ItemData[i].iconImg);
            list.item.classNum = gameDB.ItemData[i].classNum;
            list.item.price = gameDB.ItemData[i].price;
            list.item.count = gameDB.ItemData[i].count;
            list.item.maxCount = gameDB.ItemData[i].maxCount;
            list.item.damage = gameDB.ItemData[i].damage;
            list.item.armor = gameDB.ItemData[i].armor;
            list.item.speed = gameDB.ItemData[i].speed;
            list.item.range = gameDB.ItemData[i].range;
            list.item.heal = gameDB.ItemData[i].heal;
            list.itemImg.sprite = list.item.iconImg;
            list.itemName.text = list.item.name;
            list.itemPrice.text = list.item.price.ToString();
            list.ButtonSetting();
        }
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        RefreshList();
    }

    public void RefreshList()
    {
        for (int i = 0; i < buyList.childCount; i++)
        {
            list = buyList.GetChild(i).GetComponent<ItemList>();
            if (list.item.price > GameManager.Instance.gold)
                list.itemPrice.color = Color.red;
            else
                list.itemPrice.color = Color.white;
        }
        goldText.text = GameManager.Instance.gold.ToString();
        for (int i = 0; i < 15; i++)
        {
            list = sellList.GetChild(i).GetComponent<ItemList>();
            if (GameManager.Instance.items[i] != null)
            {
                list.gameObject.SetActive(true);
                list.item = GameManager.Instance.items[i];
                list.number = i;
                list.itemImg.sprite = list.item.iconImg;
                list.itemName.text = list.item.name;
                list.itemPrice.text = (list.item.price / 10).ToString();
            }
            else
            {
                list.item = null;
                list.gameObject.SetActive(false);
            }
        }
    }
}
