using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    public GameObject[] slots;
    public void RefreshSlot()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if(GameManager.Instance.equip[i] != null)
                slots[i].GetComponent<EquipSlot>().item = GameManager.Instance.equip[i];
            slots[i].GetComponent<EquipSlot>().Refresh();
        }
    }
}
