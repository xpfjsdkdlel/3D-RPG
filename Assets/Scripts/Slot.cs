using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Slot : MonoBehaviour
{
    public int slotNumber;
    private Item _item = null;
    public Item item
    {
        get => _item;
        set => _item = value;
    }
    private Image itemImg; // 아이템 이미지
    private TextMeshProUGUI countText;
    private int count; // 아이템 개수
    private Inventory inventory;

    private void Awake()
    {
        itemImg = transform.GetChild(0).GetComponent<Image>();
        countText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        inventory = GameObject.FindObjectOfType<Inventory>();
    }

    public void Use()
    {
        if(item != null)
            inventory.UseItem(slotNumber);
    }

    public void Refresh()
    {
        if(item != null)
        {
            count = item.count;
            if (count <= 0)
            {
                itemImg.gameObject.SetActive(false);
                countText.gameObject.SetActive(false);
                GameManager.Instance.items[slotNumber] = null;
                item = null;
            }
            else
            {
                itemImg.gameObject.SetActive(true);
                countText.gameObject.SetActive(true);
                countText.text = count.ToString();
                itemImg.sprite = item.iconImg;
            }
        }
        else
        {
            itemImg.gameObject.SetActive(false);
            countText.gameObject.SetActive(false);
        }
    }
}