using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Slot : MonoBehaviour
{
    private Item _item;
    public Item item
    {
        get => _item;
        set => _item = value;
    }
    private Image itemImg; // 아이템 이미지
    private TextMeshProUGUI countText;
    private int count; // 아이템 개수

    private void Start()
    {
        itemImg = transform.GetChild(0).GetComponent<Image>();
        countText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    public void Refresh()
    {
        if(_item != null)
        {
            if (count <= 0)
            {
                for (int i = 0; i < transform.childCount; i++)
                    transform.GetChild(i).gameObject.SetActive(false);
                _item = null;
            }
            else
            {
                for (int i = 0; i < transform.childCount; i++)
                    transform.GetChild(i).gameObject.SetActive(true);
                countText.text = count.ToString();
                itemImg.sprite = _item.iconImg;
            }
        }
        else
        {
            for (int i = 0; i < transform.childCount; i++)
                transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}