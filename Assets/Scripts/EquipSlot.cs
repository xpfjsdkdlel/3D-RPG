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
    private Image itemImg; // ������ �̹���
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
        // �κ��丮�� ������ �ִٸ� ������ ���� ����
        if (inventory.GetItem(item))
        {
            AudioManager.Instance.PlaySFX(Resources.Load<AudioClip>("AudioSource/SFX/Equip"));
            GameManager.Instance.equip[number] = null;
            item = null;
            Refresh();
        }
        // �κ��丮�� �����ִٸ� �κ��丮�� ���ΰ�ħ
        if (inventory.gameObject.activeSelf)
            inventory.RefreshSlot();
        gameScene.RefreshStat();
    }
}
