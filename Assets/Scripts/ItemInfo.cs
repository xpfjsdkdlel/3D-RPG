using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    public Item item;
    private GameUI gameUI;

    private void OnEnable()
    {
        gameUI = GameObject.FindObjectOfType<GameUI>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Inventory inventory;
            if(gameUI.inventory.TryGetComponent<Inventory>(out inventory))
            {
                if (inventory.GetItem(item))
                {
                    inventory.RefreshSlot();
                    Debug.Log("æ∆¿Ã≈€ »πµÊ " + item.name);
                    Destroy(gameObject);
                }
                else
                {
                    Debug.Log("¿Œ∫•≈‰∏Æ∞° ∞°µÊ √°Ω¿¥œ¥Ÿ.");
                }
            }
        }
    }
}
