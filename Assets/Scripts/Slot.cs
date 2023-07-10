using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Slot : MonoBehaviour
{
    public int slotNumber;
    private Image itemImg; // 아이템 이미지
    private int count; // 아이템 갯수
    private TextMeshProUGUI countText;
    private Inventory inventory;

    private void Awake()
    {
        itemImg = transform.GetChild(0).GetComponent<Image>();
        countText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        inventory = GameObject.FindObjectOfType<Inventory>();
    }

    public void Use()
    {
        if(GameManager.Instance.items[slotNumber] != null)
            inventory.UseItem(slotNumber);
    }

    public void Refresh()
    {
        if (GameManager.Instance.items[slotNumber] != null)
        {
            count = GameManager.Instance.items[slotNumber].count;
            if (count <= 0)
            {
                itemImg.gameObject.SetActive(false);
                countText.gameObject.SetActive(false);
                GameManager.Instance.items[slotNumber] = null;
            }
            else
            {
                itemImg.gameObject.SetActive(true);
                countText.gameObject.SetActive(true);
                countText.text = count.ToString();
                itemImg.sprite = GameManager.Instance.items[slotNumber].iconImg;
            }
        }
        else
        {
            itemImg.gameObject.SetActive(false);
            countText.gameObject.SetActive(false);
            GameManager.Instance.items[slotNumber] = null;
        }
    }
}