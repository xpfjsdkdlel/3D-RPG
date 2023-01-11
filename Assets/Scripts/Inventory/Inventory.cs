using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private GameObject slotPrefab;
    [SerializeField]
    private Transform slotParent;
    private int row = 5;
    private int index;
    private List<Item> items; // 실제 아이템의 정보를 갖는 리스트
    private List<Slot> slots = new List<Slot>(); // ui로 나타낼 슬롯 리스트

    private void Awake()
    {
        for(int i = 0; i < row * 3; i++)
        {
            GameObject obj = Instantiate(slotPrefab);
            obj.transform.parent = slotParent;
            slots.Add(obj.GetComponent<Slot>());
        }
        for(int i = 0; i < slots.Count; i++)
        {
            slots[i].Refresh();
        }
    }

    public void GetItem()
    {

    }
}
