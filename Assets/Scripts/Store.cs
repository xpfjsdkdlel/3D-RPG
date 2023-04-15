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

    private int j;

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
        j = 0;
        for (int i = 0; i < GameManager.Instance.row * 3; i++)
        {
            if (GameManager.Instance.items[i] != null)
            {
                list = sellList.GetChild(j).GetComponent<ItemList>();
                list.gameObject.SetActive(true);
                list.item = GameManager.Instance.items[i];
                list.number = i;
                list.itemImg.sprite = list.item.iconImg;
                list.itemName.text = list.item.name;
                list.itemPrice.text = (list.item.price / 10).ToString();
                j++;
            }
        }
        for (; j < GameManager.Instance.row * 3; j++)
        {
            list = sellList.GetChild(j).GetComponent<ItemList>();
            list.item = null;
            list.gameObject.SetActive(false);
            j++;
        }
    }
}
