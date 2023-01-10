using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private GameObject slotPrefab;
    private int maxCount = 15;
    private int index;

    private List<Slot> slots = new List<Slot>();

    private void Awake()
    {
        for(int i = 0; i < maxCount; i++)
        {
            GameObject obj = Instantiate(slotPrefab);
            obj.transform.parent = transform;
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
