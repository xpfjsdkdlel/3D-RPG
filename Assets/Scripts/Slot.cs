using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Slot : MonoBehaviour
{
    private Image itemImg; // ������ �̹���
    private TextMeshProUGUI countText;
    private int count; // ������ ����

    private void Start()
    {
        itemImg = transform.GetChild(0).GetComponent<Image>();
        countText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    public void Refresh()
    {
        if(count <= 0)
        {
            for(int i = 0; i < transform.childCount; i++)
                transform.GetChild(i).gameObject.SetActive(false);
        }
        else
        {
            for (int i = 0; i < transform.childCount; i++)
                transform.GetChild(i).gameObject.SetActive(true);
            countText.text = count.ToString();
        }
    }

    public void AddItem(Item item)
    {
        ++count;
        itemImg = Resources.Load<Image>("Sprite/" + item.iconImg);
        Refresh();
    }

    public void UseItem()
    {

    }

    public void DeleteItem()
    {
        --count;
        Refresh();
    }
}