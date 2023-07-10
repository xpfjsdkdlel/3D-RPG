using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    public Item item;
    public AudioClip sound;
    private GameUI gameUI;
    private Inventory inventory;

    private void OnEnable()
    {
        gameUI = GameObject.FindObjectOfType<GameUI>();
        inventory = gameUI.inventory.GetComponent<Inventory>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (inventory.GetItem(item))
            {
                AudioManager.Instance.PlaySFX(sound);
                if (inventory.gameObject.activeSelf)
                    inventory.RefreshSlot();
                Debug.Log("æ∆¿Ã≈€ »πµÊ " + item.name);
                gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("¿Œ∫•≈‰∏Æ∞° ∞°µÊ √°Ω¿¥œ¥Ÿ.");
            }
        }
    }
}
