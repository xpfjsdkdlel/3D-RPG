using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject inventory;
    bool active = false;
    private void Start()
    {
        inventory.SetActive(active);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            active = !active;
            inventory.SetActive(active);
        }
    }
}
