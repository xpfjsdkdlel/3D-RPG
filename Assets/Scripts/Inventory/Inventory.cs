using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private GameObject slotPrefab;
    [SerializeField]
    private Transform slotParent;
    private Slot[] slots; // ui로 나타낼 슬롯 리스트

    private void Awake()
    {
        slots = new Slot[GameManager.Instance.row * 3];
        for (int i = 0; i < slots.Length; i++)
        {
            GameObject obj = Instantiate(slotPrefab);
            obj.transform.parent = slotParent;
            slots[i] = obj.GetComponent<Slot>();
        }
    }

    private void OnEnable()
    {
        RefreshSlot();
    }

    public void RefreshSlot()
    {
        for (int i = 0; i < GameManager.Instance.items.Length; i++)
        {
            slots[i].item = GameManager.Instance.items[i];
            slots[i].Refresh();
        }
    }

    public bool GetItem(Item _item)
    {
        for (int i = 0; i < GameManager.Instance.items.Length; i++)
        {
            if (GameManager.Instance.items[i] == null)
            {
                GameManager.Instance.items[i] = _item;
                slots[i].Refresh();
                return true;
            }
            else if (GameManager.Instance.items[i].uid == _item.uid)
            {
                if (GameManager.Instance.items[i].type && GameManager.Instance.items[i].count < 99)
                {
                    ++GameManager.Instance.items[i].count;
                    slots[i].Refresh();
                    return true;
                }
            }

        }
        return false;
    }
}
