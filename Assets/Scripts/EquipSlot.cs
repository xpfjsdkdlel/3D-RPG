using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipSlot : MonoBehaviour
{
    public int number;
    private Item _item = null;
    public Item item
    {
        get => _item;
        set => _item = value;
    }
    private Image itemImg; // 아이템 이미지
    [SerializeField]
    private Inventory inventory;
    private GameSceneManager gameScene;

    private void Awake()
    {
        inventory = GameObject.FindObjectOfType<Inventory>();
        itemImg = transform.GetChild(0).GetComponent<Image>();
        gameScene = GameObject.FindObjectOfType<GameSceneManager>();
    }

    public void Refresh()
    {
        if (item != null)
        {
            itemImg.gameObject.SetActive(true);
            itemImg.sprite = item.iconImg;
        }
        else
            itemImg.gameObject.SetActive(false);
    }

    public void Clear()
    {
        // 인벤토리에 공간이 있다면 아이템 장착 해제
        if (inventory.GetItem(item))
        {
            AudioManager.Instance.PlaySFX(Resources.Load<AudioClip>("AudioSource/SFX/Equip"));
            GameManager.Instance.equip[number] = null;
            item = null;
            Refresh();
        }
        // 인벤토리가 열려있다면 인벤토리를 새로고침
        if (inventory.gameObject.activeSelf)
            inventory.RefreshSlot();
        gameScene.RefreshStat();
    }
}
