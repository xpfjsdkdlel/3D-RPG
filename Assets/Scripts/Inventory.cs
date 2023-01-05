using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItemData
{
    public int uid; // 아이템의 고유 ID
    public int itemID; // 아이템의 테이블을 참조하기 위한 ID
    public int amount; // 겹치는 갯수
}

[System.Serializable]
public class Inventory
{
    private int maxSlotCount = 15;
    public int MAXSLOTCOUNT { get => maxSlotCount; }
    private int curSlotCount;
    public int CURSLOTCOUNT 
    {
        get => curSlotCount;
        set => curSlotCount = value;
    }
    [SerializeField]
    private List<InventoryItemData> items = new List<InventoryItemData>();

    public void AddItem(InventoryItemData newItem)
    {
        int index = FindItemIndex(newItem);
    }

    public bool IsFull()
    {
        return curSlotCount >= maxSlotCount;
    }

    private int FindItemIndex(InventoryItemData newItem)
    {
        int result = -1;

        for(int i = items.Count - 1; i >= 0; i--)
        {
            if(items[i].uid == newItem.uid)
            {
                result = i;
                break;
            }
        }
        return result;
    }
}
