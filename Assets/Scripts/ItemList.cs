using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemList : MonoBehaviour
{
    public Item item;
    public bool buy;
    public Image itemImg;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemPrice;
    public Sprite[] btnColor;
    public Image btnImg;
    public TextMeshProUGUI btnText;
    public int number;
    private Inventory inventory;
    private Store store;

    private void Awake()
    {
        inventory = GameObject.FindObjectOfType<Inventory>();
        store = GameObject.FindObjectOfType<Store>();
        if (buy)
        {
            btnImg.sprite = btnColor[0];
            btnText.text = "구입";
        }
        else
        {
            btnImg.sprite = btnColor[1];
            btnText.text = "판매";
        }
    }

    public void Deal()
    {
        if (buy)
        {
            if(item.price <= GameManager.Instance.gold)
            {
                inventory.GetItem(item);
                GameManager.Instance.gold -= item.price;
            }
            else
            {
                Debug.Log("골드가 부족합니다.");
            }
        }
        else
        {
            if (item.equip)
            {
                GameManager.Instance.items[number] = null;
                GameManager.Instance.gold += item.price / 10;
                gameObject.SetActive(false);
            }
            else
            {
                GameManager.Instance.items[number].count--;
                item.count--;
                GameManager.Instance.gold += item.price / 10;
                if (item.count <= 0)
                    gameObject.SetActive(false);
            }
        }
        inventory.slots[number].item = null;
        if (inventory.gameObject.activeSelf)
            inventory.RefreshSlot();
        store.RefreshList();
    }
}
