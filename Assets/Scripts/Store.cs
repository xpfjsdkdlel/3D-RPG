using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Store : MonoBehaviour
{
    [SerializeField]
    private GameObject ItemList; // 생성할 프리팹
    [SerializeField]
    private Transform buyList; // 프리팹을 생성할 위치
    [SerializeField]
    private Transform sellList; // 프리팹을 생성할 위치
    [SerializeField]
    private TextMeshProUGUI goldText;
    private List<Item> items;
    private ItemList list;

    private void OnEnable()
    {
        for (int i = 0; i < buyList.childCount; i++)
        {
            list = buyList.GetChild(i).GetComponent<ItemList>();
            list.itemImg.sprite = list.item.iconImg;
            list.itemName.text = list.item.name;
            list.itemPrice.text = list.item.price.ToString();
        }
        for (int i = 0; i < GameManager.Instance.row * 3; i++)
            Instantiate(ItemList, sellList).SetActive(false);
        RefreshList();
        gameObject.SetActive(false);
    }

    public void RefreshList()
    {
        items = null;
        for(int i = 0; i < buyList.childCount; i++)
        {
            list = sellList.GetChild(i).GetComponent<ItemList>();
            if (list.item.price <= GameManager.Instance.gold)
                list.itemPrice.color = Color.white;
            else
                list.itemPrice.color = Color.red;
        }
        goldText.text = GameManager.Instance.gold.ToString();
        for (int i = 0; i < GameManager.Instance.items.Length; i++)
            items.Add(GameManager.Instance.items[i]);
        for (int i = 0; i < GameManager.Instance.row * 3; i++)
        {
            if (i < items.Count)
            {
                list = sellList.GetChild(i).GetComponent<ItemList>();
                list.item = items[i];
                list.itemImg.sprite = list.item.iconImg;
                list.itemName.text = list.item.name;
                list.itemPrice.text = list.item.price.ToString();
            }
            else
                sellList.GetChild(i).gameObject.SetActive(false);
        }
    }
}
