using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemList : MonoBehaviour
{
    public Item item;
    public bool buy;
    public Image itemImg;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemPrice;
    public Sprite[] btnColor;
    public Image btnImg;
    public TextMeshProUGUI btnText;
    public int number;
    private Inventory inventory;
    private Store store;
    private AudioClip trade;
    private AudioClip error;

    public void Init()
    {
        inventory = GameObject.FindObjectOfType<Inventory>();
        store = GameObject.FindObjectOfType<Store>();
        trade = Resources.Load<AudioClip>("AudioSource/SFX/Trade");
        error = Resources.Load<AudioClip>("AudioSource/SFX/Error");
    }

    private void OnEnable()
    {
        if (buy)
        {
            btnImg.sprite = btnColor[0];
            btnText.text = "����";
        }
        else
        {
            btnImg.sprite = btnColor[1];
            btnText.text = "�Ǹ�";
        }
    }

    public void Deal()
    {
        if (buy)
        {// ���� ��
            if (item.price <= GameManager.Instance.gold)
            {
                if (inventory.GetItem(item))
                {
                    Debug.Log("������ ����");
                    GameManager.Instance.gold -= item.price;
                    AudioManager.Instance.PlaySFX(trade);
                }
                else
                {
                    AudioManager.Instance.PlaySFX(error);
                    Debug.Log("�κ��丮�� ���� á���ϴ�.");
                }
            }
            else
            {
                AudioManager.Instance.PlaySFX(error);
                Debug.Log("��尡 �����մϴ�.");
            }
        }
        else
        {// �Ǹ� ��
            AudioManager.Instance.PlaySFX(trade);
            if (item.type)
            {
                GameManager.Instance.items[number] = null;
                GameManager.Instance.gold += item.price / 10;
                gameObject.SetActive(false);
            }
            else
            {
                --GameManager.Instance.items[number].count;
                GameManager.Instance.gold += item.price / 10;
                if (item.count <= 0)
                {
                    GameManager.Instance.items[number] = null;
                    gameObject.SetActive(false);
                }
            }
        }
        if (inventory == null)
            inventory = GameObject.FindObjectOfType<Inventory>();
        if(store == null)
            store = GameObject.FindObjectOfType<Store>();
        inventory.RefreshSlot();
        store.RefreshList();
    }
}
