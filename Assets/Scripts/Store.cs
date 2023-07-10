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
    private ItemList list;

    private void Awake()
    {
        for (int i = 0; i < GameManager.Instance.row * 3; i++)
            Instantiate(ItemList, sellList).SetActive(false);
        for (int i = 0; i < buyList.childCount; i++)
        {
            list = buyList.GetChild(i).GetComponent<ItemList>();
            list.itemImg.sprite = list.item.iconImg;
            list.itemName.text = list.item.name;
            list.itemPrice.text = list.item.price.ToString();
        }
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        RefreshList();
    }

    public void RefreshList()
    {
        for(int i = 0; i < buyList.childCount; i++)
        {
            list = buyList.GetChild(i).GetComponent<ItemList>();
            if (list.item.price > GameManager.Instance.gold)
                list.itemPrice.color = Color.red;
            else
                list.itemPrice.color = Color.white;
        }
        goldText.text = GameManager.Instance.gold.ToString();
        for (int i = 0; i < GameManager.Instance.row * 3; i++)
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
