using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private GameObject slotPrefab;
    [SerializeField]
    private Transform slotParent;
    private List<Slot> slots = new List<Slot>(); // ui로 나타낼 슬롯 리스트

    private void Awake()
    {
        for (int i = 0; i < GameManager.Instance.row * 3; i++)
        {
            GameObject obj = Instantiate(slotPrefab);
            obj.transform.parent = slotParent;
            slots.Add(obj.GetComponent<Slot>());
        }
    }

    private void OnEnable()
    {
        RefreshSlot();
    }

    public void RefreshSlot()
    {
        for (int i = 0; i < GameManager.Instance.items.Count; i++)
        {
            slots[i].item = GameManager.Instance.items[i];
            slots[i].Refresh();
        }
    }

    public void GetItem(Item _item)
    {
        for(int i = 0; i < GameManager.Instance.items.Count; i++)
        {
            if(GameManager.Instance.items[i].uid == _item.uid)
            {
                if (GameManager.Instance.items[i].type && GameManager.Instance.items[i].count < 99)
                    ++GameManager.Instance.items[i].count;
                else
                    continue;
            }
            else if(GameManager.Instance.items[i] == null)
            {
                GameManager.Instance.items[i] = _item;
                GameManager.Instance.items[i].count += _item.count;
            }
            slots[i].Refresh();
        }
    }
}
