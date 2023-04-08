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
    private Inventory inventory;

    private void Awake()
    {
        inventory = GameObject.FindObjectOfType<Inventory>();
        if (buy)
        {
            btnImg.sprite = btnColor[0];
            btnText.text = "����";
        }
        else
        {
            btnImg.sprite = btnColor[1];
            btnText.text = "�Ǹ�";
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
                Debug.Log("��尡 �����մϴ�.");
            }
        }
        else
        {

        }
    }
}
