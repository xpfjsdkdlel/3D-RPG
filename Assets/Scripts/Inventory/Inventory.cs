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
    private List<Item> items; // ���� �������� ������ ���� ����Ʈ
    private List<Slot> slots = new List<Slot>(); // ui�� ��Ÿ�� ���� ����Ʈ

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
