using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private GameObject slotPrefab;
    [SerializeField]
    private Transform slotParent;
    [SerializeField]
    private TextMeshProUGUI goldText;
    public Slot[] slots; // ui�� ��Ÿ�� ���� ����Ʈ
    private GameSceneManager gameScene;
    [SerializeField]
    private CharacterController player;
    [SerializeField]
    private Equipment equipment;
    [SerializeField]
    private AudioClip equip;
    [SerializeField]
    private AudioClip potion;
    [SerializeField]
    private AudioClip error;
    public Store store;
    private Item tempItem;

    public void Init()
    {
        slots = new Slot[GameManager.Instance.row * 3];
        for (int i = 0; i < slots.Length; i++)
        {
            GameObject obj = Instantiate(slotPrefab);
            obj.transform.parent = slotParent;
            slots[i] = obj.GetComponent<Slot>();
            slots[i].slotNumber = i;
        }
        gameScene = GameObject.FindObjectOfType<GameSceneManager>();
        store = GameObject.FindObjectOfType<Store>();
    }

    public void RefreshSlot()
    {// �� ���� ���ΰ�ħ
        for (int i = 0; i < GameManager.Instance.items.Length; i++)
            slots[i].Refresh();
        goldText.text = GameManager.Instance.gold.ToString();
    }

    public bool GetItem(Item _item)
    {// ������ ȹ��
        int n = GameManager.Instance.items.Length;
        tempItem = null;
        tempItem = new Item();
        tempItem.uid = _item.uid;
        tempItem.equip = _item.equip;
        tempItem.name = _item.name;
        tempItem.iconImg = _item.iconImg;
        tempItem.classNum = _item.classNum;
        tempItem.price = _item.price;
        tempItem.count = _item.count;
        tempItem.maxCount = _item.maxCount;
        tempItem.stat = _item.stat;
        if (!tempItem.equip)
        {
            for (int i = 0; i < GameManager.Instance.items.Length; i++)
            {
                if (GameManager.Instance.items[i] == null)
                {
                    if (i < n)
                        n = i;
                }
                else if (GameManager.Instance.items[i].uid == tempItem.uid && GameManager.Instance.items[i].count < GameManager.Instance.items[i].maxCount)
                {// ���� �������̸� count ����
                    if(GameManager.Instance.items[i].maxCount < GameManager.Instance.items[i].count + tempItem.count)
                    {// ȹ���� �������� ���� ���� ���� ���� ���� �ִ� �������� ���ٸ�
                        tempItem.count = GameManager.Instance.items[i].count - GameManager.Instance.items[i].maxCount;
                        GameManager.Instance.items[i].count = GameManager.Instance.items[i].maxCount;
                        n = GameManager.Instance.items.Length;
                    }
                }
                else
                {
                    GameManager.Instance.items[i].count += tempItem.count;
                    return true;
                }
            }
        }
        for (int i = 0; i < GameManager.Instance.items.Length; i++)
        {
            if (GameManager.Instance.items[i] == null)
            {
                GameManager.Instance.items[i] = tempItem;
                GameManager.Instance.items[i].count = 1;
                return true;
            }
        }
        return false;
    }

    public void UseItem(int num)
    {// ������ ���
        player = gameScene.player;
        Item _item = GameManager.Instance.items[num];
        if (_item.equip)
        {// ����� ���
            if (_item.classNum == 3)
            {// �� ����
                AudioManager.Instance.PlaySFX(equip);
                switch (_item.uid)
                {
                    case 201:
                        if(GameManager.Instance.equip[0] != null)
                        {
                            Item prevItem = GameManager.Instance.equip[0];
                            GameManager.Instance.equip[0] = _item;
                            GameManager.Instance.items[num] = prevItem;
                            Debug.Log(prevItem.name + "��(��) " + _item.name + "��(��) ��ü");
                        }
                        else
                        {
                            GameManager.Instance.equip[0] = _item;
                            GameManager.Instance.items[num] = null;
                            Debug.Log(_item.name + " ����");
                        }
                        break;
                    case 202:
                        if (GameManager.Instance.equip[1] != null)
                        {
                            Item prevItem = GameManager.Instance.equip[1];
                            GameManager.Instance.equip[1] = _item;
                            GameManager.Instance.items[num] = prevItem;
                            Debug.Log(prevItem.name + "��(��) " + _item.name + "��(��) ��ü");
                        }
                        else
                        {
                            GameManager.Instance.equip[1] = _item;
                            GameManager.Instance.items[num] = null;
                            Debug.Log(_item.name + " ����");
                        }
                        break;
                    case 203:
                        if (GameManager.Instance.equip[2] != null)
                        {
                            Item prevItem = GameManager.Instance.equip[2];
                            GameManager.Instance.equip[2] = _item;
                            GameManager.Instance.items[num] = prevItem;
                            Debug.Log(prevItem.name + "��(��) " + _item.name + "��(��) ��ü");
                        }
                        else
                        {
                            GameManager.Instance.equip[2] = _item;
                            GameManager.Instance.items[num] = null;
                            Debug.Log(_item.name + " ����");
                        }
                        break;
                    default:
                        break;
                }
            }
            else if (GameManager.Instance.number == _item.classNum)
            {
                if(_item.uid < 200)
                {// ���� ����
                    AudioManager.Instance.PlaySFX(equip);
                    if (GameManager.Instance.equip[3] != null)
                    {
                        Item prevItem = GameManager.Instance.equip[3];
                        GameManager.Instance.equip[3] = _item;
                        GameManager.Instance.items[num] = prevItem;
                        Debug.Log(prevItem.name + "��(��) " + _item.name + "��(��) ��ü");
                    }
                    else
                    {
                        GameManager.Instance.equip[3] = _item;
                        GameManager.Instance.items[num] = null;
                        Debug.Log(_item.name + " ����");
                    }
                }
                else
                {// �������� ����
                    AudioManager.Instance.PlaySFX(equip);
                    if (GameManager.Instance.equip[4] != null)
                    {
                        Item prevItem = GameManager.Instance.equip[4];
                        GameManager.Instance.equip[4] = _item;
                        GameManager.Instance.items[num] = prevItem;
                        Debug.Log(prevItem.name + "��(��) " + _item.name + "��(��) ��ü");
                    }
                    else
                    {
                        GameManager.Instance.equip[4] = _item;
                        GameManager.Instance.items[num] = null;
                        Debug.Log(_item.name + " ����");
                    }
                }
            }
            else
            {
                AudioManager.Instance.PlaySFX(error);
                Debug.Log("������ �� ���� ����Դϴ�.");
            }
            if(equipment.gameObject.activeSelf)
                equipment.RefreshSlot();
        }
        else
        {// ������ ���
            switch (_item.uid)
            {
                case 401:
                    if (player.maxHP <= player.HP + _item.stat)
                        player.HP = player.maxHP;
                    else
                        player.HP += _item.stat;
                    Debug.Log("ü������ ���");
                    break;
                case 402:
                    if (player.maxMP <= player.MP + _item.stat)
                        player.MP = player.maxMP;
                    else
                        player.MP += _item.stat;
                    Debug.Log("�������� ���");
                    break;
                default:
                    break;
            }
            --GameManager.Instance.items[num].count;
            AudioManager.Instance.PlaySFX(potion);
            if (GameManager.Instance.items[num].count <= 0)
                GameManager.Instance.items[num] = null;
            player.Refresh();
        }
        gameScene.RefreshStat();
        slots[num].Refresh();
    }
}
